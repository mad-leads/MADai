# Registers (or re-registers) the two Task Scheduler entries that drive the /claude system.
# Run once with admin rights or as the interactive user that will own the schedule entries.
# Re-run anytime - "-Force" replaces existing entries.
#
# To disable temporarily:
#   Disable-ScheduledTask -TaskName 'MADaiClaude*'
# To re-enable:
#   Enable-ScheduledTask -TaskName 'MADaiClaude*'
# To remove:
#   Unregister-ScheduledTask -TaskName 'MADaiClaudeWorker' -Confirm:$false
#   Unregister-ScheduledTask -TaskName 'MADaiClaudeScanner' -Confirm:$false

[CmdletBinding()] param()
$ErrorActionPreference = 'Stop'

$repoRoot   = Resolve-Path (Join-Path $PSScriptRoot '..')
$workerScript  = Join-Path $repoRoot '.claude\worker\worker-iteration.ps1'
$scannerScript = Join-Path $repoRoot '.claude\scanner\scanner-iteration.ps1'

if (-not (Test-Path $workerScript))  { throw "Missing $workerScript" }
if (-not (Test-Path $scannerScript)) { throw "Missing $scannerScript" }

$baseSettings = New-ScheduledTaskSettingsSet `
  -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries -StartWhenAvailable `
  -ExecutionTimeLimit (New-TimeSpan -Hours 1) -MultipleInstances IgnoreNew

$principal = New-ScheduledTaskPrincipal -UserId $env:USERNAME -LogonType Interactive -RunLevel Limited

# Worker - every 1 minute
$workerAction = New-ScheduledTaskAction -Execute 'powershell.exe' `
  -Argument ('-NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -File "{0}"' -f $workerScript) `
  -WorkingDirectory $repoRoot
$workerTrigger = New-ScheduledTaskTrigger -Once -At (Get-Date).Date.AddMinutes(15) `
  -RepetitionInterval (New-TimeSpan -Minutes 1) -RepetitionDuration ([TimeSpan]::FromDays(3650))
Register-ScheduledTask -TaskName 'MADaiClaudeWorker' `
  -Description 'MADai autonomous Claude Code worker - drains /api/v1/claude-tasks every minute (adaptive backoff).' `
  -Action $workerAction -Trigger $workerTrigger -Settings $baseSettings -Principal $principal -Force | Out-Null

Write-Host 'Registered MADaiClaudeWorker (every 1 minute, adaptive backoff)' -ForegroundColor Green

# Scanner - every 1 hour
$scannerAction = New-ScheduledTaskAction -Execute 'powershell.exe' `
  -Argument ('-NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -File "{0}"' -f $scannerScript) `
  -WorkingDirectory $repoRoot
$scannerTrigger = New-ScheduledTaskTrigger -Once -At (Get-Date).Date.AddMinutes(15) `
  -RepetitionInterval (New-TimeSpan -Hours 1) -RepetitionDuration ([TimeSpan]::FromDays(3650))
Register-ScheduledTask -TaskName 'MADaiClaudeScanner' `
  -Description 'MADai hourly codebase scanner - posts findings to /api/v1/claude-tasks/import-bulk.' `
  -Action $scannerAction -Trigger $scannerTrigger -Settings $baseSettings -Principal $principal -Force | Out-Null

Write-Host 'Registered MADaiClaudeScanner (every 1 hour)' -ForegroundColor Green
Write-Host ''
Write-Host 'Both schedulers run as the interactive user. They will only fire when this user is logged in (or auto-login is enabled).' -ForegroundColor DarkGray
Write-Host 'Trigger a manual run with: Start-ScheduledTask -TaskName MADaiClaudeWorker' -ForegroundColor DarkGray
