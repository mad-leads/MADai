<#
.SYNOPSIS
  Copy the /claude task-system bootstrap prompt body to the clipboard.
.DESCRIPTION
  Reads docs/claude-task-system-prompt.md, strips everything outside the
  ===== PROMPT BEGINS ===== / ===== PROMPT ENDS ===== markers, and puts the
  body on the Windows clipboard so you can paste it as the first message
  in a fresh Claude Code session.

  The doc holds MADai-specific values. When pasting into a different app,
  follow the "Genericising back to a template" find-replace list at the
  bottom of the doc file before sending.
.EXAMPLE
  C:\Code\madprospects\madai\scripts\copy-claude-prompt.ps1
  # from a sibling repo, e.g. C:\Code\madhub:
  ..\madai\scripts\copy-claude-prompt.ps1
#>
param()

$ErrorActionPreference = 'Stop'

$promptFile = Join-Path $PSScriptRoot '..\docs\claude-task-system-prompt.md'
$promptFile = (Resolve-Path $promptFile).Path

if (-not (Test-Path $promptFile)) {
  throw "Prompt file not found at $promptFile"
}

# UTF-8 read so the icon characters in the doc survive
$md = Get-Content $promptFile -Raw -Encoding UTF8

$body = $md -replace '(?s).*?===== PROMPT BEGINS =====\s*', '' `
            -replace '\s*===== PROMPT ENDS =====.*', ''

if ([string]::IsNullOrWhiteSpace($body)) {
  throw "Could not extract body between ===== PROMPT BEGINS ===== / ===== PROMPT ENDS ===== markers in $promptFile"
}

$body | Set-Clipboard

$chars = $body.Length
$lines = ($body -split "`n").Count
Write-Host ("Copied /claude bootstrap prompt to clipboard: {0:N0} chars, {1:N0} lines" -f $chars, $lines) -ForegroundColor Green
Write-Host "If pasting into a non-MADai project, find-replace the MADai-specific values first (see end of docs/claude-task-system-prompt.md)." -ForegroundColor DarkGray
