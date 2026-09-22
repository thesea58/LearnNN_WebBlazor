# 📋 Task 01 – Khởi tạo Project & Cài đặt Packages
## Project Setup & NuGet Configuration

> **Trạng thái**: ⬜ Chưa bắt đầu  
> **Độ ưu tiên**: 🔴 Cao (Bắt buộc làm đầu tiên)  
> **Phụ thuộc**: Không  
> **Tài liệu tham chiếu**: [03_architecture.md](../../design/03_architecture.md)

---

## Mục tiêu

Tạo project Blazor Web App (.NET 10), cài đặt các gói NuGet cần thiết, cấu hình chuỗi kết nối SQLite và đảm bảo project build thành công.

---

## Danh sách công việc

### 1.1 Tạo Project Blazor Web App
- [ ] Chạy lệnh tạo project:
  ```bash
  dotnet new blazor -n LearnNN_WebBlazor -int Server --empty false
  ```
- [ ] Xác nhận render mode: **InteractiveServer**
- [ ] Kiểm tra file `.csproj` đã target đúng .NET 10

### 1.2 Cài đặt NuGet Packages
- [ ] Cài EF Core SQLite Provider:
  ```bash
  dotnet add package Microsoft.EntityFrameworkCore.Sqlite
  ```
- [ ] Cài EF Core Design (cho migrations):
  ```bash
  dotnet add package Microsoft.EntityFrameworkCore.Design
  ```
- [ ] Cài EF Core Tools:
  ```bash
  dotnet add package Microsoft.EntityFrameworkCore.Tools
  ```
- [ ] Kiểm tra EF CLI global tool:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### 1.3 Cấu hình Connection String
- [ ] Cập nhật `appsettings.json`:
  ```json
  {
    "ConnectionStrings": {
      "DefaultConnection": "Data Source=LearnNN_VocabDB.db"
    }
  }
  ```
- [ ] File `LearnNN_VocabDB.db` sẽ tự động tạo khi chạy migration (không cần cài server)

### 1.4 Tạo cấu trúc thư mục
- [ ] Tạo folder `Data/Entities/`
- [ ] Tạo folder `Services/`
- [ ] Tạo folder `Models/`
- [ ] Tạo folder `Components/Pages/Words/`
- [ ] Tạo folder `Components/Pages/Topics/`
- [ ] Tạo folder `Components/Shared/`

### 1.5 Kiểm tra
- [ ] `dotnet build` thành công, không có lỗi
- [ ] `dotnet run` mở được trang mặc định trên trình duyệt

---

## Kết quả đầu ra (Deliverables)
- Project Blazor Web App chạy được
- Tất cả NuGet packages đã cài
- Cấu trúc thư mục sẵn sàng cho các task tiếp theo
