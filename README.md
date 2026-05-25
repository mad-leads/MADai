# MADai - Autonomous AI Operations Platform

> A distributed autonomous AI workforce and operations platform. Users submit any AI request; backend desktop workers powered by Claude Code Desktop (or any executor) claim tasks and execute autonomously.

---

## What's in the box

| Component | Path | Description |
|-----------|------|-------------|
| **Backend solution** | `MADai.sln` | .NET 8 solution containing the API, workers, and supporting libraries. |
| **Backend workspace** | `apps/api` | .NET source workspace. |
| **Domain** | `apps/api/src/MADai.Domain` | Entities, enums, and identity model. |
| **Shared contracts** | `apps/api/src/MADai.Shared` | DTOs and constants shared by API, workers and frontend. |
| **Application** | `apps/api/src/MADai.Application` | MediatR handlers, FluentValidation rules, AutoMapper profiles, business services. |
| **Infrastructure** | `apps/api/src/MADai.Infrastructure` | EF Core (MSSQL) DbContext, configurations, migrations, JWT, file storage, seed data. |
| **API** | `apps/api/src/MADai.Api` | ASP.NET Core REST API, SignalR hubs, Swagger, health checks. |
| **Frontend** | `apps/web` | Angular 19 standalone app with signals and SignalR live updates. |
| **Deploy** | `deploy/` | FTP/Plesk deployment scripts and deploy templates. |
| **Scripts** | `scripts/` | PowerShell helpers for dev, setup and maintenance. |

---

## Architecture

```
┌──────────────┐        ┌────────────────────────────┐
│  Angular 19  │ ─────▶ │  ASP.NET Core 8 Web API    │
│  Frontend    │ ◀──────│  + SignalR hubs            │
└──────────────┘        └──────────────┬─────────────┘
                                       │
                       ┌───────────────┼──────────────────┐
                       │ MSSQL Server  │  Hangfire (jobs) │
                       └───────────────┴──────────────────┘
                                       │
        ┌──────────────────────────────┼──────────────────────────────┐
        │                              │                              │
┌───────▼────────┐           ┌─────────▼────────┐         ┌───────────▼─────────┐
│ Task workers   │ poll API  │ Audit worker     │ scans   │ Hangfire dashboard  │
│ (N replicas)   │ ◀──────▶  │ (Quartz)         │ DB+logs │ (recurring jobs)    │
└────────────────┘           └──────────────────┘         └─────────────────────┘
```

Workers authenticate via the `X-API-Key` header against a Worker authentication scheme - separate from the JWT scheme used by humans.

---

## Quick start (local development)

### Prerequisites
- .NET 8 SDK (`dotnet --version` ≥ 8.0.400)
- Node 22+ and npm 10+
- SQL Server (LocalDB on Windows or managed MSSQL)
- ~1 GB free disk for `node_modules`

### 1. Restore & build
```powershell
dotnet restore
dotnet build
```

### 2. Apply migrations (auto on startup, or manually)
```powershell
dotnet ef database update -p apps/api/src/MADai.Infrastructure -s apps/api/src/MADai.Api
```

### 3. Run everything
```powershell
./scripts/dev-up.ps1
```

This launches API, Worker, Audit Worker, and the Angular dev server in separate windows.

Manual equivalents:
```powershell
dotnet run --project apps/api/src/MADai.Api --urls http://localhost:3011

pnpm install
pnpm web:start                                    # http://localhost:4211
```

### 4. Default credentials
A SystemAdmin user is seeded on first run:

```
Email:    admin@madai.local
Password: ChangeMe!2025
```

Change it immediately in production.

---

## Persistent workers

Persistent repository-aware worker APIs live at `/api/v1/persistent-workers`.
The SystemAdmin UI is available at `/persistent-workers`. No auto-start or
crash-recovery service is registered by this repository.

---

## Key endpoints

| Path | Description |
|------|-------------|
| `GET /health` | Application + dependency health |
| `GET /health/ready` | Readiness probe |
| `GET /swagger` | OpenAPI (dev only) |
| `POST /api/v1/auth/login` | Email/password login |
| `POST /api/v1/auth/refresh` | Refresh access token |
| `GET  /api/v1/tasks` | List tasks (paginated, filterable) |
| `POST /api/v1/tasks` | Create task |
| `POST /api/v1/tasks/{id}/retry` | Retry / re-queue |
| `POST /api/v1/tasks/{id}/cancel` | Cancel task |
| `POST /api/v1/worker/register` | Register a worker, returns `X-API-Key` |
| `POST /api/v1/worker/claim` | Claim queued tasks (worker auth) |
| `POST /api/v1/worker/tasks/{id}/progress` | Report progress |
| `POST /api/v1/worker/tasks/{id}/complete` | Mark complete |
| `POST /api/v1/worker/tasks/{id}/fail` | Report failure |
| `GET  /api/v1/dashboard/overview` | System overview metrics |
| `GET  /api/v1/audit/findings` | Self-healing audit findings |

SignalR hubs:

| Hub | Purpose |
|-----|---------|
| `/hubs/tasks` | Task lifecycle events (Created, Progress, Completed, Failed) |
| `/hubs/workers` | Worker heartbeats / status changes |
| `/hubs/notifications` | User-targeted notifications |
| `/hubs/dashboard` | Aggregate dashboard refresh signals |

---

## Database

- All major tables include `CreatedDate`, `ModifiedDate`, `DeletedDate`, `CreatedByUserId`, `ModifiedByUserId`, `IsDeleted` (`AuditableEntity`).
- Soft-deletes via global query filter; an EF Core interceptor (`AuditingInterceptor`) writes audit columns and converts deletes into soft-deletes.
- Concurrency via `RowVersion` (`byte[] timestamp`) on auditable entities.
- Migrations live in `apps/api/src/MADai.Infrastructure/Persistence/Migrations`.

Initial seed (`DataSeeder.cs`) provisions:
- All 7 roles (`SystemAdmin`, `CompanyAdmin`, `CompanyManager`, `Worker`, `User`, `Client`, `ReadOnly`)
- ~19 permissions with role grants
- 3 plans (Starter / Pro / Enterprise)
- Demo company "MAD Products"
- Admin user
- A "Welcome to MADai" sample task

---

## Worker lifecycle

1. Worker boots, posts `WorkerRegisterRequest` to `/api/v1/worker/register`.
2. API returns `WorkerId` + one-time `ApiKey` (stored hashed in DB).
3. Worker stores the key in memory only and sends `X-API-Key` on subsequent calls.
4. `HeartbeatWorker` posts periodic heartbeats.
5. `TaskPollingWorker` calls `/api/v1/worker/claim` up to MaxConcurrency tasks.
6. For each claim, `TaskExecutor` creates `/workspaces/{id}/{input,output,logs,artifacts,temp}`, runs the task, reports progress, completes or fails.
7. On failure, the API decides retry-with-backoff vs. dead-letter based on `MaxRetries`.

This baseline executor produces a deterministic textual result; replace `TaskExecutor.RunCategoryAsync` with your Claude Code Desktop bridge to obtain real generation.

---

## Self-healing audit worker

`MADai.AuditWorker` runs a Quartz job (default every 15 minutes) and scans:

- Repeat-failure clusters (same task failing N times in 24h)
- Dead-letter queue depth
- Tasks stuck in `Running` >2h (orphaned worker claims)
- Orphaned artifacts older than N hours
- Queue backlog (>100 pending in any queue)

For each `High`/`Critical` finding it auto-creates a new task in the `audit` queue (`Category = SelfHealing`) with the recommendation embedded - closing the loop.

---

## Frontend

The Angular 19 app uses:

- **Standalone components** + **Angular signals**
- **Tailwind CSS** with a custom dark theme (`tailwind.config.js`)
- **Angular Material** primitives where useful
- **@microsoft/signalr** for live task/worker/dashboard updates
- **JWT auth** with refresh-token rotation in `localStorage`
- **Functional guards** and **functional HTTP interceptors**

Pages included: Dashboard, Tasks list/detail/create, Workers, Self-healing, Files, Notifications, Admin, Settings, plus auth (Login/Register/Forgot/MFA) inside an auth layout.

---

## Production checklist

- [ ] Replace `Jwt:Secret` with a 64+ char random value (use `openssl rand -hex 64`).
- [ ] Replace the seeded `admin@madai.local` password.
- [ ] Switch `Storage:Provider` to `Azure` or `S3` and configure credentials.
- [ ] Configure a real `IEmailSender` implementation (the supplied `NullEmailSender` only logs).
- [ ] Front the API with HTTPS (reverse proxy or Kestrel cert).
- [ ] Configure `Cors:Origins` for your real frontend domain(s).
- [ ] Enable rate limiting at the edge.
- [ ] Move from LocalDB to a managed MSSQL instance.

---

## Known caveats

- **AutoMapper 12.0.1** is used because newer versions are commercial. The 12.x package has an advisory (GHSA-rvv3-g6hj-g44x); for production swap to **Mapster** (drop-in for the `Project*` use cases) or upgrade to a licensed AutoMapper.
- **`TaskExecutor.RunCategoryAsync`** is a baseline executor that writes a templated text artifact. Wire it to your Claude Code Desktop bridge to produce real output.
- **Hangfire** is wired up via packages but no recurring jobs are scheduled by default. Add them in `Program.cs` when needed.
- **Frontend packages** use pnpm with `store-dir=C:/Code/.pnpm`. Run `pnpm install` from the repo root.

---

## `/claude` task system (developer self-improvement queue)

Separate from the end-user `TaskItem` queue. The `ClaudeTask` table holds work items that the **Claude Code CLI** picks up and writes against this very repo (bug fixes, refactors, new features). Operated from `https://madai.madprospects.com/app/claude` (SystemAdmin only).

### Components

| Path | Purpose |
|------|---------|
| `apps/api/src/MADai.Domain/Tasks/ClaudeTask.cs` + `ClaudeTaskEnums.cs` | Entity + status/priority enums (`Pending`, `InProgress`, `ToBeDeployed`, `Completed`, `Failed`, `Cancelled`, `Deferred`). |
| `apps/api/src/MADai.Infrastructure/Persistence/Migrations/*_ClaudeTaskSystem.cs` | EF Core migration creating `ClaudeTasks` + `ClaudePromptTemplates`. |
| `apps/api/src/MADai.Application/Features/ClaudeTasks/` | MediatR queries + commands + the `ClaudeTaskTransitions` state-machine gate. |
| `apps/api/src/MADai.Api/Controllers/ClaudeTasksController.cs` | REST API at `/api/v1/claude-tasks`. Worker uses `X-API-Key`; humans use JWT + `SystemAdmin` role. |
| `apps/api/src/MADai.Api/Controllers/ClaudePromptTemplatesController.cs` | CRUD for reusable prompt snippets. |
| `apps/api/src/MADai.Api/Controllers/SettingsController.cs` | Generic key/value store; the page reads `claudeWorkerActive`, `claudeScannerActive`, `claudeDeployNext`. |
| `apps/web/src/app/pages/claude/` | Angular operator page at `/app/claude`. Real-time updates via the existing `/hubs/tasks` SignalR hub, event `claudeTaskUpdated`. |
| `.claude/worker/worker-iteration.ps1` + `worker-prompt.md` | Adaptive-cadence PowerShell worker (60s → 1h backoff) that fires `claude.exe` when there's work. |
| `.claude/scanner/scanner-iteration.ps1` + `scanner-prompt.md` | Hourly read-only scanner that posts findings to `/api/v1/claude-tasks/import-bulk`. |
| `scripts/register-claude-schedulers.ps1` | Registers `MADaiClaudeWorker` + `MADaiClaudeScanner` in Windows Task Scheduler. |

### Install (one-time)

```powershell
# 1. Generate a worker token and add it to .env at the repo root
$token = [Convert]::ToHexString([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32)).ToLower()
Add-Content -Path C:\Code\madprospects\madai\.env -Value "CLAUDE_WORKER_TOKEN=$token"

# 2. Apply the migration (or just let the API auto-migrate on startup)
dotnet ef database update -p apps/api/src/MADai.Infrastructure -s apps/api/src/MADai.Api

# 3. Confirm Claude CLI is installed at the expected path
Get-Command claude
# Expected: $env:USERPROFILE\.local\bin\claude.exe

# 4. Register both scheduler entries (Worker every 1 min, Scanner every 1 hour)
pwsh ./scripts/register-claude-schedulers.ps1

# 5. (After API redeploy) verify in browser at /app/claude
#    The "claude-code-bootstrap" WorkerNode is seeded automatically because we have
#    CLAUDE_WORKER_TOKEN set; the existing X-API-Key auth handler accepts it.
```

### Scheduler control

```powershell
# Manually fire the worker once (useful for verification)
Start-ScheduledTask -TaskName MADaiClaudeWorker

# Manually fire the scanner once
Start-ScheduledTask -TaskName MADaiClaudeScanner

# Disable both while doing a major manual refactor
Disable-ScheduledTask -TaskName MADaiClaude*

# Re-enable
Enable-ScheduledTask -TaskName MADaiClaude*

# Remove both entirely
Unregister-ScheduledTask -TaskName MADaiClaudeWorker  -Confirm:$false
Unregister-ScheduledTask -TaskName MADaiClaudeScanner -Confirm:$false
```

### Logs and state

- `.claude/worker/worker.log` - every fire (`SKIP` / `FIRE` / `EMPTY` / `WORK` / `DONE`) plus full Claude CLI stdout.
- `.claude/worker/state.json` - `{streak, lastFiredAt}`. Both are gitignored; the prompts and scripts are committed.
- `.claude/scanner/scanner.log`, `.claude/scanner/state.json` - same convention.

### Adaptive backoff (worker)

| Consecutive empty fetches | Minimum interval between actual API polls |
|---|---|
| 0 (just had work) | 60 s |
| 1–4  | 5 min |
| 5–9  | 10 min |
| 10–14 | 30 min |
| 15+  | 1 hour |

Task Scheduler fires the worker every minute; the script skips early if it's still inside the current bucket. Net effect: ≤ 60 s response to a new task, ~1 API call/hour when fully idle.

### Token rotation

```powershell
# 1. Generate a new token and overwrite the .env value
$token = [Convert]::ToHexString([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32)).ToLower()
(Get-Content C:\Code\madprospects\madai\.env) -replace '^CLAUDE_WORKER_TOKEN=.*', "CLAUDE_WORKER_TOKEN=$token" |
  Set-Content -Encoding UTF8 -Path C:\Code\madprospects\madai\.env

# 2. Redeploy (the deploy script injects the new token into web.config; DataSeeder reseeds the WorkerNode hash on next start)
pwsh ./deploy/deploy-api.ps1
```

### Distinction from `MADai.Worker`

`TaskItem` is the **end-user** task queue. It is unrelated to the `/claude` system: different table, different worker process, different lifecycle, different audience. They coexist deliberately.

---

## Licence

Internal MAD Products project. All rights reserved unless agreed otherwise.
