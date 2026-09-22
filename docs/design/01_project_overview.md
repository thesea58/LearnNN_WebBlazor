# 📘 LearnNN – Vocabulary Management App
## Tài liệu Tổng quan Dự án (Project Overview)

> **Phiên bản**: 1.0  
> **Ngày tạo**: 2026-09-22  
> **Trạng thái**: Draft – Chờ phê duyệt kiến trúc

---

## 1. Đánh giá Prompt ban đầu

### ✅ Điểm mạnh của Prompt

| Tiêu chí | Nhận xét |
|---|---|
| **Độ rõ ràng về Tech Stack** | ✅ Rất tốt – xác định rõ .NET 10/8/9, Blazor Web App, EF Core, SQL Server |
| **Mô hình dữ liệu** | ✅ Tốt – 2 entity `Topic` và `Word` với đủ thuộc tính và kiểu dữ liệu |
| **Phân tầng kiến trúc** | ✅ Đúng hướng – có Services layer (IVocabularyService), tách UI khỏi DbContext |
| **Tính năng CRUD** | ✅ Đầy đủ CRUD cho cả Word và Topic |
| **Yêu cầu UI cụ thể** | ✅ Có xác định table view, modal confirm, tìm kiếm, lọc, toggle status |
| **Hướng dẫn chạy** | ✅ Có step-by-step CLI commands |

### ⚠️ Điểm cần bổ sung / Rủi ro kỹ thuật

| Vấn đề | Mức độ | Đề xuất khắc phục |
|---|---|---|
| **Phân trang (Pagination)** | 🔴 Thiếu | Thêm server-side pagination hoặc virtual scroll khi dữ liệu lớn |
| **Authentication & Authorization** | 🟡 Không có | MVP có thể bỏ qua, nhưng cần ghi chú để mở rộng sau |
| **Chức năng học từ (Quiz/Flashcard)** | 🟡 Thiếu | Prompt chỉ có CRUD, không có tính năng học – cần định nghĩa rõ |
| **Import/Export dữ liệu** | 🟡 Thiếu | Không có chức năng import CSV/Excel |
| **Quản lý lỗi (Error Handling)** | 🟡 Mơ hồ | Cần định nghĩa global error boundary cho Blazor |
| **Tìm kiếm** | 🟠 Cơ bản | Chỉ tìm theo Term/Meaning, chưa có full-text search hoặc debounce |
| **Caching** | 🟠 Thiếu | Không đề cập caching cho Topic list (thường ít thay đổi) |
| **Unit Tests** | 🔴 Thiếu | Không có yêu cầu test, khó đảm bảo chất lượng Service layer |
| **Render Mode** | 🟠 Cần xem lại | InteractiveServer + DbContext có thể gây memory leak nếu dùng AddDbContext thay vì AddDbContextFactory |
| **SQLite Limitations** | 🟠 Lưu ý | SQLite không hỗ trợ concurrent write tốt, phù hợp cho single-user/dev, cần chuyển sang PostgreSQL/SQL Server khi scale |
| **Soft Delete** | 🟡 Thiếu | Xóa cứng (hard delete) có thể gây mất dữ liệu ngoài ý muốn |

### 🎯 Kết luận đánh giá

**Prompt đạt 7.5/10** – Rất tốt cho việc xây dựng MVP nhanh. Tuy nhiên cần bổ sung:
1. Tính năng học từ thực sự (Flashcard / Quiz mode)
2. Phân trang bắt buộc
3. Chiến lược Error Handling
4. Cấu hình DbContextFactory đúng cho Blazor Server

---

## 2. Phạm vi Dự án (Scope)

### In Scope (V1)
- [x] CRUD Topic (Chủ đề)
- [x] CRUD Word (Từ vựng) với đầy đủ fields
- [x] Tìm kiếm theo Term / Meaning
- [x] Lọc theo Topic và IsMastered
- [x] Toggle trạng thái IsMastered nhanh
- [x] Modal confirm xóa
- [x] Seed data mẫu
- [x] Responsive UI với Bootstrap 5

### Out of Scope (V1 – Dành cho V2)
- [ ] Authentication (đăng nhập / đăng ký)
- [ ] Flashcard / Quiz mode
- [ ] Import/Export CSV
- [ ] Audio phiên âm (Text-to-Speech)
- [ ] Progress tracking / Statistics dashboard
- [ ] Multi-user support

---

## 3. Tech Stack chính thức

| Thành phần | Công nghệ | Phiên bản |
|---|---|---|
| Framework | .NET | 10.0 |
| UI Framework | Blazor Web App | InteractiveServer |
| CSS Framework | Bootstrap | 5.3 |
| ORM | Entity Framework Core | 9.x |
| Database | SQLite | 3.x (file-based) |
| Language | C# | 13.0 |
