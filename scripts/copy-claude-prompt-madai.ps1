<#
.SYNOPSIS
  Copy the MADai-stack /claude task-system bootstrap prompt body to the clipboard.
.DESCRIPTION
  Reads docs/claude-task-system-prompt-madai.md, strips everything outside the
  ===== PROMPT BEGINS ===== / ===== PROMPT ENDS ===== markers, and puts the
  body on the Windows clipboard ready to paste into a fresh Claude Code session.

  This is the .NET 8 + EF Core + MSSQL + Angular 19 + SignalR variant.
  For the NestJS / Prisma / MySQL / SSE variant, use copy-claude-prompt.ps1.
.EXAMPLE
  C:\Code\madprospects\madai\scripts\copy-claude-prompt-madai.ps1
#>
param()

$ErrorActionPreference = 'Stop'

$promptFile = Join-Path $PSScriptRoot '..\docs\claude-task-system-prompt-madai.md'
$promptFile = (Resolve-Path $promptFile).Path

if (-not (Test-Path $promptFile)) {
  throw "Prompt file not found at $promptFile"
}

$md = Get-Content $promptFile -Raw -Encoding UTF8

$body = $md -replace '(?s).*?===== PROMPT BEGINS =====\s*', '' `
            -replace '\s*===== PROMPT ENDS =====.*', ''

if ([string]::IsNullOrWhiteSpace($body)) {
  throw "Could not extract body between ===== PROMPT BEGINS ===== / ===== PROMPT ENDS ===== markers in $promptFile"
}

$body | Set-Clipboard

$chars = $body.Length
$lines = ($body -split "`n").Count
Write-Host ("Copied MADai-stack /claude bootstrap prompt to clipboard: {0:N0} chars, {1:N0} lines" -f $chars, $lines) -ForegroundColor Green
Write-Host "Paste into a Claude Code session at C:\Code\madprospects\madai. The prompt's Phase 0 will validate the existing project layout before any code is written." -ForegroundColor DarkGray
