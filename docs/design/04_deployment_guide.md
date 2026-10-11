# 📱 Hướng dẫn Triển khai (Deploy) LearnNN_WebBlazor ra Public & Truy cập từ Điện thoại

> **Tài liệu**: Hướng dẫn nghiên cứu và triển khai thực chiến  
> **Áp dụng cho**: Dự án `LearnNN_WebBlazor` (.NET 10, Blazor Server, SQLite, SignalR)  
> **Cập nhật**: 2026-10-11

---

## 1. 🔍 Phân tích Đặc thù Kỹ thuật của Ứng dụng

Trước khi deploy, cần nắm rõ các đặc tính kiến trúc cốt lõi của **LearnNN_WebBlazor**:

| Đặc tính | Chi tiết kỹ thuật | Ảnh hưởng khi Deploy |
|---|---|---|
| **Render Mode** | Blazor Interactive Server (`InteractiveServerRenderMode`) | Client duy trì kết nối **SignalR qua WebSocket** liên tục. Reverse Proxy (Nginx, Caddy, Cloudflare) **bắt buộc phải bật WebSocket support** (`Upgrade`, `Connection`). |
| **Cơ sở dữ liệu** | SQLite (`LearnNN_VocabDB.db`) | Lưu trữ dạng file trên ổ đĩa. Nơi deploy **bắt buộc phải có Persistent Volume (ổ cứng cố định)**. *Không dùng các host serverless stateless (như Vercel, Netlify hay Render Free Web Service không có disk)* vì dữ liệu học tập và SRS sẽ bị xóa sạch mỗi khi container restart. |
| **Giao tiếp Web API** | Web Speech API (phát âm từ vựng), Clipboard API (AI Manual Bridge) | Trình duyệt di động (iOS Safari, Android Chrome) chỉ kích hoạt các tính năng này trong **Secure Context (HTTPS)**. Nếu dùng HTTP thường, các tính năng copy và âm thanh có thể bị chặn. |
| **Nền tảng Runtime** | .NET 10 Web App | Cần máy chủ có .NET 10 Runtime hoặc đóng gói qua **Docker container** (`mcr.microsoft.com/dotnet/aspnet:10.0`). |

---

## 2. 📊 Bảng So sánh các Phương án Triển khai

| Tiêu chí | Cách 1: Cloudflare Tunnel *(Khuyên dùng để test ngay)* | Cách 2: Mạng nội bộ Wi-Fi (LAN) | Cách 3: VPS Linux + Docker + Caddy *(Chuẩn 24/7 lâu dài)* | Cách 4: Cloud PaaS (Fly.io / Railway) |
|---|---|---|---|---|
| **Thời gian thiết lập** | **2 phút** | 1 phút | 15 - 30 phút | 10 phút |
| **Chi phí** | **0đ (Miễn phí 100%)** | 0đ | 0đ (Oracle Free) hoặc ~70k-100k/tháng | Miễn phí theo hạn mức / Pay-as-you-go |
| **Truy cập ngoài đường (4G)** | ✅ Có (Public toàn cầu) | ❌ Không (Chỉ cùng Wi-Fi) | ✅ Có (Public toàn cầu) | ✅ Có (Public toàn cầu) |
| **Có sẵn HTTPS (SSL)** | ✅ Có (Cloudflare SSL hợp lệ) | ❌ Không (HTTP thường) | ✅ Có (Caddy tự cấp Let's Encrypt) | ✅ Có (Domain *.fly.dev / *.up.railway.app) |
| **Tắt máy tính vẫn chạy** | ❌ Phụ thuộc máy tính chạy | ❌ Phụ thuộc máy tính chạy | ✅ **24/7 trên Cloud** | ✅ **24/7 trên Cloud** |
| **Bảo toàn dữ liệu SQLite** | ✅ Lưu ngay trên ổ cứng máy | ✅ Lưu ngay trên ổ cứng máy | ✅ Lưu qua Docker Volume (`./data`) | ✅ Cần cấu hình Persistent Volume |

---

## 3. 🚀 Hướng dẫn Chi tiết Từng Phương án

### 🌟 PHƯƠNG ÁN 1A: Microsoft Dev Tunnels – Script 1-Click *(KHUYÊN DÙNG HÀNG ĐẦU – LINK CỐ ĐỊNH & BẢO MẬT PRIVATE)*

Đây là phương án tối ưu nhất giải quyết trọn vẹn mong muốn: **Link cố định vĩnh viễn (không bao giờ đổi qua các lần khởi động)**, **hoàn toàn miễn phí**, **không cần mua domain**, và **bảo vệ tài khoản an toàn (Private - chỉ bạn mới vào được)**.

```
[Điện thoại (4G/Wi-Fi)] 
       │ (HTTPS https://learnnn-vocab-5186.asse.devtunnels.ms)
       │ (Xác thực tài khoản GitHub / Microsoft của bạn)
       ▼
[Microsoft Dev Tunnels Gateway]
       │ (Đường hầm mã hóa bảo mật)
       ▼
[Máy tính của bạn (start-mobile.bat)] ──► [LearnNN_WebBlazor (.NET 10 Blazor)]
```

#### Ưu điểm vượt trội:
* **Link cố định vĩnh viễn**: URL không bao giờ bị đổi mỗi lần tắt/bật lại máy tính. Bạn có thể lưu Bookmark hoặc Add to Home Screen trên điện thoại 1 lần duy nhất để dùng mãi mãi.
* **Bảo mật Private**: Bắt buộc đăng nhập tài khoản Microsoft hoặc GitHub của chính bạn khi truy cập trên điện thoại, ngăn chặn người lạ xem trộm dữ liệu học tập hoặc tiêu hao hạn mức AI.
* **1-Click khởi động**: Dự án đã tích hợp sẵn script `start-mobile.bat` – bấm 1 phát tự động chạy app, mở tunnel và in mã QR ngay trên màn hình.

#### Cách sử dụng:
1. **Bước 1**: Nhấp đúp chuột vào file `start-mobile.bat` tại thư mục gốc của dự án.
2. **Bước 2 (Lần đầu tiên)**: Trình duyệt sẽ mở ra yêu cầu đăng nhập tài khoản GitHub hoặc Microsoft (chỉ cần làm 1 lần duy nhất).
3. **Bước 3**: Màn hình terminal sẽ in ra đường link cố định và **Mã QR trực tiếp**.
4. **Bước 4**: Dùng camera điện thoại quét mã QR hoặc mở link `https://...` trên Safari/Chrome điện thoại -> Đăng nhập tài khoản Microsoft/GitHub của bạn -> Bắt đầu học từ vựng!

---

### ⚡ PHƯƠNG ÁN 1B: Cloudflare Quick Tunnel (Link tạm thời – Không cần đăng nhập)

Dành cho trường hợp bạn muốn test nhanh không muốn đăng nhập tài khoản Microsoft/GitHub:
```powershell
dotnet run --project Source/LearnNN_WebBlazor.csproj
cloudflared tunnel --url http://localhost:5186
```
*(Lưu ý: Link sẽ thay đổi ngẫu nhiên mỗi lần chạy lại lệnh)*.
1. Nhập link `https://random-subdomain-1234.trycloudflare.com` vào trình duyệt điện thoại (Safari trên iOS hoặc Chrome trên Android).
2. *(Mẹo)*: Bạn có thể tạo mã QR nhanh bằng cách copy link đó vào trang tạo QR hoặc dùng tiện ích browser để quét bằng camera điện thoại.
3. Toàn bộ tính năng (Lật thẻ Flashcard 3D, Nghe phát âm từ vựng, Game nối từ, Quiz trắc nghiệm, AI Bridge) hoạt động 100% mượt mà!

---

### 🏠 PHƯƠNG ÁN 2: Truy cập qua Mạng Wi-Fi Nội bộ (LAN)

Thích hợp khi bạn chỉ muốn nằm trên giường hoặc ngồi sofa cầm điện thoại học mà không cần kết nối ra ngoài Internet. Cả điện thoại và máy tính phải bắt chung một mạng Wi-Fi.

#### Bước 1: Cho phép Kestrel lắng nghe mọi địa chỉ IP
Mở terminal và chạy lệnh với cờ `--urls`:
```powershell
dotnet run --project Source/LearnNN_WebBlazor.csproj --urls "http://0.0.0.0:5186"
```

#### Bước 2: Mở port trên Windows Firewall
Chạy PowerShell với quyền Administrator để cho phép điện thoại kết nối vào cổng 5186:
```powershell
New-NetFirewallRule -DisplayName "LearnNN Blazor Dev" -Direction Inbound -LocalPort 5186 -Protocol TCP -Action Allow
```

#### Bước 3: Lấy địa chỉ IP nội bộ của máy tính
Gõ lệnh:
```powershell
ipconfig
```
Tìm dòng **IPv4 Address** của card Wi-Fi (ví dụ: `192.168.1.15`).

#### Bước 4: Truy cập từ điện thoại
Mở Safari/Chrome trên điện thoại và gõ:
```text
http://192.168.1.15:5186
```

---

### ☁️ PHƯƠNG ÁN 3: Triển khai 24/7 lên VPS Linux với Docker & Caddy (Khuyên dùng cho Học lâu dài)

Phương án này giúp bạn tắt máy tính thì ứng dụng vẫn chạy trên Cloud 24/7, tự động backup dữ liệu từ vựng, có tên miền riêng và chứng chỉ SSL miễn phí trọn đời.

#### Lựa chọn nhà cung cấp VPS:
1. **Oracle Cloud Always Free (Khuyên dùng số 1 - Miễn phí 0đ vĩnh viễn)**: Đăng ký gói Free Tier nhận máy ảo ARM Ampere 4 OCPU, 24GB RAM, 200GB SSD hoàn toàn miễn phí.
2. **VPS giá rẻ tại Việt Nam / Quốc tế**: Vietnix, TinoHost, CloudFly, Hetzner, DigitalOcean (giá chỉ từ ~70.000đ - 100.000đ/tháng).

#### Quy trình triển khai:

```
[Internet / Mobile]
       │ (Port 80/443 - HTTPS)
       ▼
[Caddy Reverse Proxy] (Tự động xin & gia hạn Let's Encrypt SSL)
       │ (Proxy WebSocket & HTTP)
       ▼
[Docker Container: learnnn-app]
       │
   (Volume ./data) ──► File SQLite: LearnNN_VocabDB.db
```

#### Bước 1: Cài đặt Docker & Docker Compose trên VPS (Ubuntu)
```bash
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh
sudo usermod -aG docker $USER
```

#### Bước 2: Clone source code về VPS
```bash
git clone https://github.com/your-username/LearnNN_WebBlazor.git
cd LearnNN_WebBlazor
```

#### Bước 3: Khởi chạy ứng dụng với Docker Compose
Dự án đã có sẵn file `Dockerfile` và `docker-compose.yml`. Chỉ cần chạy:
```bash
docker compose up -d --build
```
Ứng dụng sẽ tự động tải SDK .NET 10, build bản Release và chạy container trên cổng `5186`. Toàn bộ dữ liệu SQLite được lưu an toàn tại thư mục `./data/LearnNN_VocabDB.db` trên VPS (không bao giờ bị mất khi khởi động lại).

#### Bước 4: Cài Caddy để có HTTPS tự động (Reverse Proxy)
Cài đặt Caddy trên Ubuntu:
```bash
sudo apt install -y debian-keyring debian-archive-keyring apt-transport-https curl
curl -1sLf 'https://dl.cloudsmith.io/public/caddy/stable/gpg.key' | sudo gpg --dearmor -o /usr/share/keyrings/caddy-stable-archive-keyring.gpg
curl -1sLf 'https://dl.cloudsmith.io/public/caddy/stable/debian.deb.txt' | sudo tee /etc/apt/sources.list.d/caddy-stable.list
sudo apt update && sudo apt install caddy
```

Chỉnh sửa file cấu hình Caddy: `/etc/caddy/Caddyfile`
```caddy
# Nếu có tên miền (vd: vocab.yourdomain.com):
vocab.yourdomain.com {
    reverse_proxy localhost:5186
}

# Hoặc nếu chỉ dùng IP VPS (tự tạo SSL tự ký):
:443 {
    reverse_proxy localhost:5186
}
```
Khởi động lại Caddy:
```bash
sudo systemctl reload caddy
```
Bây giờ bạn có thể truy cập `https://vocab.yourdomain.com` trên điện thoại từ bất kỳ nơi đâu!

---

### 🌐 PHƯƠNG ÁN 4: Triển khai lên Cloud PaaS (Fly.io)

Nếu không muốn tự cài đặt hệ điều hành Linux trên VPS, bạn có thể dùng **Fly.io**:

1. Cài đặt Fly CLI:
   ```powershell
   powershell -Command "iwr https://fly.io/install.ps1 -useb | iex"
   ```
2. Đăng nhập: `fly auth login`
3. Khởi tạo app từ thư mục dự án: `fly launch --no-deploy`
4. Tạo Volume lưu trữ cho SQLite (đảm bảo không bị mất dữ liệu):
   ```bash
   fly volumes create vocab_data --size 1
   ```
5. Trong file `fly.toml`, cấu hình mount volume vào `/app/data`:
   ```toml
   [mounts]
     source = "vocab_data"
     destination = "/app/data"
   ```
6. Deploy:
   ```bash
   fly deploy
   ```
   Fly.io sẽ cấp cho bạn một domain HTTPS miễn phí: `https://ten-ung-dung.fly.dev`.

---

## 4. 📲 Tối ưu Trải nghiệm trên Điện thoại (Biến Web thành "App")

Để học từ vựng thuận tiện nhất mà không bị thanh địa chỉ trình duyệt làm vướng mắt:

### 🍎 Dành cho iPhone / iPad (Safari)
1. Mở trang web trên trình duyệt Safari.
2. Bấm vào nút **Chia sẻ** (biểu tượng hình vuông có mũi tên trỏ lên ở giữa thanh công cụ dưới cùng).
3. Cuộn xuống và chọn **"Thêm vào MH chính" (Add to Home Screen)**.
4. Đặt tên (ví dụ: *LearnNN*) rồi nhấn **Thêm**.
5. Biểu tượng ứng dụng sẽ xuất hiện trên màn hình chính như một ứng dụng iOS thật. Khi bấm vào sẽ mở toàn màn hình (Standalone Mode).

### 🤖 Dành cho Android (Google Chrome)
1. Mở trang web trên trình duyệt Chrome.
2. Bấm vào biểu tượng **3 dấu chấm** ở góc trên bên phải.
3. Chọn **"Thêm vào màn hình chính" (Add to Home screen)** hoặc **"Cài đặt ứng dụng"**.
4. Ứng dụng sẽ xuất hiện trong màn hình ứng dụng của Android.

---

## 5. 🛡️ Lưu ý Quan trọng về Vận hành & Bảo mật

1. **Bảo vệ API Key Gemini**:
   - Trong `Source/Program.cs`, dự án đã được cấu hình đọc `GEMINI_API_KEY` từ **Environment Variable** (biến môi trường).
   - Khi public ra ngoài, không nên hardcode API key vào file `appsettings.json` công khai trên GitHub. Thay vào đó, hãy truyền qua biến môi trường của Docker hoặc VPS.
2. **Cơ chế Reconnect của Blazor Server khi khóa màn hình**:
   - Khi điện thoại tắt màn hình hoặc chuyển sang ứng dụng khác (Zalo/Facebook), hệ điều hành sẽ tạm dừng kết nối WebSocket.
   - Khi bạn mở lại điện thoại, modal Reconnect (`ReconnectModal.razor`) sẽ tự động xuất hiện và kết nối lại sau 1-2 giây.
3. **Sao lưu dữ liệu SQLite định kỳ**:
   - Để an toàn dữ liệu học tập và chuỗi SRS SM-2, hãy tạo cron job sao lưu file `LearnNN_VocabDB.db` hàng ngày (hoặc dùng tính năng Export CSV/JSON có sẵn trong mục Cài đặt của app).
