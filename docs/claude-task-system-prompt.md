# MADai `/claude` Task System Bootstrap

> Paste-ready prompt that bootstraps the autonomous task-execution system into a fresh repo (or re-builds it from scratch). After Claude finishes, the app has:
>
> - A **super-admin-only operator page** at `/app/claude` for queueing tasks, watching them execute live, importing in bulk, and managing prompt templates.
> - A **polling worker** that checks the queue on an adaptive cadence (every minute when active, scaling back to hourly when idle), claims tasks, and either does the work itself or **spawns 2â€“4 parallel agents** within a single iteration when independent tasks are available.
> - A **codebase scanner** that runs hourly, reads the repo, and posts new findings (stubs / TODOs / bugs / gaps) into the queue for the worker to pick up.
> - Real-time updates via SSE so the operator sees status flip `PENDING â†’ IN_PROGRESS â†’ COMPLETED / FAILED` within seconds of the worker acting.

All MADai-specific values are filled in. Copy the block between the `===== PROMPT BEGINS =====` and `===== PROMPT ENDS =====` markers; paste into a fresh Claude Code session at `C:\Code\madai` (or wherever the rebuild target sits).

---

===== PROMPT BEGINS =====

You are the bootstrap agent for **MADai**. Your job is to add a complete autonomous `/claude` task-execution system to the existing application at `C:\Code\madai`.

The system has four parts:
1. **Task queue API** (CRUD + status state machine + bulk-import + real-time stream + attachments) under `/v1/claude-tasks`.
2. **Super-admin-only operator page** at `/app/claude` in the existing Angular frontend.
3. **Polling worker** scheduled by Windows Task Scheduler. Adaptive cadence. Spawns parallel agents within an iteration when 2â€“4 independent tasks can be claimed at once.
4. **Codebase scanner** scheduled hourly by Windows Task Scheduler. Read-only. Posts new findings to the queue.

You will inherit the existing app's conventions:
- **Backend**: NestJS 11 (modular layout under `apps/api/src/modules/<feature>/`), Prisma 5 against MSSQL, `class-validator` DTOs, global `JwtAuthGuard` with constant-time `X-Worker-Token` bypass, `@Public()` decorator for opt-out routes.
- **Frontend**: Angular 19 standalone components, signals + `OnPush`, `FormBuilder` reactive forms, `mc-*` Tailwind primitives defined in `apps/web/src/styles.scss` (`mc-card`, `mc-card-hover`, `mc-btn-primary`, `mc-btn-secondary`, `mc-btn-ghost`, `mc-input`, `mc-label`, `mc-chip`, `mc-heading`, `mc-eyebrow`).
- **Auth**: JWT bearer issued at `/v1/auth/login`. Super-admin check: `req.user.isSuperAdmin === true` (the `User` model already has an `isSuperAdmin: Boolean @default(false)` column).
- **API client**: `ApiService` at `apps/web/src/app/core/services/api.service.ts` - unwraps the `{ ok, data }` envelope.
- **Toast / confirm**: `NotificationService` at `apps/web/src/app/core/services/notification.service.ts` with `ConfirmOverlayComponent` at `apps/web/src/app/core/components/confirm-overlay.component.ts`. Never call `window.confirm()`.
- **Real-time transport**: SSE - `Sse` decorator in NestJS, native `EventSource` in Angular (with `?token=<jwt>` query-param auth since `EventSource` cannot set headers).

Never introduce parallel patterns. Use what's there.

# Polling cadence (worker)

The worker is fired every 1 minute by Task Scheduler. Each fire reads `.claude/worker/state.json` (`{streak, lastFiredAt}`) to decide whether to actually poll the queue, based on this **adaptive backoff ladder**:

| Consecutive empty fetches (streak) | Bucket - minimum seconds between actual polls |
|---|---|
| 0 (just had work) | **60 s** (1 min - fast cadence) |
| 1â€“4 | **300 s** (5 min) |
| 5â€“9 | **600 s** (10 min) |
| 10â€“14 | **1800 s** (30 min) |
| 15+ | **3600 s** (1 hour) |

On every fire:
- If we're inside the current bucket, **skip** (log only, no API call).
- If past the bucket, **poll** `GET https://madaiapi.madleads.ai/v1/claude-tasks/next` with `X-Worker-Token: 4d260e4cee6be56fcac5fc668e7c942d5daf8ee2f4005f4616d241c3753fede5`.
- On `204` (queue empty): increment streak, save state, exit.
- On `200` (work waiting): reset streak to 0, invoke `C:\Users\xy26114\.local\bin\claude.exe --print --dangerously-skip-permissions --add-dir C:\Code\madai` with the worker prompt piped via stdin.

This means the worker is **reactive when active** (â‰¤ 60 s response to a new task) and **cheap when idle** (1 fire/hour after 15 empty fetches, no API call burn).

The scanner runs at a **fixed hourly cadence** - no adaptive logic.

# Parallel-agent execution (within a worker iteration)

After the worker claims its first task A, it inspects the rest of the PENDING queue for up to **3 additional tasks** that are independent of each other AND of A. Independence means:
- No overlapping files (different feature areas / services / pages)
- No shared dependencies (e.g. both editing `prisma/schema.prisma` â†’ NOT independent)
- No ordering requirement in the task descriptions
- Same shape (code-fix tasks batch with code-fix; site-generation tasks always solo)

If a clean batch of 2â€“4 tasks can be formed, the worker:
1. PATCHes every task in the batch to `IN_PROGRESS` (multiple calls in one message).
2. Spawns one `Agent` tool use per task **in a single message with multiple Agent tool uses** so the SDK runs them concurrently. Each agent's prompt contains the repo path, API base, worker token, task ID, task description, and a verbatim status-discipline clause that makes it own its own task's `COMPLETED`/`FAILED` PATCH.
3. Waits for all agents to return.
4. Post-batch sanity sweep - any task stuck on `IN_PROGRESS` gets rescued to `FAILED`.

If no clean batch can be formed, the worker works task A solo. **Better to run 1 task safely than 4 with merge conflicts.**

# Status state machine

| Status | Meaning |
|---|---|
| `PENDING` | Queued, no worker has claimed it. |
| `IN_PROGRESS` | Actively being worked on. |
| `TO_BE_DEPLOYED` | Done locally but not deployed. Rare; usually skipped. |
| `COMPLETED` | Done and (where applicable) deployed. |
| `FAILED` | **Terminal.** Worker tried and couldn't finish. `findNext()` does NOT return it - no infinite retry. Operator triages on `/app/claude`. |
| `CANCELLED` | **Terminal.** Operator decided not to do this. |
| `DEFERRED` | Worker needs operator input. Operator flips back to `PENDING` with notes when ready. |

# Definition of done

The bootstrap is complete when **all** of these are true:

1. **Backend endpoints** under `/v1`:
   - `GET /claude-tasks` - bucketed list (active â†’ to-deploy â†’ terminal)
   - `GET /claude-tasks/next` - 200 `{ok:true, data:{task:{...}}}` or 204
   - `GET /claude-tasks/:id`, `POST /claude-tasks`, `PATCH /claude-tasks/:id` (partial - only `!== undefined` fields written), `DELETE /claude-tasks/:id`
   - `POST /claude-tasks/import-bulk` - dedupe by trim+lowercase title against PENDING+IN_PROGRESS only
   - `POST /claude-tasks/:id/attachments` (multipart, â‰¤ 10 MB Ã— 10 files, MIME allow-list: jpeg, png, webp, gif, pdf, plain, csv, json) + `DELETE .../attachments/:filename`
   - `GET /claude-tasks/events` - SSE stream (`text/event-stream`); auth via `?token=<jwt>` query param (since `EventSource` cannot set headers)
   - `GET / POST / PATCH / DELETE /claude-prompt-templates` (templates CRUD)
   - `GET / PATCH /settings` for worker toggles (`workerActive`, `scannerActive`, `deployNext`)

2. **Auth on every endpoint**: global `JwtAuthGuard` + super-admin check (`req.user.isSuperAdmin === true`) on write operations + `X-Worker-Token` constant-time bypass via `crypto.timingSafeEqual`.

3. **Status transitions validated server-side**: `PENDING â†’ IN_PROGRESS` or `CANCELLED`; `IN_PROGRESS â†’ {COMPLETED, TO_BE_DEPLOYED, FAILED, DEFERRED, PENDING}`; `DEFERRED â†’ PENDING` or `CANCELLED`; terminal statuses (COMPLETED, CANCELLED, FAILED) reject changes without `?override=true`.

4. **Operator page** at `/app/claude` (gated by an `isSuperAdmin` route guard):
   - Header: page icon + "Claude Tasks" title + 4 icon-only action buttons with `title` tooltips - Templates (âŒ§), Import (â‡£), New task (+), Refresh (â†»).
   - Worker toggle row (in an `mc-card`): three switches - **Active** (green), **Scanner** (green), **Deploy Next** (amber). Bind via PATCH to `/v1/settings`.
   - 6 summary cards (grid): Total, In progress (blue), Pending (yellow), To deploy (purple), Completed (green), Failed (red).
   - Status filter dropdown (default `Not completed`) + "Show last N completed" filter (5 / 10 / 25 / 50 / All).
   - Task list: vertically stacked `mc-card-hover` rows with status icon (â³ / â†» / ðŸš€ / âœ“ / âœ• / âš  / â¸), `#id` chip (mono font), title, description (`line-clamp-2`), notes (`line-clamp-1 italic`), status badge, priority badge (hidden when priority=3 normal), attachment count (ðŸ“Ž N), "Updated X ago", hover-revealed delete button.
   - Edit/New modal (`max-w-xl`): title (required, max 300), description, notes, status select, priority select (1=Critical / 2=High / 3=Normal / 4=Low), file picker + drag-drop + paste-from-clipboard. Footer with Cancel + Save (icon-only with tooltips per the icon-only button rule).
   - Import modal (`max-w-2xl`): JSON paste textarea + "Load static file" button (fetches `/outstanding-tasks.json` from `apps/web/public/`).
   - Templates modal (`max-w-2xl`): list of templates with Useâ†’ + Delete buttons + inline create form.
   - Real-time: subscribe to `/v1/claude-tasks/events?token=<jwt>` on `ngOnInit`; on `{type:'task.created'|'task.updated'|'task.deleted'}` events, update `allTasks` signal and emit a brief `NotificationService` toast.
   - Confirm via `NotificationService.confirm(...)` (which opens `ConfirmOverlayComponent`) before delete.

5. **Worker scheduler entry** `MADaiWorker` (Task Scheduler):
   - Fires every 1 minute, `MultipleInstances=IgnoreNew`
   - Adaptive backoff per the ladder above
   - State at `.claude/worker/state.json`, log at `.claude/worker/worker.log`
   - Survives reboots (Task Scheduler persistent registration)

6. **Scanner scheduler entry** `MADaiScanner` (Task Scheduler):
   - Fires every 1 hour, `MultipleInstances=IgnoreNew`
   - State at `.claude/scanner/state.json`, log at `.claude/scanner/scanner.log`

7. **Worker and scanner prompt files** at `.claude/worker/worker-prompt.md` and `.claude/scanner/scanner-prompt.md` - re-read fresh by the scheduler scripts on every fire, so edits land on the next iteration without restart.

8. **Tests** (Jest for API, jasmine/karma for Angular) - happy path + status state machine has explicit tests for every illegal transition.

9. **Docs** - section in `README.md` covering install, token rotation, start/stop/re-enable both schedulers, and log locations.

10. **One end-to-end verification run** (Phase 7) covering create â†’ status flips â†’ real-time UI update â†’ bulk-import dedupe â†’ manual worker fire â†’ manual scanner fire â†’ delete.

# Phases

## Phase 0 - Discovery (READ-ONLY, no production code yet)

Inspect `C:\Code\madai` until you can articulate, one sentence each:
1. **Backend module convention** - where to add `claude-tasks/`, `claude-prompt-templates/`, `settings/` under `apps/api/src/modules/`.
2. **Prisma schema location** - `prisma/schema.prisma`. Confirm enum + JSON column patterns already in use.
3. **DTO + ValidationPipe pattern** - sample one existing module's `dto/*.dto.ts`.
4. **Existing `JwtAuthGuard` location** - `apps/api/src/common/guards/jwt-auth.guard.ts`. Confirm `X-Worker-Token` bypass is already wired (if rebuilding from scratch in a clean repo, you'll wire it here).
5. **Super-admin check pattern** - look for any existing decorator like `@SuperAdminOnly()` or guard like `SuperAdminGuard`. If absent, build a thin one: `@Injectable() class SuperAdminGuard implements CanActivate { canActivate(ctx) { return ctx.switchToHttp().getRequest().user?.isSuperAdmin === true } }`.
6. **`ApiService` envelope contract** - `{ ok: true, data: T } | { ok: false, error: { message } }`. The frontend `ApiService.unwrap` throws on `ok: false`.
7. **Sample admin page to mirror** - `apps/web/src/app/features/admin/admin.page.ts` (or whichever the closest analogue is).
8. **Deploy command** - `pwsh deploy.ps1` (with `-SkipFrontend` / `-SkipBackend` flags). Verify `https://madaiapi.madleads.ai/v1/health` returns 200 right now.
9. **Existing Task Scheduler entries** - none should conflict with `MADaiWorker` / `MADaiScanner` names.

Do NOT write production code until all 9 are clear.

## Phase 1 - Backend data tier

Add to `prisma/schema.prisma`:


```prisma
enum ClaudeTaskStatus {
  PENDING
  IN_PROGRESS
  TO_BE_DEPLOYED
  COMPLETED
  CANCELLED
  FAILED
  DEFERRED
}

model ClaudeTask {
  id          Int      @id @default(autoincrement())
  title       String   @db.VarChar(300)
  description String?  @db.Text
  notes       String?  @db.Text
  status      ClaudeTaskStatus @default(PENDING)
  priority    Int      @default(3)
  attachments Json?
  createdAt   DateTime @default(now())
  updatedAt   DateTime @updatedAt
  @@index([status, priority, id])
}

model ClaudePromptTemplate {
  id          Int      @id @default(autoincrement())
  name        String   @unique @db.VarChar(200)
  description String?  @db.Text
  content     String   @db.Text
  createdAt   DateTime @default(now())
  updatedAt   DateTime @updatedAt
}

model Settings {
  key       String   @id @db.VarChar(100)
  value     Json
  updatedAt DateTime @updatedAt
}
```



Run `npx prisma generate` then `npx prisma db push` (matches the existing deploy.ps1 sync pattern). Seed three rows in `Settings`: `workerActive=true`, `scannerActive=true`, `deployNext=false`.

Worker token: generate 32-byte hex with `[Convert]::ToHexString([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32)).ToLower()` and store in `.env.deploy` under `CLAUDE_WORKER_TOKEN`. The current token in MADai is `4d260e4cee6be56fcac5fc668e7c942d5daf8ee2f4005f4616d241c3753fede5` - re-use it for continuity OR rotate (and update the worker prompt + scanner prompt + worker script in lockstep).

Wire the `X-Worker-Token` bypass into `apps/api/src/common/guards/jwt-auth.guard.ts`:


```ts
override canActivate(context: ExecutionContext) {
  const isPublic = this.reflector.getAllAndOverride<boolean>(IS_PUBLIC_KEY, [
    context.getHandler(), context.getClass(),
  ]);
  if (isPublic) return true;
  const req = context.switchToHttp().getRequest();
  const presented = req.headers?.['x-worker-token'];
  const expected  = process.env.CLAUDE_WORKER_TOKEN;
  if (typeof presented === 'string' && expected &&
      crypto.timingSafeEqual(Buffer.from(presented), Buffer.from(expected))) {
    req.worker = true;
    return true;
  }
  return super.canActivate(context);
}
```



## Phase 2 - Backend API endpoints

Implement under `apps/api/src/modules/claude-tasks/`, `apps/api/src/modules/claude-prompt-templates/`, `apps/api/src/modules/settings/`. Each module: `*.module.ts`, `*.controller.ts`, `*.service.ts`, `dto/*.dto.ts`. Standard NestJS pattern, `@ApiTags` + `@ApiBearerAuth` for Swagger, `@Roles`/`SuperAdminGuard` on write routes.

Critical implementation details:

- **`GET /claude-tasks` is bucketed**: bucket 0 = `PENDING|IN_PROGRESS|DEFERRED` (priority ASC, createdAt ASC); bucket 1 = `TO_BE_DEPLOYED`; bucket 2 = `COMPLETED|CANCELLED|FAILED` (createdAt DESC). Fetch flat with `findMany({ orderBy: [{ priority: 'asc' }, { createdAt: 'asc' }] })`, then sort into buckets in memory - Prisma can't compose CASE expressions.

- **`GET /claude-tasks/next`** picks from bucket 0 only:
  ```ts
  async findNext(): Promise<{ task: any | null }> {
    const task = await this.prisma.claudeTask.findFirst({
      where: { status: { in: ['PENDING', 'IN_PROGRESS', 'DEFERRED'] } },
      orderBy: [{ priority: 'asc' }, { createdAt: 'asc' }],
    });
    return { task: task ?? null };
  }
  ```
  Controller returns `200 {task}` or `204` via `res.status(HttpStatus.NO_CONTENT)`.

- **`PATCH /claude-tasks/:id` is partial** - only fields with `!== undefined` are written. The worker PATCHes bodies like `{status:'IN_PROGRESS'}` and the service must NOT blow away `description`.

- **Route ordering**: `/next`, `/import-bulk`, `/events` MUST come before `/:id` in the controller, otherwise `ParseIntPipe` 400s on the literal segments.

- **`POST /import-bulk` dedupe** is against PENDING + IN_PROGRESS only - let FAILED/COMPLETED tasks be re-queueable once a blocker is resolved.

- **Real-time `events` stream**:
  ```ts
  @Sse('events')
  events(): Observable<MessageEvent> { return this.bus.asObservable(); }
  ```
  An injected `EventEmitter` (or `Subject`) in the service publishes `{type, taskId, task}` on every create/update/delete. Auth: validate the `?token=<jwt>` query param against the existing JWT issuer (since browser `EventSource` cannot set headers).

- **Attachments**: re-use the existing `StorageService`. Store keyed under `claude-tasks-<id>/`.

## Phase 3 - Operator frontend page (`/app/claude`)

Build the standalone component at `apps/web/src/app/features/claude/claude.page.ts`. Inherit:
- Layout: render inside the existing `DashboardLayoutComponent`.
- Primitives: `mc-card`, `mc-card-hover`, `mc-btn-primary`, `mc-btn-secondary`, `mc-btn-ghost`, `mc-input`, `mc-label`, `mc-chip`, `mc-heading`, `mc-eyebrow` from `apps/web/src/styles.scss`.
- API client: `ApiService.get/post/patch/delete`.
- Toast + confirm: `NotificationService`.
- Routing: standalone route at `/app/claude` in `apps/web/src/app/app.routes.ts` with `loadComponent: () => import('./features/claude/claude.page').then(m => m.ClaudePage)`. Protected by a `SuperAdminGuard` (Angular guard that reads `AuthService.currentUser()` and checks `isSuperAdmin`).
- Nav: add a sidebar entry for "Claude Tasks" (icon: âš™) in `DashboardLayoutComponent`, visible only when `currentUser.isSuperAdmin`.

Component structure: `ChangeDetectionStrategy.OnPush`, `signal()`-driven state, `computed()` for derived filteredTasks, reactive `FormBuilder.nonNullable.group(...)` for the edit form.

**Real-time wiring**:

```ts
ngOnInit() {
  this.loadTasks();
  const token = localStorage.getItem('mc_access_token');
  this.events = new EventSource(`${this.api.base}/claude-tasks/events?token=${token}`);
  this.events.addEventListener('message', (ev) => this.zone.run(() => {
    const msg = JSON.parse(ev.data);
    if (msg.type === 'task.deleted') this.allTasks.update(arr => arr.filter(t => t.id !== msg.taskId));
    else this.allTasks.update(arr => {
      const idx = arr.findIndex(t => t.id === msg.task.id);
      if (idx >= 0) { const copy = [...arr]; copy[idx] = msg.task; return copy; }
      return [msg.task, ...arr];
    });
  }));
}
ngOnDestroy() { this.events?.close(); }
```



**Default filter** on page load: `statusFilter.set('not_completed')` so the page doesn't show a wall of done rows.

## Phase 4 - Worker scheduler script + worker prompt

Create `C:\Code\madai\.claude\worker\worker-iteration.ps1`:


```powershell
# MADai autonomous worker -- Task Scheduler entry point.
# ASCII-only -- PowerShell 5.1 mojibakes non-ASCII in UTF-8-no-BOM files.

[CmdletBinding()] param()
$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue'
$workerDir  = $PSScriptRoot
$repoRoot   = (Resolve-Path (Join-Path $workerDir '..\..')).Path
$statePath  = Join-Path $workerDir 'state.json'
$promptPath = Join-Path $workerDir 'worker-prompt.md'
$logPath    = Join-Path $workerDir 'worker.log'
$apiBase    = 'https://madaiapi.madleads.ai/v1'
$token      = '4d260e4cee6be56fcac5fc668e7c942d5daf8ee2f4005f4616d241c3753fede5'
$claudeBin  = "$env:USERPROFILE\.local\bin\claude.exe"

function Write-Log { param([string]$m)
  Add-Content -Path $logPath -Encoding UTF8 -Value ('{0:yyyy-MM-ddTHH:mm:ssZ}  {1}' -f (Get-Date).ToUniversalTime(), $m)
}
function Read-State {
  if (-not (Test-Path $statePath)) { return @{ streak=0; lastFiredAt=[datetime]::MinValue } }
  try {
    $r = Get-Content $statePath -Raw | ConvertFrom-Json
    return @{ streak=[int]$r.streak; lastFiredAt=if ($
