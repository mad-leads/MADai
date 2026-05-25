param(
    [switch]$NoBuild
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$envPath = Join-Path $root '.env'
$publishDir = Join-Path $root '.deploy\api'
$project = Join-Path $root 'apps\api\src\MADai.Api\MADai.Api.csproj'

function Read-DotEnv([string]$path) {
    $map = @{}
    if (Test-Path -LiteralPath $path) {
        foreach ($line in Get-Content -LiteralPath $path) {
            $trimmed = $line.Trim()
            if (-not $trimmed -or $trimmed.StartsWith('#')) { continue }
            $idx = $trimmed.IndexOf('=')
            if ($idx -lt 1) { continue }
            $key = $trimmed.Substring(0, $idx).Trim()
            $value = $trimmed.Substring($idx + 1).Trim()
            $map[$key] = $value
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
$ftpHost = Require-Key $envValues 'API_FTP_HOST'
$ftpUser = Require-Key $envValues 'API_FTP_USER'
$ftpPass = Require-Key $envValues 'API_FTP_PASS'
$ftpPath = Require-Key $envValues 'API_FTP_PATH'

if (-not $NoBuild) {
    dotnet publish $project -c Release -o $publishDir
}

$webConfig = Join-Path $publishDir 'web.config'
if (-not (Test-Path -LiteralPath $webConfig)) {
    throw "Published web.config not found at $webConfig"
}

$corsOrigin = if ($envValues.ContainsKey('CORS_ORIGIN') -and $envValues['CORS_ORIGIN']) { $envValues['CORS_ORIGIN'] } else { Require-Key $envValues 'FE_URL' }
$replacements = @{
    '__ConnectionStrings__Default__' = Require-Key $envValues 'ConnectionStrings__Default'
    '__ConnectionStrings__Hangfire__' = Require-Key $envValues 'ConnectionStrings__Hangfire'
    '__Jwt__Secret__' = Require-Key $envValues 'Jwt__Secret'
    '__CORS_ORIGIN__' = $corsOrigin
    '__CLAUDE_WORKER_TOKEN__' = Require-Key $envValues 'CLAUDE_WORKER_TOKEN'
    '__SMTP_HOST__' = Require-Key $envValues 'SMTP_HOST'
    '__SMTP_PORT__' = Require-Key $envValues 'SMTP_PORT'
    '__SMTP_SECURE__' = Require-Key $envValues 'SMTP_SECURE'
    '__SMTP_USER__' = Require-Key $envValues 'SMTP_USER'
    '__SMTP_PASS__' = Require-Key $envValues 'SMTP_PASS'
    '__SMTP_FROM_ADDRESS__' = Require-Key $envValues 'SMTP_FROM_ADDRESS'
}

$content = Get-Content -LiteralPath $webConfig -Raw
foreach ($key in $replacements.Keys) {
    $content = $content.Replace($key, [System.Security.SecurityElement]::Escape($replacements[$key]))
}
Set-Content -LiteralPath $webConfig -Value $content -Encoding UTF8

Upload-Directory $publishDir $ftpHost $ftpPath $ftpUser $ftpPass
Write-Host "API deploy complete: $ftpPath"
