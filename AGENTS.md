# MADai — Codex project notes

Autonomous AI ops platform. .NET 8 API + workers, Angular 19 frontend, MSSQL,
Hangfire. Deployed to 1-grid Plesk (FTP-based).

For a full architecture overview, see `README.md` at the repo root — this file
is the short cheat-sheet for Codex / future-you.

## Canonical infrastructure values (source of truth: `C:\Code\madprospects\Setup.xlsx`)

| Thing             | Value                                      |
|-------------------|--------------------------------------------|
| Frontend URL      | `https://madai.madprospects.com`         |
| API URL           | `https://madaiapi.madprospects.com`      |
| SQL host          | `WINSVRSQL03.hostserv.co.za,1433`          |
| App DB            | `madai`                                    |
| Hangfire DB       | `madaihangfire`                            |
| Hangfire dashboard| `/hangfire` (requires JWT + SystemAdmin)   |
| FTP host          | `41.185.110.61`                            |
| API FTP path      | `/madaiapi.madprospects.com`             |
| FE FTP path       | `/madai.madprospects.com`                |

DB and SQL host are **not reachable from dev machines** — only from the
deployed 1-grid box. Verify DB connectivity via the `/health/ready` endpoint
after deploy.

## Layout (only the bits you'll touch most)

| Path                                | Purpose                                    |
|-------------------------------------|--------------------------------------------|
| `apps/api/src/MADai.Api/`           | ASP.NET Core 8 Web API, SignalR, Hangfire  |
| `apps/api/src/MADai.Api/Program.cs` | Service registration + middleware pipeline |
| `apps/api/src/MADai.Api/appsettings.json` | Local-dev defaults (LocalDB). Prod conn strings come from web.config env-vars injected by `deploy/deploy-api.ps1` from `.env` |
| `apps/web/`                         | Angular 19 frontend, pnpm workspace        |
| `apps/web/src/environments/environment.prod.ts` | FE API base URL              |
| `.claude/scanner/`                  | Hourly read-only scanner (Task Scheduler)  |
| `.claude/worker/`                   | Per-minute Codex task worker (Task Scheduler) |

## Deploy

```powershell
# API
pwsh ./deploy/deploy-api.ps1            # publish + FTP upload + secret injection
pwsh ./deploy/deploy-api.ps1 -NoBuild   # re-upload existing publish output

# Frontend
pwsh ./deploy/deploy-frontend.ps1       # pnpm install + ng build + FTP upload
pwsh ./deploy/deploy-frontend.ps1 -NoBuild
```

Both scripts read FTP creds + runtime secrets from `.env` (gitignored).
`.env.example` lives at the repo root — copy and fill in.

Required `.env` keys:
- `API_FTP_HOST`, `API_FTP_USER`, `API_FTP_PASS`, `API_FTP_PATH`
- `FE_FTP_HOST`, `FE_FTP_USER`, `FE_FTP_PASS`, `FE_FTP_PATH`
- `ConnectionStrings__Default`, `ConnectionStrings__Hangfire`
- `Jwt__Secret` (64+ char random)
- `CORS_ORIGIN` (or `FE_URL` as fallback)
- `CLAUDE_WORKER_TOKEN` (for the `/Codex` operator system)
- `SMTP_HOST`, `SMTP_PORT`, `SMTP_SECURE`, `SMTP_USER`, `SMTP_PASS`, `SMTP_FROM_ADDRESS`

After deploy:
- `https://madaiapi.madprospects.com/health` -> 200 healthy
- `https://madai.madprospects.com` -> frontend loads
- `https://madaiapi.madprospects.com/hangfire` -> 401 (unauthenticated; expected). Hit it after logging in as a SystemAdmin to see the dashboard.

## `/Codex` task system (operator queue)

Separate from the end-user `TaskItem` queue - see the README's "/Codex task
system" section for full details. The scheduled tasks live under
`.claude/worker/` and `.claude/scanner/`; install / reinstall them via:

```powershell
pwsh ./scripts/register-claude-schedulers.ps1
```

## Local dev

```powershell
dotnet restore
pnpm install
./scripts/dev-up.ps1   # launches Api + Angular dev server
```

Seeded admin: `admin@madai.local` / `ChangeMe!2025`.
