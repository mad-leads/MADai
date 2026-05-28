# MADai `/claude` Task System Bootstrap (MADai-stack edition)

> Sibling of `claude-task-system-prompt.md` but adapted to MADai's actual stack:
> **.NET 8 ASP.NET Core + EF Core + MSSQL + Angular 19 + SignalR**, NOT NestJS / Prisma / MSSQL / SSE.
>
> Adds a separate `ClaudeTask` queue (parallel to the existing `TaskItem` table - kept distinct so end-user AI tasks don't mix with developer self-improvement tasks), an operator page at `/app/claude`, an adaptive-cadence PowerShell worker that calls the Claude CLI, and a codebase scanner. Live updates ride the existing SignalR infrastructure, not SSE.

Copy from `===== PROMPT BEGINS =====` to `===== PROMPT ENDS =====`. Paste into a fresh Claude Code session at `C:\Code\madai`.

---

===== PROMPT BEGINS =====

You are the bootstrap agent for **MADai**. Your job is to add a complete autonomous `/claude` task-execution system to the existing application at `C:\Code\madai`.

MADai is **.NET 8 ASP.NET Core + EF Core + MSSQL + Angular 19 + SignalR**. Inherit every existing convention; do not introduce parallel stacks.

The system has four parts:
1. **ClaudeTask queue API** (CRUD + status state machine + bulk-import + attachments) under `/api/v1/claude-tasks`.
2. **SystemAdmin-only operator page** at `/app/claude` in the existing Angular frontend.
3. **Polling worker** scheduled by Windows Task Scheduler. Adaptive cadence. Spawns parallel agents within an iteration when 2–4 independent tasks can be claimed at once.
4. **Codebase scanner** scheduled hourly by Windows Task Scheduler. Read-only. Posts new findings to the queue.

# Existing app conventions to inherit (do not deviate)

- **Solution layout** (Clean Architecture):
  - `src/MADai.Domain/` - entities + enums (`Tasks/`, `Identity/`, `Workers/` etc.)
  - `src/MADai.Application/` - MediatR handlers, FluentValidation, AutoMapper, business services. DI via `AddApplication()`.
  - `src/MADai.Infrastructure/` - `MADaiDbContext`, EF Core configurations under `Persistence/Configurations/`, migrations under `Persistence/Migrations/`. DI via `AddInfrastructure(IConfiguration)`.
  - `src/MADai.Api/` - ASP.NET Core Web API. Controllers under `Controllers/`. SignalR hubs under `Hubs/`. JWT bearer + a custom `Worker` authentication scheme (`X-API-Key` header).
  - `src/MADai.Shared/` - DTOs and constants used by both server and client.
- **DTO contract**: every API response is wrapped in `MADai.Shared.Contracts.ApiResult<T>` with shape `{ success: bool, data: T?, error: string?, issues: string[]? }`. Frontend `ApiService` unwraps with `.data` and throws when `success === false`.
- **DTOs are positional `sealed record`s** - note the known limitation: AutoMapper's `ProjectTo<T>` cannot construct them. Use explicit `.Select(t => new TDto(...))` for any EF Core projection.
- **MediatR** for command/query - handlers live under `src/MADai.Application/Features/<feature>/{Commands,Queries}/`. `ValidationBehavior<TRequest,TResponse>` is already wired so adding a `FluentValidation.AbstractValidator<TCommand>` automatically validates on send.
- **Auth**:
  - JWT bearer scheme is the default (`builder.Services.AddAuthentication().AddJwtBearer(...)`).
  - A separate `Worker` scheme reads `X-API-Key` from the header and validates with `crypto.timingSafeEqual`-equivalent via `CryptographicOperations.FixedTimeEquals`. Wired in `src/MADai.Api/Authentication/WorkerAuthHandler.cs`. Policy: `[Authorize(AuthenticationSchemes = MADaiConstants.AuthSchemeWorker, Policy = "WorkerOnly")]`.
  - Role-based for humans: `[Authorize(Roles = "SystemAdmin")]`. Role constants in `MADai.Domain.Identity.Roles`. `SystemAdmin` is the highest tier; treat it as the super-admin.
  - Allow anonymous endpoints: `[AllowAnonymous]`.
- **Real-time**: SignalR (not SSE). Existing hubs:
  - `/hubs/tasks` (`TasksHub`)
  - `/hubs/workers` (`WorkersHub`)
  - `/hubs/notifications` (`NotificationsHub`)
  - `/hubs/dashboard` (`DashboardHub`)
  Publisher abstraction: `IEventPublisher` in `MADai.Application.Abstractions`. The SignalR implementation lives at `src/MADai.Api/SignalR/SignalREventPublisher.cs`. Reuse it - do not invent SSE.
- **Frontend**:
  - Angular 19 standalone components, signals + `ChangeDetectionStrategy.OnPush`.
  - Routes in `frontend/src/app/app.routes.ts`. Two layouts: `MainLayout` (authenticated) and `AuthLayout` (login etc.).
  - `frontend/src/app/core/http/api.service.ts` - `ApiService.get/post/put/delete/upload(...)` unwraps the `ApiResult<T>` envelope.
  - `frontend/src/app/core/realtime/signalr.service.ts` - `SignalRService.connect(path)` returns a `signalR.HubConnection` with the JWT attached via `accessTokenFactory`.
  - `frontend/src/app/core/auth/auth.service.ts` - `AuthService.user` is a `signal<UserSummary | null>`. Check `user().roles.includes('SystemAdmin')` for the super-admin gate.
  - `frontend/src/app/core/ui/toast.service.ts` - `ToastService.success/error/warning/info(message)`. There is no `confirm()` overlay yet; if needed, build one inline.
  - **There is no `mc-*` Tailwind primitive system** in MADai. Use the existing classes from `frontend/src/styles.scss`: `glass`, `glass-card`, `gradient-text`, the custom Tailwind theme (`bg-ink-50`, `bg-ink-100/80`, `from-primary-500 to-accent-400`, `shadow-glow`, `animate-fade-in`, `animate-slide-up`). Mirror the styling already in `frontend/src/app/pages/tasks/tasks-list.ts` and `frontend/src/app/pages/dashboard/dashboard.html`.
- **Deploy**: `./scripts/deploy-api.ps1` (Plesk FTP, with secret injection from `.env`). Health probe: `https://madaiapi.madproducts.com/health`. Frontend: `./scripts/deploy-frontend.ps1` to `https://madai.madproducts.com`.

# Why a separate `ClaudeTask` table (and not reusing `TaskItem`)

MADai's existing `TaskItem` entity (`src/MADai.Domain/Tasks/TaskItem.cs`) is the **end-user AI task queue** - users submit "write me a blog post" / "generate image" / "scrape this site" and the production `MADai.Worker` claims and executes them. That queue must never mix with developer self-improvement tasks like "fix the dashboard 500 bug" - wrong worker, wrong execution semantics, wrong audience.

So this bootstrap adds a parallel queue. Same patterns, separate table:

```
ClaudeTask  ────────────  ClaudeTaskAttachment (Json column inside, no separate table)
            ────────────  ClaudePromptTemplate
            ────────────  SystemSetting (already exists; add new keys)
```

# Polling cadence (worker)

The worker is fired every 1 minute by Task Scheduler. Each fire reads `.claude/worker/state.json` (`{streak, lastFiredAt}`) to decide whether to actually poll, using this **adaptive backoff ladder**:

| Consecutive empty fetches (streak) | Bucket - minimum seconds between actual polls |
|---|---|
| 0 (just had work) | **60 s** |
| 1–4 | **300 s** |
| 5–9 | **600 s** |
| 10–14 | **1800 s** |
| 15+ | **3600 s** |

On every fire:
- If we're inside the current bucket, **skip** (log only, no API call).
- If past the bucket, **poll** `GET https://madaiapi.madproducts.com/api/v1/claude-tasks/next` with `X-API-Key: <CLAUDE_WORKER_TOKEN>` header.
- On `204`: increment streak, save state, exit.
- On `200`: reset streak to 0, invoke `claude.exe --print --dangerously-skip-permissions --add-dir C:\Code\madai` with the worker prompt piped via stdin.

Scanner runs at a **fixed hourly cadence** - no adaptive logic.

# Parallel-agent execution

After claiming task A, the worker inspects the rest of the PENDING queue for up to **3 additional tasks** that are independent of A and of each other. Independence means:
- No overlapping files (different feature areas / services / pages)
- No shared dependencies (e.g. both editing `src/MADai.Domain/` enums → NOT independent)
- No ordering requirement in the descriptions
- Same shape (code-fix batches with code-fix; deploy/migration tasks always solo)

If a clean batch of 2–4 tasks can be formed, the worker:
1. PATCHes every task in the batch to `InProgress` (multiple calls in one message).
2. Spawns one `Agent` tool use per task **in a single message with multiple Agent tool uses** so they run concurrently. Each agent's prompt contains the repo path, API base, worker token, task ID, task description, and a verbatim status-discipline clause.
3. Waits for all agents to return.
4. Post-batch sanity sweep - any task stuck on `InProgress` gets rescued to `Failed`.

If no clean batch can be formed, the worker works task A solo. **Better to run 1 task safely than 4 with merge conflicts.**

# ClaudeTaskStatus state machine

| Status | Meaning |
|---|---|
| `Pending` | Queued, no worker has claimed it. |
| `InProgress` | Actively being worked on. |
| `ToBeDeployed` | Done locally but not deployed. Rare; usually skipped. |
| `Completed` | Done and (where applicable) deployed. |
| `Failed` | **Terminal.** `/next` does NOT return it - no infinite retry. Operator triages on `/app/claude`. |
| `Cancelled` | **Terminal.** Operator decided not to do this. |
| `Deferred` | Worker needs operator input. Operator flips back to `Pending` with notes when ready. |

Transitions enforced server-side:
- `Pending → InProgress | Cancelled`
- `InProgress → Completed | ToBeDeployed | Failed | Deferred | Pending`
- `Deferred → Pending | Cancelled`
- Terminal (`Completed`, `Cancelled`, `Failed`) rejects further changes without `?override=true`.

# Definition of done

The bootstrap is complete when **all** of these are true:

1. **Backend endpoints** under `/api/v1`:
   - `GET /claude-tasks` - bucketed list (active → to-deploy → terminal), paged & filterable
   - `GET /claude-tasks/next` - 200 `ApiResult<ClaudeTaskDto>` or 204
   - `GET /claude-tasks/{id}`, `POST /claude-tasks`, `PATCH /claude-tasks/{id}` (partial - only non-null fields written), `DELETE /claude-tasks/{id}`
   - `POST /claude-tasks/import-bulk` - dedupe by trim+lowercase title against `Pending`+`InProgress` only
   - `POST /claude-tasks/{id}/attachments` (multipart, ≤ 10 MB × 10 files, MIME allow-list: jpeg, png, webp, gif, pdf, plain, csv, json) + `DELETE .../attachments/{filename}`
   - `GET / POST / PATCH / DELETE /claude-prompt-templates`
   - `GET / PATCH /settings` for `claudeWorkerActive`, `claudeScannerActive`, `claudeDeployNext` (stored in the existing `SystemSetting` table by key)
2. **Auth**: every endpoint requires `[Authorize]`. Write endpoints add `[Authorize(Roles = "SystemAdmin")]`. The worker bypasses via the existing `Worker` authentication scheme + `X-API-Key` header (reuse `WorkerAuthHandler`).
3. **Real-time**: extend `SignalREventPublisher` with `PublishClaudeTaskUpdatedAsync(...)` that broadcasts `{type, taskId, task}` on the existing `/hubs/tasks` hub under a `claudeTaskUpdated` event (or a new `/hubs/claude-tasks` hub if you prefer isolation - either works, document the choice).
4. **Operator page** at `/app/claude`:
   - Gated by a `SystemAdminGuard` that reads `AuthService.user().roles.includes('SystemAdmin')`.
   - Visually mirrors `frontend/src/app/pages/tasks/tasks-list.ts` - same `glass-card` rows, same table density, same status badge colours from `TASK_STATUS_COLOR` (extend with `ClaudeTask`-equivalent map).
   - Header row: page title + 4 icon-only action buttons (Templates, Import, New, Refresh).
   - Worker-toggles row (in a `glass` card): 3 switches - `Active` (green), `Scanner` (green), `Deploy Next` (amber). PATCH to `/api/v1/settings`.
   - 6 summary cards: Total / In progress (blue) / Pending (yellow) / To deploy (purple) / Completed (green) / Failed (red).
   - Status filter dropdown defaulting to `Not completed` + "show last N completed" filter.
   - Task list rows with status icon + `#id` mono chip + title + description (line-clamp-2) + notes (line-clamp-1 italic) + status badge + priority badge + attachment count + "updated X ago" + hover delete.
   - Edit/New modal: title (req, max 300), description, notes, status select, priority select (1=Critical / 2=High / 3=Normal / 4=Low), file picker with drag-drop and clipboard-paste.
   - Import modal: JSON paste textarea + "load static file" button (fetches `/outstanding-tasks.json` from `frontend/public/`).
   - Templates modal: list with Use→ + Delete, inline create form.
   - Real-time: `SignalRService.connect('/hubs/tasks')` in `ngOnInit`, listen to `claudeTaskUpdated`. Update `allTasks` signal on each event.
5. **Worker scheduler** `MADaiClaudeWorker` (every 1 minute, `MultipleInstances=IgnoreNew`, adaptive backoff).
6. **Scanner scheduler** `MADaiClaudeScanner` (every 1 hour, `MultipleInstances=IgnoreNew`).
7. **Worker + scanner prompt files** at `.claude/worker/worker-prompt.md` and `.claude/scanner/scanner-prompt.md` - re-read on every fire so edits land on the next iteration.
8. **Tests**: xUnit for backend (handler tests + illegal-transition tests for every forbidden state move). Frontend tests skipped unless trivial - note in the README.
9. **README section** covering install, token rotation, scheduler control, and log locations.
10. **One end-to-end verification run** (Phase 7).

# Phases

## Phase 0 - Discovery (READ-ONLY, no production code yet)

Inspect `C:\Code\madai` until you can articulate, one sentence each:
1. **EF Core entity convention** - `src/MADai.Domain/Tasks/TaskItem.cs` is the canonical example. Confirm `AuditableEntity` base, `TenantEntity` (with `CompanyId`) pattern.
2. **EF Core configuration convention** - `src/MADai.Infrastructure/Persistence/Configurations/TaskItemConfiguration.cs` is the canonical example. `IEntityTypeConfiguration<T>` with column lengths, indexes, FK behaviours.
3. **DbContext wiring** - `src/MADai.Infrastructure/Persistence/MADaiDbContext.cs` exposes `DbSet<>` properties; new entities must be added there. `IDbContextAccess` in `src/MADai.Application/Abstractions/IDbContextAccess.cs` is the abstraction handlers use - extend it for any new DbSet they need.
4. **Migration command** - from repo root: `dotnet ef migrations add <Name> -p src/MADai.Infrastructure -s src/MADai.Api -o Persistence/Migrations`. Then `dotnet ef database update` (auto-applied at startup via `DataSeeder.MigrateAsync()`).
5. **Controller convention** - `src/MADai.Api/Controllers/TasksController.cs` is the canonical example. `[ApiController]`, `[Route("api/v{version:apiVersion}/<name>")]`, `[Asp.Versioning.ApiVersion("1.0")]`, `IMediator _mediator`, returns `ActionResult<ApiResult<T>>`.
6. **MediatR command/query convention** - `src/MADai.Application/Features/Tasks/Commands/CreateTaskCommand.cs` is canonical. Command record + `AbstractValidator` + handler. Throw `ConflictException` / `NotFoundException` / `ForbiddenException` from `MADai.Application.Common` - the `ExceptionMiddleware` maps them to HTTP codes.
7. **Existing `X-API-Key` worker auth flow** - `src/MADai.Api/Authentication/WorkerAuthHandler.cs` reads the header and matches against `WorkerNode.ApiKeyHash`. **You will REUSE this** - register one ClaudeTask worker node with a known machine name (e.g. `claude-code-bootstrap`) and use its issued key as `CLAUDE_WORKER_TOKEN`. Do not invent a second token system.
8. **Existing SignalR `IEventPublisher`** - `src/MADai.Application/Abstractions/IEventPublisher.cs`. Implementation at `src/MADai.Api/SignalR/SignalREventPublisher.cs`. Add a new method here OR add a new hub at `/hubs/claude-tasks` and a new publisher method - both are valid; choose and document.
9. **Deploy command** - `./scripts/deploy-api.ps1` (pure PowerShell + FTP + `.env` secret injection into `web.config`). Verify `https://madaiapi.madproducts.com/health` returns 200 before starting any code changes.

Do NOT write production code until all 9 are clear.

## Phase 1 - Backend data tier

Add three new entities under `src/MADai.Domain/Tasks/`:

```csharp
// src/MADai.Domain/Enums/ClaudeTaskEnums.cs
namespace MADai.Domain.Enums;

public enum ClaudeTaskStatus
{
    Pending = 0,
    InProgress = 10,
    ToBeDeployed = 20,
    Completed = 30,
    Failed = 40,
    Cancelled = 50,
    Deferred = 60
}

public enum ClaudeTaskPriority
{
    Critical = 1,
    High = 2,
    Normal = 3,
    Low = 4
}
```

```csharp
// src/MADai.Domain/Tasks/ClaudeTask.cs
using MADai.Domain.Common;
using MADai.Domain.Enums;

namespace MADai.Domain.Tasks;

public class ClaudeTask : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public ClaudeTaskStatus Status { get; set; } = ClaudeTaskStatus.Pending;
    public ClaudeTaskPriority Priority { get; set; } = ClaudeTaskPriority.Normal;
    public string? AttachmentsJson { get; set; }   // JSON array of {fileName, storagePath, contentType, sizeBytes}
}

public class ClaudePromptTemplate : AuditableEntity
{
    public string Name { get; set; } = string.Empty;        // unique
    public string? Description { get; set; }
    public string Content { get; set; } = string.Empty;     // the template body
}
```

Add EF Core configuration following the existing pattern in `src/MADai.Infrastructure/Persistence/Configurations/`:

```csharp
// src/MADai.Infrastructure/Persistence/Configurations/ClaudeTaskConfiguration.cs
public class ClaudeTaskConfiguration : IEntityTypeConfiguration<ClaudeTask>
{
    public void Configure(EntityTypeBuilder<ClaudeTask> b)
    {
        b.ToTable("ClaudeTasks");
        b.HasKey(t => t.Id);
        b.Property(t => t.Title).IsRequired().HasMaxLength(300);
        b.Property(t => t.Description);
        b.Property(t => t.Notes);
        b.Property(t => t.AttachmentsJson).HasColumnType("nvarchar(max)");
        b.HasIndex(t => new { t.Status, t.Priority, t.Id });
    }
}
```

Add `DbSet<ClaudeTask>` and `DbSet<ClaudePromptTemplate>` to `MADaiDbContext` and to the `IDbContextAccess` interface.

Generate migration: `dotnet ef migrations add ClaudeTaskSystem -p src/MADai.Infrastructure -s src/MADai.Api -o Persistence/Migrations`.

Worker token: generate one via PowerShell `[Convert]::ToHexString([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32)).ToLower()` and store in `.env` as `CLAUDE_WORKER_TOKEN=...`. Then register a `WorkerNode` for the Claude worker in `DataSeeder` (mark `IsActive=true`, `MachineName='claude-code-bootstrap'`, `Capabilities='ClaudeWorker'`, set `ApiKeyHash` to `WorkerApiKeyHasher.Hash(<token>)`). This makes the existing `WorkerAuthHandler` accept the same token without any new auth code.

In `DataSeeder.SeedAsync`, also seed the three settings keys: `claudeWorkerActive=true`, `claudeScannerActive=true`, `claudeDeployNext=false`.

## Phase 2 - Backend API + MediatR handlers

Create the feature folder `src/MADai.Application/Features/ClaudeTasks/{Commands,Queries}/`:

- `GetClaudeTasksQuery.cs` (paged + filterable; returns `PagedResult<ClaudeTaskSummaryDto>`). **DO NOT use `ProjectTo<T>`** - positional record DTOs fail at runtime with AutoMapper. Use explicit `.Select(t => new ClaudeTaskSummaryDto(t.Id, t.Title, ...))`.
- `GetClaudeTaskByIdQuery.cs` (returns `ClaudeTaskDetailDto` or throws `NotFoundException`).
- `GetNextClaudeTaskQuery.cs` (returns `ClaudeTaskDto?`; service-layer logic is `FirstOrDefault(t => t.Status == Pending || t.Status == InProgress || t.Status == Deferred, orderBy: Priority asc then CreatedDate asc)`).
- `CreateClaudeTaskCommand.cs` (with `AbstractValidator<CreateClaudeTaskCommand>` that requires Title, max 300; description/notes ≤ 8000).
- `UpdateClaudeTaskCommand.cs` (partial - only non-null fields applied; enforces the state-machine transitions, throws `ConflictException` on illegal moves unless `Override=true`).
- `DeleteClaudeTaskCommand.cs`.
- `ImportBulkClaudeTasksCommand.cs` - body `{ items: [{ title, description?, notes?, priority? }] }`. Dedupe by `Title.Trim().ToLowerInvariant()` against existing `Pending`+`InProgress` rows. Returns `{ created: int, skipped: int, items: ClaudeTaskSummaryDto[] }`.

Add `ClaudeTasksController` mirroring `TasksController`:

```csharp
[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/claude-tasks
