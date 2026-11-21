# ==============================================================================
# LearnNN_WebBlazor - 1-Click Mobile Access Script (Microsoft Dev Tunnels)
# ==============================================================================

[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$ErrorActionPreference = "Stop"

# Refresh Environment PATH
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")
$windowsAppsPath = Join-Path $env:LOCALAPPDATA "Microsoft\WindowsApps"
if ($env:Path -notlike "*$windowsAppsPath*") {
    $env:Path = "$env:Path;$windowsAppsPath"
}

Clear-Host
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host "   [MOBILE] LearnNN WebBlazor - KHOI DONG TRUY CAP DIEN THOAI (1-CLICK)" -ForegroundColor Yellow
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""

# 1. Kiem tra devtunnel CLI
if (-not (Get-Command devtunnel -ErrorAction SilentlyContinue)) {
    Write-Host "[-] Chua tim thay 'devtunnel' CLI tren may." -ForegroundColor Yellow
    Write-Host "[*] Dang tu dong cai dat qua Winget..." -ForegroundColor Green
    winget install Microsoft.devtunnel --silent --accept-package-agreements --accept-source-agreements
    $env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")
}

# 2. Kiem tra trang thai dang nhap Dev Tunnels
Write-Host "[*] Dang kiem tra xac thuc Dev Tunnels..." -ForegroundColor Gray
$userShow = devtunnel user show 2>&1 | Out-String
if ($userShow -match "Not logged in") {
    Write-Host "[!] Ban can dang nhap tai khoan Microsoft hoac GitHub 1 LAN DUY NHAT." -ForegroundColor Yellow
    Write-Host "[*] Vui long dang nhap tren trinh duyet..." -ForegroundColor Cyan
    devtunnel user login
    $userShow = devtunnel user show 2>&1 | Out-String
}
Write-Host "[+] $userShow".Trim() -ForegroundColor Green

# 3. Quan ly Tunnel Co dinh (Persistent Tunnel)
$tunnelFile = Join-Path $PSScriptRoot ".devtunnel_id"
$tunnelId = ""

# Doc tu file neu da luu
if (Test-Path $tunnelFile) {
    $tunnelId = (Get-Content $tunnelFile -Raw).Trim()
}

# Neu chua co ID hoac can xac thuc lai, kiem tra danh sach tunnel san co cua tai khoan
$tunnelValid = $false
if (-not [string]::IsNullOrWhiteSpace($tunnelId)) {
    try {
        $showRaw = devtunnel show $tunnelId -j 2>$null
        if ($showRaw) {
            $showObj = $showRaw | ConvertFrom-Json
            if ($showObj.tunnel -and $showObj.tunnel.tunnelId) {
                $tunnelValid = $true
            }
        }
    } catch {
        $tunnelValid = $false
    }
}

if (-not $tunnelValid) {
    try {
        $listRaw = devtunnel list -j 2>$null
        if ($listRaw) {
            $listObj = $listRaw | ConvertFrom-Json
            if ($listObj.tunnels) {
                $found = $listObj.tunnels | Where-Object { $_.tunnelId -like "learnnn-vocab*" } | Select-Object -First 1
                if ($found) {
                    $tunnelId = $found.tunnelId
                    $tunnelValid = $true
                    Write-Host "[+] Da tim thay Tunnel san co: $tunnelId" -ForegroundColor Green
                }
            }
        }
    } catch { }
}

# Neu chua co thi tao moi
if (-not $tunnelValid) {
    Write-Host "[*] Dang khoi tao Tunnel co dinh cho LearnNN..." -ForegroundColor Cyan
    $createJson = $null
    try {
        $createJson = devtunnel create learnnn-vocab -j 2>$null | ConvertFrom-Json
    } catch { }

    if (-not $createJson -or -not $createJson.tunnel) {
        $createJson = devtunnel create -j | ConvertFrom-Json
    }

    if ($createJson.tunnel -and $createJson.tunnel.tunnelId) {
        $tunnelId = $createJson.tunnel.tunnelId
    } elseif ($createJson.tunnelId) {
        $tunnelId = $createJson.tunnelId
    }
}

if ([string]::IsNullOrWhiteSpace($tunnelId)) {
    Write-Host "[-] Khong tim thay Tunnel ID hop le. Dang dung mac dinh learnnn-vocab.asse" -ForegroundColor Yellow
    $tunnelId = "learnnn-vocab.asse"
}

# Luu ID co dinh
[System.IO.File]::WriteAllText($tunnelFile, $tunnelId, [System.Text.Encoding]::UTF8)

# 4. Dam bao Port 5186 da duoc mo tren Tunnel & Cap quyen Anonymous cho Blazor WebSockets
$showData = devtunnel show $tunnelId -j | ConvertFrom-Json
$hasPort5186 = $false
if ($showData.tunnel -and $showData.tunnel.ports) {
    $foundPort = $showData.tunnel.ports | Where-Object { $_.portNumber -eq 5186 }
    if ($foundPort) { $hasPort5186 = $true }
}

if (-not $hasPort5186) {
    Write-Host "[*] Dang cau hinh mo cong 5186 cho Tunnel..." -ForegroundColor Gray
    devtunnel port create $tunnelId -p 5186 --protocol http 2>&1 | Out-Null
    $showData = devtunnel show $tunnelId -j | ConvertFrom-Json
}

# Cap quyen Anonymous de Blazor Server WebSockets ket noi muot ma khong bi chan 401
Write-Host "[*] Cau hinh quyen truy cap (Anonymous cho Blazor WebSockets)..." -ForegroundColor Gray
try {
    devtunnel access create $tunnelId -a 2>&1 | Out-Null
} catch { }

# Lay URL cua cong 5186
$tunnelUrl = ""
if ($showData.tunnel -and $showData.tunnel.ports) {
    $foundPort = $showData.tunnel.ports | Where-Object { $_.portNumber -eq 5186 }
    if ($foundPort -and $foundPort.portUri) {
        $tunnelUrl = $foundPort.portUri
    }
}

if ([string]::IsNullOrWhiteSpace($tunnelUrl)) {
    $cleanId = $tunnelId.Split('.')[0]
    $tunnelUrl = "https://$cleanId-5186.asse.devtunnels.ms"
}

# 5. Khoi dong Web App Blazor (.NET 10) neu chua chay
$dotnetProc = $null
$tcpConn = Get-NetTCPConnection -LocalPort 5186 -State Listen -ErrorAction SilentlyContinue
if ($tcpConn) {
    Write-Host "[+] Ung dung LearnNN_WebBlazor da dang chay tren cong 5186." -ForegroundColor Green
} else {
    Write-Host "[*] Dang khoi chay ung dung LearnNN_WebBlazor (.NET 10)..." -ForegroundColor Cyan
    $projectPath = Join-Path $PSScriptRoot "Source\LearnNN_WebBlazor.csproj"
    $dotnetProc = Start-Process -FilePath "dotnet" -ArgumentList "run --project `"$projectPath`"" -PassThru -WindowStyle Hidden
    Start-Sleep -Seconds 4
}

# Lay dia chi IP noi bo LAN (neu cung mang Wi-Fi)
$localIp = (Get-NetIPAddress -AddressFamily IPv4 -ErrorAction SilentlyContinue | 
    Where-Object { $_.InterfaceAlias -notmatch "Loopback|vEthernet|WSL" -and $_.IPAddress -notlike "169.254*" } | 
    Select-Object -ExpandProperty IPAddress -First 1)

# 6. Hien thi thong tin ket noi va Ma QR
Write-Host ""
Write-Host "==================================================================" -ForegroundColor Green
Write-Host "   [READY] DUONG HAM CO DINH SAN SANG - TRUY CAP TU DIEN THOAI" -ForegroundColor Green
Write-Host "==================================================================" -ForegroundColor Green
Write-Host ""
Write-Host " [HTTPS] LINK TRUY CAP CO DINH (Khuyen nghi cho Dien thoai):" -ForegroundColor Yellow
Write-Host "    $tunnelUrl" -ForegroundColor Cyan
Write-Host "    (Link co dinh, ho tro HTTPS day du cho WebSockets, TTS am thanh & Camera)" -ForegroundColor DarkGray
if (-not [string]::IsNullOrWhiteSpace($localIp)) {
    Write-Host ""
    Write-Host " [LAN] HOAC LINK NOI BO (Khi dien thoai ket noi cung mang Wi-Fi):" -ForegroundColor Yellow
    Write-Host "    http://${localIp}:5186" -ForegroundColor Cyan
}
Write-Host ""
Write-Host " [OK] KET NOI TRUC TIEP: Mo link tren dien thoai la vao ngay (khong can dang nhap Microsoft)." -ForegroundColor Green
Write-Host ""
Write-Host " [QR CODE] QUET MA QR DUOI DAY BANG CAMERA DIEN THOAI DE MO:" -ForegroundColor Yellow
Write-Host ""

try {
    curl.exe -s "https://qrenco.de/$tunnelUrl"
} catch {
    Write-Host "    (Go truc tiep duong link tren vao Safari/Chrome dien thoai)" -ForegroundColor Gray
}

Write-Host ""
Write-Host " [TIP] Meo: Tren Safari (iOS) chon 'Chia se' -> 'Them vao MH chinh' de dung toan man hinh nhu App!" -ForegroundColor Cyan
Write-Host "------------------------------------------------------------------" -ForegroundColor DarkGray
Write-Host " Dang duy tri ket noi... Nhan Ctrl + C de dung." -ForegroundColor Gray
Write-Host ""

# 7. Giu Tunnel hoat dong (voi co -a de ho tro Anonymous WebSocket)
try {
    devtunnel host $tunnelId -a
} finally {
    if ($dotnetProc -and -not $dotnetProc.HasExited) {
        Write-Host "[*] Dang dung tien trinh .NET Blazor..." -ForegroundColor Gray
        Stop-Process -Id $dotnetProc.Id -Force -ErrorAction SilentlyContinue
    }
    Write-Host "[*] Da dong duong ham. Tam biet!" -ForegroundColor Yellow
}