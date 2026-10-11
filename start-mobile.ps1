# ==============================================================================
# LearnNN_WebBlazor - 1-Click Mobile Access Script (Microsoft Dev Tunnels)
# ==============================================================================

[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$ErrorActionPreference = "Stop"

# Refresh Environment PATH in case devtunnel was recently installed
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")

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
    Write-Host "[*] Trình duyệt sẽ mở ra sau giây lát để bạn xác thực..." -ForegroundColor Cyan
    devtunnel user login
}

# 3. Quản lý Tunnel Cố định (Persistent Tunnel)
$tunnelFile = Join-Path $PSScriptRoot ".devtunnel_id"
$tunnelId = ""

if (Test-Path $tunnelFile) {
    $tunnelId = (Get-Content $tunnelFile -Raw).Trim()
}

$tunnelValid = $false
if (-not [string]::IsNullOrWhiteSpace($tunnelId)) {
    try {
        $showJson = devtunnel show $tunnelId -j 2>$null | ConvertFrom-Json
        if ($showJson -and $showJson.tunnelId) {
            $tunnelValid = $true
        }
    } catch {
        $tunnelValid = $false
    }
}

if (-not $tunnelValid) {
    Write-Host "[*] Đang tạo đường hầm cố định vĩnh viễn (LearnNN Private Tunnel)..." -ForegroundColor Cyan
    $createOutput = $null
    try {
        $createOutput = devtunnel create learnnn-vocab -j 2>$null | ConvertFrom-Json
    } catch {
        # Nếu tên 'learnnn-vocab' đã trùng trong tenant, tạo ID ngẫu nhiên nhưng cố định
        $createOutput = devtunnel create -j 2>$null | ConvertFrom-Json
    }

    if ($createOutput -and $createOutput.tunnelId) {
        $tunnelId = $createOutput.tunnelId
        Set-Content -Path $tunnelFile -Value $tunnelId -Encoding UTF8
        Write-Host "[+] Đã đăng ký Tunnel ID: $tunnelId" -ForegroundColor Green
        
        # Mở port 5186 cho web Blazor (Private: không kèm --allow-anonymous)
        devtunnel port create $tunnelId -p 5186 --protocol http | Out-Null
    } else {
        Write-Host "[-] Không thể tạo Tunnel. Vui lòng kiểm tra kết nối mạng." -ForegroundColor Red
        pause
        exit 1
    }
}

# 4. Lấy URL cố định
$showData = devtunnel show $tunnelId -j | ConvertFrom-Json
$tunnelUrl = ""

# Lấy URL của port 5186
if ($showData.ports) {
    $port5186 = $showData.ports | Where-Object { $_.portNumber -eq 5186 }
    if ($port5186) {
        $tunnelUrl = $port5186.portUri
    }
}

if ([string]::IsNullOrWhiteSpace($tunnelUrl)) {
    # Format chuẩn Microsoft Dev Tunnels: https://<tunnelId>-5186.<clusterId>.devtunnels.ms
    $clusterId = if ($showData.clusterId) { $showData.clusterId } else { "asse" }
    $tunnelUrl = "https://$tunnelId-5186.$clusterId.devtunnels.ms"
}

# 5. Khởi động Web App Blazor (.NET 10) nếu chưa chạy
$dotnetProc = $null
$isAppRunning = $false

# Kiểm tra port 5186 có đang lắng nghe không
$tcpConn = Get-NetTCPConnection -LocalPort 5186 -State Listen -ErrorAction SilentlyContinue
if ($tcpConn) {
    Write-Host "[+] Ứng dụng LearnNN_WebBlazor đã đang chạy trên cổng 5186." -ForegroundColor Green
    $isAppRunning = $true
} else {
    Write-Host "[*] Đang khởi chạy ứng dụng LearnNN_WebBlazor (.NET 10)..." -ForegroundColor Cyan
    $projectPath = Join-Path $PSScriptRoot "Source\LearnNN_WebBlazor.csproj"
    $dotnetProc = Start-Process -FilePath "dotnet" -ArgumentList "run --project `"$projectPath`"" -PassThru -WindowStyle Hidden
    Start-Sleep -Seconds 3
}

# 6. Hiển thị thông tin kết nối và Mã QR
Write-Host ""
Write-Host "==================================================================" -ForegroundColor Green
Write-Host "   🎉 ĐƯỜNG HẦM CỐ ĐỊNH SẴN SÀNG - TRUY CẬP TỪ ĐIỆN THOẠI" -ForegroundColor Green
Write-Host "==================================================================" -ForegroundColor Green
Write-Host ""
Write-Host " 🌐 LINK TRUY CẬP CỐ ĐỊNH (HTTPS):" -ForegroundColor Yellow
Write-Host "    $tunnelUrl" -ForegroundColor Cyan -NoNewline
Write-Host "  (Link này vĩnh viễn không bao giờ đổi!)" -ForegroundColor DarkGray
Write-Host ""
Write-Host " 🔐 CHẾ ĐỘ BẢO MẬT: PRIVATE" -ForegroundColor Magenta
Write-Host "    Khi mở trên điện thoại, đăng nhập cùng tài khoản Microsoft/GitHub" -ForegroundColor White
Write-Host "    để bảo vệ dữ liệu học tập và số lượt AI của riêng bạn." -ForegroundColor White
Write-Host ""
Write-Host " 📷 QUÉT MÃ QR DƯỚI ĐÂY BẰNG CAMERA ĐIỆN THOẠI ĐỂ MỞ:" -ForegroundColor Yellow
Write-Host ""

try {
    # Sinh mã QR trực tiếp trong Console
    curl.exe -s "https://qrenco.de/$tunnelUrl"
} catch {
    Write-Host "    (Gõ trực tiếp đường link trên vào Safari/Chrome điện thoại)" -ForegroundColor Gray
}

Write-Host ""
Write-Host " 💡 Mẹo: Trên Safari (iOS) bấm 'Thêm vào MH chính', trên Android bấm 'Cài đặt' để dùng như App!" -ForegroundColor Cyan
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
