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
Write-Host "   📱 LearnNN WebBlazor - KHỞI ĐỘNG TRUY CẬP ĐIỆN THOẠI (1-CLICK)" -ForegroundColor Yellow
Write-Host "==================================================================" -ForegroundColor Cyan
Write-Host ""

# 1. Kiểm tra devtunnel CLI
if (-not (Get-Command devtunnel -ErrorAction SilentlyContinue)) {
    Write-Host "[-] Chưa tìm thấy 'devtunnel' CLI trên máy." -ForegroundColor Yellow
    Write-Host "[*] Đang tự động cài đặt qua Winget..." -ForegroundColor Green
    winget install Microsoft.devtunnel --silent --accept-package-agreements --accept-source-agreements
    $env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")
}

# 2. Kiểm tra trạng thái đăng nhập Dev Tunnels
Write-Host "[*] Đang kiểm tra xác thực Dev Tunnels..." -ForegroundColor Gray
$userShow = devtunnel user show 2>&1 | Out-String
if ($userShow -match "Not logged in") {
    Write-Host "[!] Bạn cần đăng nhập tài khoản Microsoft hoặc GitHub 1 LẦN DUY NHẤT để tạo đường hầm riêng tư." -ForegroundColor Yellow
    Write-Host "[*] Vui lòng đăng nhập trên trình duyệt..." -ForegroundColor Cyan
    devtunnel user login
    $userShow = devtunnel user show 2>&1 | Out-String
}
Write-Host "[+] $userShow".Trim() -ForegroundColor Green

# 3. Quản lý Tunnel Cố định (Persistent Tunnel)
$tunnelFile = Join-Path $PSScriptRoot ".devtunnel_id"
$tunnelId = ""

# Đọc từ file nếu đã lưu
if (Test-Path $tunnelFile) {
    $tunnelId = (Get-Content $tunnelFile -Raw).Trim()
}

# Nếu chưa có ID hoặc cần xác thực lại, kiểm tra danh sách tunnel sẵn có của tài khoản
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
    # Kiểm tra xem tài khoản đã có sẵn tunnel learnnn-vocab chưa
    try {
        $listRaw = devtunnel list -j 2>$null
        if ($listRaw) {
            $listObj = $listRaw | ConvertFrom-Json
            if ($listObj.tunnels) {
                $found = $listObj.tunnels | Where-Object { $_.tunnelId -like "learnnn-vocab*" } | Select-Object -First 1
                if ($found) {
                    $tunnelId = $found.tunnelId
                    $tunnelValid = $true
                    Write-Host "[+] Đã tìm thấy Tunnel sẵn có: $tunnelId" -ForegroundColor Green
                }
            }
        }
    } catch { }
}

# Nếu chưa có thì tạo mới
if (-not $tunnelValid) {
    Write-Host "[*] Đang khởi tạo Tunnel cố định cho LearnNN..." -ForegroundColor Cyan
    $createJson = $null
    try {
        $createJson = devtunnel create learnnn-vocab -j 2>$null | ConvertFrom-Json
    } catch { }

    if (-not $createJson -or -not $createJson.tunnel) {
        # Fallback tạo tunnel với ID ngẫu nhiên nhưng cố định
        $createJson = devtunnel create -j | ConvertFrom-Json
    }

    if ($createJson.tunnel -and $createJson.tunnel.tunnelId) {
        $tunnelId = $createJson.tunnel.tunnelId
    } elseif ($createJson.tunnelId) {
        $tunnelId = $createJson.tunnelId
    }
}

if ([string]::IsNullOrWhiteSpace($tunnelId)) {
    Write-Host "[-] Không tìm thấy Tunnel ID hợp lệ. Đang dùng mặc định learnnn-vocab.asse" -ForegroundColor Yellow
    $tunnelId = "learnnn-vocab.asse"
}

# Lưu ID cố định
Set-Content -Path $tunnelFile -Value $tunnelId -Encoding UTF8

# 4. Đảm bảo Port 5186 đã được mở trên Tunnel
$showData = devtunnel show $tunnelId -j | ConvertFrom-Json
$hasPort5186 = $false
if ($showData.tunnel -and $showData.tunnel.ports) {
    $foundPort = $showData.tunnel.ports | Where-Object { $_.portNumber -eq 5186 }
    if ($foundPort) { $hasPort5186 = $true }
}

if (-not $hasPort5186) {
    Write-Host "[*] Đang cấu hình mở cổng 5186 cho Tunnel..." -ForegroundColor Gray
    devtunnel port create $tunnelId -p 5186 --protocol http 2>&1 | Out-Null
    $showData = devtunnel show $tunnelId -j | ConvertFrom-Json
}

# Lấy URL của cổng 5186
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

# 5. Khởi động Web App Blazor (.NET 10) nếu chưa chạy
$dotnetProc = $null
$tcpConn = Get-NetTCPConnection -LocalPort 5186 -State Listen -ErrorAction SilentlyContinue
if ($tcpConn) {
    Write-Host "[+] Ứng dụng LearnNN_WebBlazor đã đang chạy trên cổng 5186." -ForegroundColor Green
} else {
    Write-Host "[*] Đang khởi chạy ứng dụng LearnNN_WebBlazor (.NET 10)..." -ForegroundColor Cyan
    $projectPath = Join-Path $PSScriptRoot "Source\LearnNN_WebBlazor.csproj"
    $dotnetProc = Start-Process -FilePath "dotnet" -ArgumentList "run --project `"$projectPath`"" -PassThru -WindowStyle Hidden
    Start-Sleep -Seconds 4
}

# 6. Hiển thị thông tin kết nối và Mã QR
Write-Host ""
Write-Host "==================================================================" -ForegroundColor Green
Write-Host "   🎉 ĐƯỜNG HẦM CỐ ĐỊNH SẴN SÀNG - TRUY CẬP TỪ ĐIỆN THOẠI" -ForegroundColor Green
Write-Host "==================================================================" -ForegroundColor Green
Write-Host ""
Write-Host " 🌐 LINK TRUY CẬP CỐ ĐỊNH (HTTPS):" -ForegroundColor Yellow
Write-Host "    $tunnelUrl" -ForegroundColor Cyan
Write-Host "    (Link này vĩnh viễn không bao giờ đổi, hãy Bookmark trên điện thoại!)" -ForegroundColor DarkGray
Write-Host ""
Write-Host " 🔐 BẢO MẬT: PRIVATE" -ForegroundColor Magenta
Write-Host "    Khi mở trên điện thoại, đăng nhập cùng tài khoản Microsoft/GitHub:" -ForegroundColor White
Write-Host "    $userShow" -ForegroundColor Gray
Write-Host ""
Write-Host " 📷 QUÉT MÃ QR DƯỚI ĐÂY BẰNG CAMERA ĐIỆN THOẠI ĐỂ MỞ:" -ForegroundColor Yellow
Write-Host ""

try {
    curl.exe -s "https://qrenco.de/$tunnelUrl"
} catch {
    Write-Host "    (Gõ trực tiếp đường link trên vào Safari/Chrome điện thoại)" -ForegroundColor Gray
}

Write-Host ""
Write-Host " 💡 Mẹo: Trên Safari (iOS) chọn 'Thêm vào MH chính', trên Android chọn 'Cài đặt' để dùng như App!" -ForegroundColor Cyan
Write-Host "------------------------------------------------------------------" -ForegroundColor DarkGray
Write-Host " Đang duy trì kết nối... Nhấn Ctrl + C để dừng." -ForegroundColor Gray
Write-Host ""

# 7. Giữ Tunnel hoạt động
try {
    devtunnel host $tunnelId
} finally {
    if ($dotnetProc -and -not $dotnetProc.HasExited) {
        Write-Host "[*] Đang dừng tiến trình .NET Blazor..." -ForegroundColor Gray
        Stop-Process -Id $dotnetProc.Id -Force -ErrorAction SilentlyContinue
    }
    Write-Host "[*] Đã đóng đường hầm. Tạm biệt!" -ForegroundColor Yellow
}
