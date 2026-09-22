# 📋 Task 03 – Service Layer (Business Logic)
## IVocabularyService & VocabularyService

> **Trạng thái**: ✅ Hoàn thành  
> **Độ ưu tiên**: 🔴 Cao  
> **Phụ thuộc**: Task 02 (Database & Entities)  
> **Tài liệu tham chiếu**: [03_architecture.md](../../design/03_architecture.md)

---

## Mục tiêu

Xây dựng tầng Service chứa toàn bộ business logic, CRUD operations cho Topic và Word. Service sử dụng `IDbContextFactory` để tạo DbContext scope riêng cho mỗi thao tác, đảm bảo an toàn trong Blazor Server.

---

## Danh sách công việc

### 3.1 Tạo Filter/ViewModel Models
- [x] Tạo `Models/WordFilterModel.cs`:
  - `SearchTerm` (string?) – tìm kiếm theo Term hoặc Meaning
  - `TopicId` (int?) – lọc theo chủ đề
  - `MasteredFilter` (enum: All / NotMastered / Mastered)
  - `Page` (int, default: 1)
  - `PageSize` (int, default: 20)
- [x] Tạo enum `MasteredFilter` (All = 0, NotMastered = 1, Mastered = 2)

### 3.2 Tạo Interface IVocabularyService
- [x] Tạo `Services/IVocabularyService.cs` với các method:
  ```
  // TOPIC
  GetAllTopicsAsync()
  GetTopicByIdAsync(int id)
  CreateTopicAsync(Topic topic)
  UpdateTopicAsync(Topic topic)
  DeleteTopicAsync(int id)
  TopicNameExistsAsync(string name, int? excludeId)

  // WORD
  GetWordsAsync(WordFilterModel filter)
  GetWordCountAsync(WordFilterModel filter)
  GetWordByIdAsync(int id)
  CreateWordAsync(Word word)
  UpdateWordAsync(Word word)
  DeleteWordAsync(int id)
  ToggleMasteredAsync(int wordId)
  ```

### 3.3 Triển khai VocabularyService
- [x] Tạo `Services/VocabularyService.cs`:
  - Inject `IDbContextFactory<AppDbContext>`
  - Mỗi method tạo DbContext riêng: `await using var db = await _factory.CreateDbContextAsync()`
  - **GetWordsAsync**: Build dynamic IQueryable với filter → search → sort → pagination
  - **ToggleMasteredAsync**: Toggle `IsMastered` = !`IsMastered` + cập nhật `UpdatedAt`
  - **TopicNameExistsAsync**: Kiểm tra trùng tên topic (case-insensitive)
  - Tất cả method đều async/await
  - Include `Topic` navigation khi lấy danh sách Word

### 3.4 Đăng ký DI trong Program.cs
- [x] Thêm: `builder.Services.AddScoped<IVocabularyService, VocabularyService>()`

### 3.5 Kiểm tra
- [x] Build thành công
- [x] Không có warning về async methods
- [x] Tất cả method trả về đúng kiểu dữ liệu

---

## Lưu ý kỹ thuật

> ⚠️ **QUAN TRỌNG**: Không sử dụng `AddDbContext` mà phải dùng `IDbContextFactory`. Trong Blazor Server, mỗi circuit (kết nối SignalR) tồn tại lâu dài – nếu dùng DbContext scoped thông thường sẽ gây lỗi concurrency và memory leak.

---

## Kết quả đầu ra (Deliverables)
- `Models/WordFilterModel.cs`
- `Services/IVocabularyService.cs`
- `Services/VocabularyService.cs`
- Program.cs đã đăng ký DI
