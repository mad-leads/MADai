<#
.SYNOPSIS
  Copy the 1-grid MSSQL prompt body (between the PASTE markers) to the clipboard.
.DESCRIPTION
  Reads docs/1grid-mssql-prompt.md, strips the header and PASTE markers, and
  puts the body on the Windows clipboard so you can paste it as the first
  message in a Claude Code session for any other product.
.EXAMPLE
  C:\Code\madprospects\madai\scripts\copy-mssql-prompt.ps1
  # from a sibling repo, e.g. C:\Code\madhub:
  ..\madai\scripts\copy-mssql-prompt.ps1
#>
param()

$ErrorActionPreference = 'Stop'

$promptFile = Join-Path $PSScriptRoot '..\docs\1grid-mssql-prompt.md'
$promptFile = (Resolve-Path $promptFile).Path

if (-not (Test-Path $promptFile)) {
  throw "Prompt file not found at $promptFile"
}

$md = Get-Content $promptFile -Raw

# Trim the header above PASTE START and anything after PASTE END
$body = $md -replace '(?s).*?--- PASTE START ---\s*', '' `
            -replace '\s*--- PASTE END ---.*', ''

if ([string]::IsNullOrWhiteSpace($body)) {
  throw "Could not extract body between PASTE START / PASTE END markers in $promptFile"
}

$body | Set-Clipboard

$chars = $body.Length
$lines = ($body -split "`n").Count
Write-Host ("Copied prompt body to clipboard: {0:N0} chars, {1:N0} lines" -f $chars, $lines) -ForegroundColor Green
Write-Host "Next step: paste it as the first message in your project's Claude Code session, then append a 'Project specifics' block." -ForegroundColor DarkGray
