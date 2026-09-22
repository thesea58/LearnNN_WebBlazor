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

### 4. Thành phần Dùng chung (Shared Components)
- `ConfirmDeleteModal`: Modal xác nhận trước khi thực hiện hành động xóa nguy hiểm, hỗ trợ backdrop click, loading state khi đang xử lý.
- `LoadingSpinner`: Vòng xoay tải dữ liệu với thông báo tùy biến và các kích thước khác nhau.
- `AlertMessage`: Hộp thông báo kết quả (Success, Warning, Error, Info) hỗ trợ đóng thủ công hoặc tự động biến mất (Auto-dismiss) sau 4 giây.

---

## 🛠️ Công nghệ sử dụng

- **Framework**: .NET 10 Blazor Web App (Interactive Server render mode)
- **Data Access**: Entity Framework Core 10 với SQLite (`learnnn.db`)
- **UI & Styling**: Bootstrap 5, Bootstrap Icons, Modern CSS variables & glassmorphism
- **Design Pattern**: 3-Tier Layered Architecture (Entities -> DbContext -> Service Layer -> UI Components)

---

## 📂 Cấu trúc Dự án

```
LearnNN_WebBlazor/
├── docs/                               # Tài liệu thiết kế, task & kiến trúc
│   ├── design/                         # ERD, DB design, spec kiến trúc
│   ├── task/                           # Kế hoạch & tiến độ 9 tasks
│   └── PROJECT_MAP.md                  # Bản đồ kiến trúc chuẩn mực của dự án
│
├── Source/                             # Mã nguồn chính
│   ├── Data/                           # Data Access Layer (AppDbContext, Entities)
│   │   └── Entities/                   # Topic.cs, Word.cs
│   ├── Models/                         # DTOs, ViewModels, Enums (WordFilterModel, AlertType)
│   ├── Services/                       # Business Logic (IVocabularyService, VocabularyService)
│   ├── Components/                     # Giao diện Blazor UI
│   │   ├── Layout/                     # MainLayout, NavMenu, ReconnectModal
│   │   ├── Pages/                      # Home, Topics/TopicManage, Words/WordList, WordFormModal
│   │   └── Shared/                     # ConfirmDeleteModal, LoadingSpinner, AlertMessage
│   └── wwwroot/                        # Static assets (CSS, images, libraries)
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
