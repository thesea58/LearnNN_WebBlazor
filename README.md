# 📘 LearnNN – Vocabulary Management App

Ứng dụng quản lý và trau dồi từ vựng tiếng Anh cá nhân được xây dựng trên nền tảng **Blazor Web App (.NET 10)** với kiến trúc phân tầng 3 lớp (3-Tier Layered Architecture) và cơ sở dữ liệu **SQLite** thông qua Entity Framework Core.

---

## 🚀 Tính năng chính

### 1. Dashboard tổng quan (`/`)
- Thống kê tổng số từ vựng, số từ đã thuộc (`IsMastered = true`), số từ cần ôn tập và số lượng chủ đề.
- Thanh tiến độ trực quan hiển thị tỷ lệ phần trăm từ vựng đã nắm vững.
- Lối tắt thao tác nhanh đến các chức năng thêm từ, tra cứu danh sách và quản lý chủ đề.
- Bảng xem trước các từ vựng được thêm gần đây nhất kèm nhãn phân loại.

### 2. Quản lý Chủ đề (`/topics`)
- Danh sách chủ đề học tập hiển thị số lượng từ vựng trong từng nhóm.
- Thêm mới và chỉnh sửa chủ đề với validation độ dài và kiểm tra trùng tên.
- Xóa chủ đề an toàn với modal xác nhận (`ConfirmDeleteModal`), cảnh báo rõ ràng khi xóa kèm cascade toàn bộ từ vựng liên quan.

### 3. Quản lý Kho Từ vựng (`/words`)
- Hiển thị danh sách từ vựng chi tiết: từ gốc, phiên âm IPA, từ loại, định nghĩa, câu ví dụ tiếng Anh và bản dịch tiếng Việt.
- Thanh tìm kiếm từ vựng hoặc nghĩa với cơ chế **Debounce (300ms)** giúp tối ưu hiệu năng.
- Bộ lọc đa tiêu chí: theo chủ đề và theo trạng thái ghi nhớ (Tất cả / Đã thuộc / Chưa thuộc).
- Đổi trạng thái thuộc từ (`IsMastered`) nhanh chóng với 1 cú click ngay trên bảng.
- Phân trang server-side linh hoạt (20 từ/trang).
- Form modal thêm/sửa từ vựng với đầy đủ validation (`DataAnnotationsValidator`).

### 4. Trung tâm Học tập & Trò chơi Từ vựng (`/study`)
- **Flashcard 3D**: Lật thẻ 3D trực quan, phát âm Audio Web Speech API, tích hợp thuật toán lặp lại ngắt quãng SuperMemo-2 (SM-2) với 4 mức đánh giá (Quên, Khó, Tốt, Dễ) và Anki-style re-queueing trong phiên.
- **Game Ghép đôi (Word Match)**: Nối từ tiếng Anh và nghĩa tiếng Việt với đồng hồ bấm giờ và đánh giá số bước.
- **Game Xếp chữ (Word Scramble)**: Xếp các ký tự đảo lộn để đoán từ vựng với gợi ý âm thanh và bóc tách chữ cái.
- **Bài kiểm tra Trắc nghiệm (Quiz)**: Luyện tập 4 lựa chọn, tự động sinh distractor từ kho từ vựng và lưu vết tỷ lệ chính xác.

### 5. Hạ tầng Trí tuệ Nhân tạo (AI Infrastructure)
- **3 Chế độ hoạt động**: Auto (Gemini API), Manual Bridge (Copy/Paste miễn phí 100%), và Hybrid (gọi API tự động, tự hạ cấp sang Manual khi mạng yếu hoặc chạm giới hạn 429).
- **AI Quiz Explainer & Trap Detector**: Bóc tách bẫy trắc nghiệm, giải thích ngữ pháp và cung cấp mẹo nhớ trực tiếp trên từng câu sai trong Quiz.
- **AI Inbox & Sandbox (`/ai/inbox`)**: Quản lý lịch sử prompt, phòng thí nghiệm thử nghiệm sinh dữ liệu từ vựng.

### 6. Trình bày Đa chế độ & Trợ năng (Multi-View & Accessibility)
- **Kiểu Dòng (Table) & Kiểu Thẻ Icon (Card Grid)**: Xem danh sách theo bảng chi tiết hoặc dạng lưới thẻ chạm mượt mà trên điện thoại.
- **Trợ năng (A11y)**: Hỗ trợ chỉ báo `:focus-visible` chuẩn WCAG 2.4.7, thuộc tính `aria-label` cho toàn bộ các nút icon, thẻ `<html lang="vi">` và tiện ích đọc màn hình `.sr-only`.

### 7. Trải nghiệm Di động & PWA (Web Mobile & PWA)
- **Giao diện Điện thoại Chuẩn mực (<1024px)**: Thanh điều hướng dưới đáy (Bottom-nav) 5 mục, bảng tự động chuyển `table-responsive-stack`, chống bàn phím ảo che input.
- **Khóa Bố cục PC (>=1024px)**: Bảo toàn 100% giao diện và hành vi máy tính để bàn (được bảo vệ bởi 20 baseline screenshots).
- **Cài đặt PWA (Standalone)**: Cấu hình `manifest.webmanifest`, bộ icon PNG đa kích thước, chạy toàn màn hình không có thanh URL.
- **Blazor Server Resilience**: Tự động kết nối lại khi quay lại tab / đổi mạng, trang `offline.html` dự phòng độc lập, lưu giữ phiên học 10 phút.
- **Khởi chạy 1-Click (`start-mobile.bat`)**: Tự động mở đường hầm Microsoft Dev Tunnels HTTPS cố định và hiển thị mã QR để quét bằng camera điện thoại.

---

## 🛠️ Công nghệ sử dụng

- **Framework**: .NET 10 Blazor Web App (Interactive Server render mode)
- **Data Access**: Entity Framework Core 10 với SQLite (`LearnNN_VocabDB.db`)
- **AI Engine**: Google AI Studio Gemini API (`gemini-3.5-flash`, `gemini-3.5-flash-lite`) & Manual Bridge
- **PWA & Offline**: Service Worker (Cache-First static assets, SignalR bypass, Offline fallback)
- **UI & Styling**: Bootstrap 5, Bootstrap Icons, CSS custom properties & Glassmorphism
- **Automated Testing**: Playwright .NET, NUnit, SkiaSharp visual pixel-by-pixel comparison (121 tests pass 100%)
- **Design Pattern**: 3-Tier Layered Architecture (Entities -> DbContext -> Service Layer -> UI Components)

---

## 📂 Cấu trúc Dự án

```
LearnNN_WebBlazor/
├── docs/                               # Tài liệu thiết kế, task & kiến trúc
│   ├── design/                         # ERD, DB design, spec kiến trúc, deployment guide
│   ├── task/                           # Kế hoạch & tiến độ 25 tasks
│   └── PROJECT_MAP.md                  # Bản đồ kiến trúc chuẩn mực của dự án
│
├── Source/                             # Mã nguồn chính
│   ├── Data/                           # Data Access Layer (AppDbContext, Entities)
│   ├── Models/                         # DTOs, ViewModels, Enums, AI options
│   ├── Services/                       # Business Logic (Vocabulary, SRS Engine, AI Services)
│   ├── Components/                     # Giao diện Blazor UI (Layout, Pages, Shared)
│   └── wwwroot/                        # Static assets (CSS, Icons PWA, Service Worker)
│
├── Tests/                              # Kiểm thử tự động E2E & Visual Regression
│   └── LearnNN_WebBlazor.Tests/        # Playwright .NET test suite (121 tests, 20 PC baselines)
│
└── start-mobile.bat                    # Script khởi chạy 1-click truy cập từ điện thoại qua QR Code
```

---

## 💻 Hướng dẫn Cài đặt & Khởi chạy

### Yêu cầu môi trường
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- Công cụ `dotnet-ef` (nếu cần quản lý migration)

### Các bước khởi chạy

```bash
# 1. Di chuyển vào thư mục dự án
cd d:\DEV_NET\LearnNN_WebBlazor

# 2. Khôi phục dependencies
dotnet restore

# 3. Áp dụng migration để tạo database SQLite & nạp dữ liệu mẫu
dotnet ef database update --project Source/LearnNN_WebBlazor.csproj

# 4. Chạy ứng dụng
dotnet run --project Source/LearnNN_WebBlazor.csproj
```

Ứng dụng sẽ khả dụng tại:
- **HTTP**: `http://localhost:5186`
- **HTTPS**: `https://localhost:7195`

---

## 📝 Bản quyền & Giấy phép
Dự án được xây dựng phục vụ mục đích học tập và trau dồi ngoại ngữ.
