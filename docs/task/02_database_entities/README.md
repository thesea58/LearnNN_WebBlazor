# 📋 Task 02 – Thiết kế Database & Entity Classes
## Database, Entities, DbContext & Migration

> **Trạng thái**: ⬜ Chưa bắt đầu  
> **Độ ưu tiên**: 🔴 Cao  
> **Phụ thuộc**: Task 01 (Project Setup)  
> **Tài liệu tham chiếu**: [02_database_design.md](../../design/02_database_design.md), [database_design.xml](../../design/database_design.xml)

---

## Mục tiêu

Tạo các Entity classes (Topic, Word), cấu hình AppDbContext với Fluent API, thêm Seed Data mẫu, chạy Migration và tạo database SQLite.

---

## Danh sách công việc

### 2.1 Tạo Entity Classes
- [ ] Tạo `Data/Entities/Topic.cs`:
  - Thuộc tính: Id, Name (Required, MaxLength 100), Description (MaxLength 250), CreatedAt, UpdatedAt
  - Navigation: `ICollection<Word> Words`
  - Data Annotations cho validation
- [ ] Tạo `Data/Entities/Word.cs`:
  - Thuộc tính: Id, TopicId (FK), Term, Phonetic, PartOfSpeech, Meaning, ExampleSentence, ExampleTranslation, IsMastered, CreatedAt, UpdatedAt
  - Navigation: `Topic Topic`
  - Data Annotations cho validation

### 2.2 Tạo AppDbContext
- [ ] Tạo `Data/AppDbContext.cs`:
  - Kế thừa `DbContext`
  - Khai báo `DbSet<Topic>` và `DbSet<Word>`
  - Cấu hình Fluent API trong `OnModelCreating`:
    - Unique index trên `Topics.Name`
    - Index trên `Words.TopicId`, `Words.Term`, `Words.IsMastered`
    - Default value cho `IsMastered` (false) và `CreatedAt` (set trong C# code: `DateTime.UtcNow`)
    - Relationship: Topic (1) → Word (N) với Cascade Delete

### 2.3 Seed Data
- [ ] Thêm 3 Topics mẫu: "Công nghệ", "Giao tiếp", "IELTS Academic"
- [ ] Thêm 7 Words mẫu phân bổ vào 3 topics
- [ ] Đảm bảo seed data có đủ trường dữ liệu (phonetic, example, translation...)

### 2.4 Đăng ký DbContext trong Program.cs
- [ ] Sử dụng `AddDbContextFactory<AppDbContext>` (KHÔNG dùng `AddDbContext`)
- [ ] Cấu hình `UseSqlite(connectionString)`

### 2.5 Tạo Migration & Database
- [ ] Chạy migration:
  ```bash
  dotnet ef migrations add InitialCreate
  ```
- [ ] Cập nhật database:
  ```bash
  dotnet ef database update
  ```
- [ ] (Tùy chọn) Thêm auto-migrate trong `Program.cs`

### 2.6 Kiểm tra
- [ ] File `LearnNN_VocabDB.db` đã tạo trong thư mục project
- [ ] Bảng `Topics` và `Words` có đúng schema
- [ ] Seed data đã được insert
- [ ] Quan hệ FK hoạt động đúng (cascade delete)

---

## Kết quả đầu ra (Deliverables)
- `Topic.cs`, `Word.cs` với Data Annotations
- `AppDbContext.cs` với Fluent API + Seed Data
- Migration files trong folder `Migrations/`
- File `LearnNN_VocabDB.db` sẵn sàng với dữ liệu mẫu
