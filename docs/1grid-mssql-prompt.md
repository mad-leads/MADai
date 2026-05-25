# 1-grid MSSQL connection prompt (paste into Claude Code)

A self-contained brief for any project that needs to talk to the shared
1-grid SQL Server. Copy from the `--- PASTE START ---` line down.

--- PASTE START ---

I'm working on a .NET project that needs to use a SQL Server database hosted on **1-grid** (Plesk Windows shared hosting). Set up the database integration following these conventions exactly - they're the result of hard-won debugging on a sibling project (MADai) and avoid every pitfall I hit there.

## Server details

- **Host**: `WINSVRSQL03.hostserv.co.za`
- **Port**: `1433`
- **Auth mode**: SQL Server authentication (not Windows auth)
- **Certificate**: self-signed (must be trusted, not validated)
- **External access**: **BLOCKED**. The MSSQL host is only reachable from inside 1-grid's hosting network. Do **not** attempt to connect from my dev laptop or any other network - every port is firewalled. Verifying connectivity from outside is a waste of time; the connection will only succeed from the deployed API process running on 1-grid.

## Per-project values I'll provide (or placeholders for now)

- `<<DB_NAME>>` - the database name provisioned in the 1-grid panel (e.g. `madhub`, `madleads`, `madauthor`)
- `<<DB_USER>>` - the SQL login (often `madproducts`, sometimes per-project)
- `<<DB_PASSWORD>>` - must come from `.env`, never committed

## Connection string template

```
Server=tcp:WINSVRSQL03.hostserv.co.za,1433;Database=<<DB_NAME>>;User Id=<<DB_USER>>;Password=<<DB_PASSWORD>>;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Connect Timeout=30;
```

Each flag is mandatory:
- `tcp:` prefix + explicit `,1433` - avoids Plesk DNS quirks
- `Encrypt=True` + `TrustServerCertificate=True` - `Microsoft.Data.SqlClient` 4+ defaults to encrypted connections; the cert is self-signed so we trust it without validation
- `MultipleActiveResultSets=True` - EF Core needs it for certain query shapes
- `Connect Timeout=30` - Plesk's app pool can take a few seconds to wake

## Local development workflow

Because the server isn't reachable from dev boxes, do **not** point `appsettings.Development.json` at 1-grid. Use LocalDB:

```
Server=(localdb)\MSSQLLocalDB;Database=<<DB_NAME>>;Trusted_Connection=True;TrustServerCertificate=True;
```

Put it in `appsettings.Local.json` (already-gitignored convention for ASP.NET Core) and have `Program.cs` load that file as an optional layer:

```csharp
builder.Configuration
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
```

## Config layering (highest → lowest precedence)

1. Environment variables (production injection)
2. `appsettings.Local.json` - gitignored, holds dev creds
3. `appsettings.{Environment}.json`
4. `appsettings.json` - **placeholder values only**, never commit real secrets

## .env file structure

```
ConnectionStrings__Default=Server=tcp:WINSVRSQL03.hostserv.co.za,1433;Database=<<DB_NAME>>;User Id=<<DB_USER>>;Password=<<DB_PASSWORD>>;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;
Jwt__Secret=<<64+ random chars - generate one>>

API_FTP_HOST=41.185.110.61
API_FTP_USER=<<FTP_USER>>
API_FTP_PASS=<<FTP_PASSWORD>>
API_FTP_PATH=/<<api-subdomain>>.madproducts.com

FE_FTP_HOST=41.185.110.61
FE_FTP_USER=<<FTP_USER>>
FE_FTP_PASS=<<FTP_PASSWORD>>
FE_FTP_PATH=/<<frontend-subdomain>>.madproducts.com
```

`.env` must be in `.gitignore`.

## Plesk deploy pattern - THE critical gotcha

**Plesk's "Application Settings" UI writes to `<appSettings>` in web.config, which is a .NET Framework concept that ASP.NET Core does not read.** Setting connection strings or `Jwt__Secret` via that UI is silently useless. The app will start, every request will throw `IDX10703: key length is zero` or `ConnectionString property has not been initialized`, and the panel will show no error.

The canonical place for ASP.NET Core env vars on IIS is `<environmentVariables>` inside `<aspNetCore>` in `web.config`. Use deploy-time substitution so secrets stay in `.env`:

**Source `src/<Project>.Api/web.config`** - placeholders, safe to commit:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet"
                  arguments=".\<Project>.Api.dll"
                  stdoutLogEnabled="false"
                  stdoutLogFile=".\logs\stdout"
                  hostingModel="inprocess">
        <environmentVariables>
          <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
          <environmentVariable name="ConnectionStrings__Default" value="__CONNECTIONSTRINGS_DEFAULT__" />
          <environmentVariable name="Jwt__Secret" value="__JWT_SECRET__" />
        </environmentVariables>
      </aspNetCore>
    </system.webServer>
  </location>
</configuration>
```

**csproj** - stop the SDK from overwriting it:
```xml
<PropertyGroup>
  <IsTransformWebConfigDisabled>true</IsTransformWebConfigDisabled>
</PropertyGroup>
```

**Deploy script** (`scripts/deploy-api.ps1`) - after `dotnet publish`, before FTP upload, do a string replace on the *published* `web.config` substituting `__CONNECTIONSTRINGS_DEFAULT__` and `__JWT_SECRET__` with values from `.env`. XML-escape the values (`&` → `&amp;` etc.) so passwords containing special chars don't break the XML. Then push to FTP. The substituted secrets exist only in the deployed `web.config` on the server.

The same `.env` also holds the FTP credentials, so the deploy script uses one file for everything. The working reference implementation is at `C:\Code\madai\scripts\deploy-api.ps1` - copy it, swap the project paths.

## Known caveats - don't burn time on these

1. **Cannot connect from dev laptop.** The SQL host is firewalled to 1-grid's hosting subnet. Don't troubleshoot connectivity locally; only the deployed API can connect.
2. **Plesk's env-var panel is unreliable.** Use the `web.config` `<environmentVariables>` approach above.
3. **App-pool cold starts** add ~4–10 s to first request after idle. Mitigate via Plesk "Idle Timeout = 0" or an external uptime ping every 60 s.
4. **`Microsoft.Data.SqlClient` (not `System.Data.SqlClient`).** Recent EF Core uses the former; mixing them causes runtime errors.
5. **Windows PowerShell 5.1 reads `.ps1` files as ANSI** - no non-ASCII characters in deploy scripts unless you save with a UTF-8 BOM. Em-dashes and curly quotes will silently corrupt the parser.
6. **AutoMapper `ProjectTo<T>` doesn't work with positional record DTOs** (they have no parameterless constructor). Use manual `.Select(t => new TDto(...))` for the queries that need server-side projection, or change the DTOs to non-positional records.

## What I want you to do

When I describe my project:

1. Use `Microsoft.EntityFrameworkCore.SqlServer` + `Microsoft.Data.SqlClient`.
2. Set up `DbContext` to read `ConnectionStrings:Default` from configuration via `builder.Configuration.GetConnectionString("Default")`.
3. Layer config as listed above; ensure `appsettings.Local.json` is in `.gitignore`.
4. Wire `Program.cs` to load `.env` if `DotNetEnv` is already a dependency, otherwise rely on the ASP.NET Core env-var provider.
5. If this project is Plesk-deployed, scaffold the deploy-time `web.config` substitution pattern.
6. Never put real credentials in `appsettings.json` or `appsettings.Production.json` - those files hold blank placeholders only.
7. Add a `Connect Timeout=30` to all connection strings.

Don't simulate connecting to the server during development - the connection only works from the deployed environment. If you need to verify the schema, do it by reading the EF migrations or by asking me to run a query from the deployed app.

--- PASTE END ---
