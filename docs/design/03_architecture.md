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

```
LearnNN_WebBlazor/
├── 📄 LearnNN_WebBlazor.csproj          # Project file (.NET 10)
├── 📄 Program.cs                         # DI registration, middleware
├── 📄 appsettings.json                   # Connection string, logging
├── 📄 appsettings.Development.json
│
├── 📁 Data/
│   ├── 📄 AppDbContext.cs               # DbContext + Fluent API + Seed
│   └── 📁 Entities/
│       ├── 📄 Topic.cs                  # Topic entity + DataAnnotations
│       └── 📄 Word.cs                   # Word entity + DataAnnotations
│
├── 📁 Services/
│   ├── 📄 IVocabularyService.cs         # Interface (contract)
│   └── 📄 VocabularyService.cs          # Implementation
│
├── 📁 Models/                           # ViewModels / DTOs (tách entity khỏi UI)
│   ├── 📄 WordFilterModel.cs            # Filter state cho WordList
│   └── 📄 WordFormModel.cs              # Form model cho Add/Edit
│
├── 📁 Components/
│   ├── 📄 App.razor
│   ├── 📄 Routes.razor
│   ├── 📁 Layout/
│   │   ├── 📄 MainLayout.razor
│   │   └── 📄 NavMenu.razor
│   ├── 📁 Pages/
│   │   ├── 📁 Words/
│   │   │   ├── 📄 WordList.razor        # Danh sách + filter + search
│   │   │   └── 📄 WordFormModal.razor   # Form thêm/sửa từ (modal)
│   │   ├── 📁 Topics/
│   │   │   └── 📄 TopicManage.razor     # CRUD Topics
│   │   └── 📄 Home.razor               # Dashboard / trang chủ
│   └── 📁 Shared/
│       ├── 📄 ConfirmDeleteModal.razor  # Modal xác nhận xóa
│       ├── 📄 LoadingSpinner.razor      # Loading indicator
│       └── 📄 AlertMessage.razor        # Toast/Alert notification
│
├── 📁 Migrations/                       # EF Core migration files (auto-generated)
│
└── 📁 wwwroot/
    ├── 📄 app.css                       # Custom styles
    └── 📁 lib/bootstrap/                # Bootstrap 5
```

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
