param(
    [switch]$NoBuild
)

$ErrorActionPreference = 'Stop'
$env:PNPM_HOME = 'C:\Code\.pnpm'
$env:PNPM_STORE_DIR = 'C:\Code\.pnpm'
$env:NPM_CONFIG_STORE_DIR = 'C:\Code\.pnpm'
$env:COREPACK_ENABLE_DOWNLOAD_PROMPT = '0'
$env:NODE_OPTIONS = '--max-old-space-size=4096'
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

function Invoke-Pnpm {
    param([string[]]$Arguments)

    $corepack = Get-Command corepack.cmd,corepack -ErrorAction SilentlyContinue | Select-Object -First 1
    if (-not $corepack) {
        throw 'corepack is required to run pnpm for frontend deployment but was not found.'
    }

    & $corepack.Source pnpm @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "pnpm failed with exit code $LASTEXITCODE"
    }
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
$remotePath = Get-OptionalKey $envValues 'FRONTEND_REMOTE_PATH' (Require-Key $envValues 'FE_FTP_PATH')

if (-not $NoBuild) {
    Push-Location $root
    try {
        Invoke-Pnpm @('install', '--store-dir', 'C:\Code\.pnpm', '--prefer-offline')
        Invoke-Pnpm @('--dir', 'apps/web', 'exec', 'ng', 'build')
    }
    finally {
        Pop-Location
    }
}

if (-not (Test-Path -LiteralPath $dist)) {
    throw "Frontend dist folder not found at $dist"
}

Publish-RemoteDirectory $dist $remotePath $sftpHost $sftpUser $sftpPass $sftpPort $transferProtocol
Write-Host "Frontend deploy complete: $remotePath"
