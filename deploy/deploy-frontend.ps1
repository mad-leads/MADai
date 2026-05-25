param(
    [switch]$NoBuild
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$frontend = Join-Path $root 'apps\web'
$envPath = Join-Path $root '.env'
$dist = Join-Path $frontend 'dist\madai\browser'
if (-not (Test-Path -LiteralPath $dist)) {
    $dist = Join-Path $frontend 'dist\madai'
}

function Read-DotEnv([string]$path) {
    $map = @{}
    if (Test-Path -LiteralPath $path) {
        foreach ($line in Get-Content -LiteralPath $path) {
            $trimmed = $line.Trim()
            if (-not $trimmed -or $trimmed.StartsWith('#')) { continue }
            $idx = $trimmed.IndexOf('=')
            if ($idx -lt 1) { continue }
            $map[$trimmed.Substring(0, $idx).Trim()] = $trimmed.Substring($idx + 1).Trim()
        }
    }
    return $map
}

function Require-Key($map, [string]$key) {
    $envValue = [Environment]::GetEnvironmentVariable($key)
    if (-not [string]::IsNullOrWhiteSpace($envValue)) {
        return $envValue
    }

    if (-not $map.ContainsKey($key) -or [string]::IsNullOrWhiteSpace($map[$key])) {
        throw "Missing required .env key: $key"
    }
    return $map[$key]
}

function ConvertTo-FtpUri([string]$hostName, [string]$remotePath, [string]$relativePath) {
    $basePath = '/' + $remotePath.Trim('/')
    $rel = ($relativePath -replace '\\','/').TrimStart('/')
    return "ftp://$hostName$basePath/$rel"
}

function Ensure-FtpDirectory([string]$hostName, [string]$remotePath, [string]$relativePath, [string]$user, [string]$pass) {
    $directory = [System.IO.Path]::GetDirectoryName($relativePath)
    if ([string]::IsNullOrWhiteSpace($directory)) { return }

    $current = ''
    foreach ($segment in (($directory -replace '\\','/').Split('/') | Where-Object { $_ })) {
        $current = if ($current) { "$current/$segment" } else { $segment }
        $uri = ConvertTo-FtpUri $hostName $remotePath $current
        $request = [System.Net.FtpWebRequest]::Create($uri)
        $request.Method = [System.Net.WebRequestMethods+Ftp]::MakeDirectory
        $request.Credentials = [System.Net.NetworkCredential]::new($user, $pass)
        try {
            $response = $request.GetResponse()
            $response.Close()
        }
        catch [System.Net.WebException] {
            if ($_.Exception.Response) {
                $_.Exception.Response.Close()
            }
        }
    }
}

function Get-RelativePath([string]$basePath, [string]$fullPath) {
    $base = (Resolve-Path -LiteralPath $basePath).Path.TrimEnd('\') + '\'
    $full = (Resolve-Path -LiteralPath $fullPath).Path
    return $full.Substring($base.Length)
}

function Upload-Directory([string]$source, [string]$hostName, [string]$remotePath, [string]$user, [string]$pass) {
    $client = [System.Net.WebClient]::new()
    $client.Credentials = [System.Net.NetworkCredential]::new($user, $pass)
    try {
        Get-ChildItem -LiteralPath $source -Recurse -File | ForEach-Object {
            $relative = Get-RelativePath $source $_.FullName
            Ensure-FtpDirectory $hostName $remotePath $relative $user $pass
            $uri = ConvertTo-FtpUri $hostName $remotePath $relative
            Write-Host "Uploading $relative"
            $client.UploadFile($uri, $_.FullName) | Out-Null
        }
    }
    finally {
        $client.Dispose()
    }
}

$envValues = Read-DotEnv $envPath
$ftpHost = Require-Key $envValues 'FE_FTP_HOST'
$ftpUser = Require-Key $envValues 'FE_FTP_USER'
$ftpPass = Require-Key $envValues 'FE_FTP_PASS'
$ftpPath = Require-Key $envValues 'FE_FTP_PATH'

if (-not $NoBuild) {
    Push-Location $root
    try {
        pnpm install
        pnpm web:build
    }
    finally {
        Pop-Location
    }
}

if (-not (Test-Path -LiteralPath $dist)) {
    throw "Frontend dist folder not found at $dist"
}

Upload-Directory $dist $ftpHost $ftpPath $ftpUser $ftpPass
Write-Host "Frontend deploy complete: $ftpPath"
