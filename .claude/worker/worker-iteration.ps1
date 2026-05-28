# MADai autonomous Claude Code worker - Task Scheduler entry point.
# Fires every 1 minute. Implements adaptive backoff: actual API polls happen at
# 60s / 5min / 10min / 30min / 60min intervals depending on consecutive empty fetches.
# ASCII-only - PowerShell 5.1 mojibakes non-ASCII in UTF-8-no-BOM files.

[CmdletBinding()] param()
$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue'

$workerDir  = $PSScriptRoot
$repoRoot   = (Resolve-Path (Join-Path $workerDir '..\..')).Path
$statePath  = Join-Path $workerDir 'state.json'
$promptPath = Join-Path $workerDir 'worker-prompt.md'
$logPath    = Join-Path $workerDir 'worker.log'
$apiBase    = 'https://madaiapi.madprospects.com/api/v1'

# Load token from .env at repo root (CLAUDE_WORKER_TOKEN=...)
$envFile = Join-Path $repoRoot '.env'
$token = $null
if (Test-Path $envFile) {
  foreach ($line in Get-Content $envFile) {
    if ($line -match '^\s*CLAUDE_WORKER_TOKEN\s*=\s*(.*)$') { $token = $Matches[1].Trim() }
  }
}
if (-not $token) {
  Write-Error 'CLAUDE_WORKER_TOKEN missing from .env at repo root.'
  exit 1
}

$claudeBin = "$env:USERPROFILE\.local\bin\claude.exe"
if (-not (Test-Path $claudeBin)) {
  Write-Error "Claude binary not found at $claudeBin. Adjust the path or install Claude Code."
  exit 1
}

function Write-Log {
  param([string] $message)
  Add-Content -Path $logPath -Encoding UTF8 -Value ('{0:yyyy-MM-ddTHH:mm:ssZ}  {1}' -f (Get-Date).ToUniversalTime(), $message)
}

function Read-State {
  if (-not (Test-Path $statePath)) { return @{ streak = 0; lastFiredAt = [datetime]::MinValue } }
  try {
    $raw = Get-Content $statePath -Raw | ConvertFrom-Json
    $last = if ($raw.lastFiredAt) { [datetime]$raw.lastFiredAt } else { [datetime]::MinValue }
    return @{ streak = [int]$raw.streak; lastFiredAt = $last }
  } catch {
    return @{ streak = 0; lastFiredAt = [datetime]::MinValue }
  }
}

function Write-State {
  param([int] $streak, [datetime] $ts)
  [pscustomobject]@{
    streak      = $streak
    lastFiredAt = $ts.ToUniversalTime().ToString('o')
  } | ConvertTo-Json | Out-File $statePath -Encoding utf8 -Force
}

function Get-BucketSeconds {
  param([int] $streak)
  if ($streak -le 0)  { return 60 }
  if ($streak -le 4)  { return 300 }
  if ($streak -le 9)  { return 600 }
  if ($streak -le 14) { return 1800 }
  return 3600
}

# --- main flow -------------------------------------------------------------
$state = Read-State
$now = Get-Date
$bucket = Get-BucketSeconds $state.streak
$elapsed = if ($state.lastFiredAt -eq [datetime]::MinValue) { [int]::MaxValue } else { ($now - $state.lastFiredAt).TotalSeconds }

if ($elapsed -lt $bucket) {
  Write-Log ("SKIP  streak={0} bucket={1}s elapsed={2:N0}s - not yet" -f $state.streak, $bucket, $elapsed)
  exit 0
}

Write-Log ("FIRE  streak={0} bucket={1}s elapsed={2:N0}s - checking queue" -f $state.streak, $bucket, $elapsed)

try {
  $resp = Invoke-WebRequest -Uri "$apiBase/claude-tasks/next" -Headers @{ 'X-API-Key' = $token } -Method GET -UseBasicParsing
  $code = [int]$resp.StatusCode
} catch {
  $code = if ($_.Exception.Response) { [int]$_.Exception.Response.StatusCode } else { 0 }
  Write-Log "WARN  /next failed: status=$code"
  Write-State -streak ($state.streak + 1) -ts $now
  exit 0
}

if ($code -eq 204) {
  $newStreak = $state.streak + 1
  Write-Log ("EMPTY queue (204). streak {0} -> {1}. Next bucket: {2}s." -f $state.streak, $newStreak, (Get-BucketSeconds $newStreak))
  Write-State -streak $newStreak -ts $now
  exit 0
}

Write-Log "WORK  queue non-empty. Invoking Claude Code worker session..."
Write-State -streak 0 -ts $now

$prompt = Get-Content $promptPath -Raw
Push-Location $repoRoot
try {
  $start = Get-Date
  $prompt | & $claudeBin --print --dangerously-skip-permissions --add-dir $repoRoot 2>&1 |
    ForEach-Object { Add-Content -Path $logPath -Encoding UTF8 -Value "  $_" }
  $elapsedRun = ((Get-Date) - $start).TotalSeconds
  Write-Log ("DONE  claude exit={0} elapsed={1:N0}s" -f $LASTEXITCODE, $elapsedRun)
} finally {
  Pop-Location
}
