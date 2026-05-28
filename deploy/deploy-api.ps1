param(
    [switch]$NoBuild
)

$ErrorActionPreference = 'Stop'
$env:DOTNET_CLI_WORKLOAD_UPDATE_NOTIFY_DISABLE = 'true'
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

function Get-OptionalKey($map, [string]$key, [string]$defaultValue = '') {
    $envValue = [Environment]::GetEnvironmentVariable($key)
    if (-not [string]::IsNullOrWhiteSpace($envValue)) {
        return $envValue
    }

    if ($map.ContainsKey($key) -and -not [string]::IsNullOrWhiteSpace($map[$key])) {
        return $map[$key]
    }

    return $defaultValue
}

function Normalize-RemotePath([string]$path) {
    $trimmed = $path.Trim()
    if (-not $trimmed.StartsWith('/')) {
        $trimmed = "/$trimmed"
    }
    return $trimmed.TrimEnd('/')
}

function Get-WinScpExe {
    $winScp = Get-Command WinSCP.com,winscp.com.exe,WinSCP.com.exe -ErrorAction SilentlyContinue | Select-Object -First 1
    if (-not $winScp) {
        throw 'WinSCP.com is required for SFTP deployment but was not found.'
    }
    return $winScp.Source
}

function Publish-RemoteDirectory([string]$source, [string]$remotePath, [string]$hostName, [string]$user, [string]$pass, [string]$port, [string]$transferProtocol = 'ftp') {
    $remote = Normalize-RemotePath $remotePath
    $encodedUser = [System.Uri]::EscapeDataString($user)
    $encodedPass = [System.Uri]::EscapeDataString($pass)
    $protocol = if ($transferProtocol -eq 'sftp') { 'sftp' } else { 'ftp' }
    $hostKey = if ($protocol -eq 'sftp') { ' -hostkey=*' } else { '' }
    $sessionUrl = "${protocol}://$encodedUser`:$encodedPass@$hostName`:$port/"
    $scriptPath = Join-Path ([System.IO.Path]::GetTempPath()) ("madai-winscp-{0}.txt" -f ([Guid]::NewGuid().ToString('N')))
    $commands = @(
        'option batch abort',
        'option confirm off',
        "open `"$sessionUrl`"$hostKey",
        'option transfer binary',
        "synchronize remote -delete -criteria=either `"$source`" `"$remote`"",
        'exit'
    )
    Set-Content -LiteralPath $scriptPath -Value $commands -Encoding ASCII
    try {
        & (Get-WinScpExe) /ini=nul /script=$scriptPath
        if ($LASTEXITCODE -ne 0) {
            throw "WinSCP failed with exit code $LASTEXITCODE"
        }
    }
    finally {
        Remove-Item -LiteralPath $scriptPath -Force -ErrorAction SilentlyContinue
    }
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
$sftpHost = Require-Key $envValues 'SFTP_HOST'
$sftpUser = Require-Key $envValues 'SFTP_USER'
$sftpPass = Require-Key $envValues 'SFTP_PASS'
$transferProtocol = Get-OptionalKey $envValues 'TRANSFER_PROTOCOL' 'ftp'
$defaultPort = if ($transferProtocol -eq 'sftp') { '22' } else { '21' }
$sftpPort = Get-OptionalKey $envValues 'SFTP_PORT' $defaultPort
$remotePath = Get-OptionalKey $envValues 'API_WEB_ROOT' (Get-OptionalKey $envValues 'API_REMOTE_PATH' (Require-Key $envValues 'API_FTP_PATH'))

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

Publish-RemoteDirectory $publishDir $remotePath $sftpHost $sftpUser $sftpPass $sftpPort $transferProtocol
Write-Host "API deploy complete: $remotePath"
