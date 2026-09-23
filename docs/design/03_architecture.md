# 🏗️ LearnNN – Architecture & Project Structure
## Kiến trúc Hệ thống & Cấu trúc Thư mục

> **Phiên bản**: 1.0  
> **Ngày tạo**: 2026-09-22

---

## 1. Kiến trúc phân tầng (Layered Architecture)

```
┌─────────────────────────────────────────────────────────────┐
│                        UI Layer                             │
│   Blazor Components (.razor pages)                          │
│   - Words/WordList.razor                                    │
│   - Words/WordFormModal.razor                               │
│   - Topics/TopicManage.razor                                │
│   - Shared/ConfirmDeleteModal.razor                         │
├─────────────────────────────────────────────────────────────┤
│                     Service Layer                           │
│   IVocabularyService  ◄──► VocabularyService                │
│   (Business Logic, Validation, Async/Await)                 │
├─────────────────────────────────────────────────────────────┤
│                     Data Layer                              │
│   AppDbContext (EF Core DbContextFactory)                   │
│   Entities: Topic.cs, Word.cs                               │
├─────────────────────────────────────────────────────────────┤
│                   Database Layer                            │
│   SQLite – LearnNN_VocabDB.db (file-based)                  │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Cấu trúc thư mục dự án

👉 **Xem chi tiết cây thư mục dự án tại bản đồ kiến trúc (Single Source of Truth):** [PROJECT_MAP.md](../PROJECT_MAP.md)

*(Để tránh việc phải cập nhật cấu trúc thư mục ở nhiều nơi, chúng ta áp dụng nguyên tắc Single Source of Truth: toàn bộ cây thư mục và quy định tổ chức file chỉ được quản lý duy nhất tại `PROJECT_MAP.md`. Các tài liệu khác chỉ cần reference đến file đó).*

---

## 3. Service Interface – IVocabularyService

```csharp
namespace LearnNN.Services;

public interface IVocabularyService
{
    // ─── TOPICS ──────────────────────────────────────────
    Task<List<Topic>> GetAllTopicsAsync();
    Task<Topic?> GetTopicByIdAsync(int id);
    Task<Topic> CreateTopicAsync(Topic topic);
    Task<Topic> UpdateTopicAsync(Topic topic);
    Task DeleteTopicAsync(int id);
    Task<bool> TopicNameExistsAsync(string name, int? excludeId = null);

    // ─── WORDS ───────────────────────────────────────────
    Task<List<Word>> GetWordsAsync(WordFilterModel filter);
    Task<int> GetWordCountAsync(WordFilterModel filter);
    Task<Word?> GetWordByIdAsync(int id);
    Task<Word> CreateWordAsync(Word word);
    Task<Word> UpdateWordAsync(Word word);
    Task DeleteWordAsync(int id);
    Task ToggleMasteredAsync(int wordId);
}
```

---

## 4. Filter Model

```csharp
namespace LearnNN.Models;

public class WordFilterModel
{
    public string? SearchTerm { get; set; }      // Tìm theo Term hoặc Meaning
    public int? TopicId { get; set; }             // Lọc theo Topic
    public MasteredFilter MasteredFilter { get; set; } = MasteredFilter.All;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public enum MasteredFilter
{
    All = 0,
    NotMastered = 1,
    Mastered = 2
}
```

---

## 5. Program.cs – Cấu hình DI

```csharp
using LearnNN.Data;
using LearnNN.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ─── BLAZOR ──────────────────────────────────────────────────
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ─── DATABASE ────────────────────────────────────────────────
// QUAN TRỌNG: Dùng AddDbContextFactory (không phải AddDbContext)
// để tránh concurrency issues trong Blazor Server
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ─── SERVICES ────────────────────────────────────────────────
builder.Services.AddScoped<IVocabularyService, VocabularyService>();

var app = builder.Build();

// ─── AUTO MIGRATE ON STARTUP ─────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    await using var db = await factory.CreateDbContextAsync();
    await db.Database.MigrateAsync();  // Tự động apply migration
}

// ─── MIDDLEWARE ───────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
```

---

## 6. appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=LearnNN_VocabDB.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## 7. Luồng dữ liệu (Data Flow)

```
User Action (Browser)
       │
       ▼
Blazor Component (.razor)
  - EventCallback triggered
  - Call IVocabularyService method
       │
       ▼
VocabularyService
  - Business logic / validation
  - Create DbContext via IDbContextFactory
       │
       ▼
AppDbContext (EF Core)
  - LINQ query
  - SQL generated & executed
       │
       ▼
SQL Server Database
  - Return data
       │
       ▼ (return path)
VocabularyService → Component → StateHasChanged() → UI re-render
```

---

## 8. Routing Map

| URL | Component | Mô tả |
|---|---|---|
| `/` | `Home.razor` | Dashboard tổng quan |
| `/words` | `WordList.razor` | Danh sách từ vựng |
| `/topics` | `TopicManage.razor` | Quản lý chủ đề |
