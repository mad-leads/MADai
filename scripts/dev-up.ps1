$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot

Start-Process powershell -ArgumentList '-NoExit', '-Command', "cd '$root'; dotnet run --project apps/api/src/MADai.Api --urls http://localhost:3011" -WindowStyle Normal
Start-Process powershell -ArgumentList '-NoExit', '-Command', "cd '$root'; pnpm web:start" -WindowStyle Normal
