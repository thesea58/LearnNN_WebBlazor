# 🗄️ LearnNN – Database Design Document
## Thiết kế Cơ sở Dữ liệu (Database Design)

> **Phiên bản**: 1.0  
> **Ngày tạo**: 2026-09-22  
> **Database**: SQLite (file-based, không cần cài server)  
> **ORM**: EF Core 9 – Code First

---

## 1. Entity Relationship Diagram (ERD)

```
┌───────────────────────────────┐         ┌───────────────────────────────────────────┐
│           Topic               │         │                   Word                    │
├───────────────────────────────┤         ├───────────────────────────────────────────┤
│ PK  Id           INT IDENTITY │◄────────│ PK  Id               INT IDENTITY        │
│     Name         NVARCHAR(100)│  1    N │ FK  TopicId           INT NOT NULL        │
│     Description  NVARCHAR(250)│         │     Term              NVARCHAR(100)       │
│     CreatedAt    DATETIME2    │         │     Phonetic          NVARCHAR(100)       │
│     UpdatedAt    DATETIME2    │         │     PartOfSpeech      NVARCHAR(50)        │
└───────────────────────────────┘         │     Meaning           NVARCHAR(500)       │
                                          │     ExampleSentence   NVARCHAR(500)       │
                                          │     ExampleTranslation NVARCHAR(500)      │
                                          │     IsMastered        BIT DEFAULT 0       │
                                          │     CreatedAt         DATETIME2           │
                                          │     UpdatedAt         DATETIME2           │
                                          └───────────────────────────────────────────┘
```

**Quan hệ**: `Topic` (1) ←→ (N) `Word`  
- Một Topic có nhiều Word  
- Một Word thuộc đúng một Topic  
- Cascade Delete: Xóa Topic sẽ xóa tất cả Word của Topic đó

---

## 2. Mô tả chi tiết bảng

### 2.1 Bảng `Topics`

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| `Id` | INT | PK, IDENTITY(1,1), NOT NULL | Khóa chính tự tăng |
| `Name` | NVARCHAR(100) | NOT NULL, UNIQUE | Tên chủ đề (ví dụ: IELTS, Giao tiếp) |
| `Description` | NVARCHAR(250) | NULL | Mô tả ngắn về chủ đề |
| `CreatedAt` | DATETIME2(7) | NOT NULL, DEFAULT GETUTCDATE() | Thời điểm tạo (UTC) |
| `UpdatedAt` | DATETIME2(7) | NULL | Thời điểm cập nhật cuối (UTC) |

**Index**:
- `IX_Topics_Name` – UNIQUE INDEX trên cột `Name`

---

### 2.2 Bảng `Words`

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| `Id` | INT | PK, IDENTITY(1,1), NOT NULL | Khóa chính tự tăng |
| `TopicId` | INT | FK → Topics(Id), NOT NULL | Khóa ngoại liên kết Topic |
| `Term` | NVARCHAR(100) | NOT NULL | Từ hoặc cụm từ tiếng Anh |
| `Phonetic` | NVARCHAR(100) | NULL | Phiên âm IPA, ví dụ: /ˈæp.əl/ |
| `PartOfSpeech` | NVARCHAR(50) | NULL | Từ loại: noun, verb, adj, adv, phrase... |
| `Meaning` | NVARCHAR(500) | NOT NULL | Nghĩa tiếng Việt / định nghĩa |
| `ExampleSentence` | NVARCHAR(500) | NULL | Câu ví dụ bằng tiếng Anh |
| `ExampleTranslation` | NVARCHAR(500) | NULL | Dịch câu ví dụ sang tiếng Việt |
| `IsMastered` | BIT | NOT NULL, DEFAULT 0 | false = Chưa thuộc / true = Đã thuộc |
| `CreatedAt` | DATETIME2(7) | NOT NULL, DEFAULT GETUTCDATE() | Thời điểm tạo (UTC) |
| `UpdatedAt` | DATETIME2(7) | NULL | Thời điểm cập nhật cuối (UTC) |

**Index**:
- `IX_Words_TopicId` – Tìm kiếm Word theo Topic (tự động tạo bởi EF Core)
- `IX_Words_Term` – Tìm kiếm nhanh theo từ
- `IX_Words_IsMastered` – Lọc nhanh theo trạng thái học

---

## 3. DDL Script (SQL Server)

```sql
-- ============================================================
-- LearnNN Vocabulary App – Database Schema
-- Target: SQLite 3.x (file-based)
-- Generated: 2026-09-22
-- ============================================================

-- ============================================================
-- TABLE: Topics
-- ============================================================
CREATE TABLE IF NOT EXISTS [Topics] (
    [Id]          INTEGER      PRIMARY KEY AUTOINCREMENT,
    [Name]        TEXT         NOT NULL,
    [Description] TEXT,
    [CreatedAt]   TEXT         NOT NULL  DEFAULT (datetime('now')),
    [UpdatedAt]   TEXT
);

-- Unique index on Topic Name
CREATE UNIQUE INDEX IF NOT EXISTS [IX_Topics_Name]
    ON [Topics] ([Name]);

-- ============================================================
-- TABLE: Words
-- ============================================================
CREATE TABLE IF NOT EXISTS [Words] (
    [Id]                 INTEGER      PRIMARY KEY AUTOINCREMENT,
    [TopicId]            INTEGER      NOT NULL,
    [Term]               TEXT         NOT NULL,
    [Phonetic]           TEXT,
    [PartOfSpeech]       TEXT,
    [Meaning]            TEXT         NOT NULL,
    [ExampleSentence]    TEXT,
    [ExampleTranslation] TEXT,
    [IsMastered]         INTEGER      NOT NULL  DEFAULT 0,
    [CreatedAt]          TEXT         NOT NULL  DEFAULT (datetime('now')),
    [UpdatedAt]          TEXT,

    FOREIGN KEY ([TopicId]) REFERENCES [Topics] ([Id])
        ON DELETE CASCADE
        ON UPDATE NO ACTION
);

-- Indexes on Words
CREATE INDEX IF NOT EXISTS [IX_Words_TopicId]
    ON [Words] ([TopicId]);

CREATE INDEX IF NOT EXISTS [IX_Words_Term]
    ON [Words] ([Term]);

CREATE INDEX IF NOT EXISTS [IX_Words_IsMastered]
    ON [Words] ([IsMastered]);
```

---

## 4. Seed Data (Dữ liệu mẫu)

```sql
-- ============================================================
-- SEED DATA (SQLite)
-- ============================================================

-- Topics
INSERT INTO [Topics] ([Name], [Description], [CreatedAt]) VALUES
('Công nghệ',      'Từ vựng về công nghệ thông tin, lập trình, phần mềm', datetime('now')),
('Giao tiếp',      'Từ vựng giao tiếp hàng ngày, xã giao, văn phòng',    datetime('now')),
('IELTS Academic', 'Từ vựng học thuật dùng trong kỳ thi IELTS',           datetime('now'));

-- Words – Topic: Công nghệ (Id=1)
INSERT INTO [Words]
    ([TopicId],[Term],[Phonetic],[PartOfSpeech],[Meaning],[ExampleSentence],[ExampleTranslation],[IsMastered],[CreatedAt])
VALUES
(1, 'algorithm',   '/ˈæl.ɡə.rɪ.ðəm/', 'noun',
    'Thuật toán – tập hợp các bước xử lý để giải quyết bài toán',
    'The sorting algorithm runs in O(n log n) time.',
    'Thuật toán sắp xếp này chạy trong thời gian O(n log n).',
    0, datetime('now')),

(1, 'framework',   '/ˈfreɪm.wɜːk/', 'noun',
    'Khung phần mềm – bộ thư viện/công cụ hỗ trợ xây dựng ứng dụng',
    '.NET is a powerful framework for building web applications.',
    '.NET là một framework mạnh mẽ để xây dựng ứng dụng web.',
    1, datetime('now')),

(1, 'repository',  '/rɪˈpɒz.ɪ.tər.i/', 'noun',
    'Kho lưu trữ mã nguồn',
    'Please push your code to the Git repository.',
    'Hãy đẩy code của bạn lên kho Git.',
    0, datetime('now')),

-- Words – Topic: Giao tiếp (Id=2)
(2, 'apologize',   '/əˈpɒl.ə.dʒaɪz/', 'verb',
    'Xin lỗi, tạ lỗi',
    'I apologize for the delay in responding.',
    'Tôi xin lỗi vì đã trả lời trễ.',
    0, datetime('now')),

(2, 'clarify',     '/ˈklær.ɪ.faɪ/', 'verb',
    'Làm rõ, giải thích rõ hơn',
    'Could you clarify what you mean by that?',
    'Bạn có thể làm rõ ý bạn muốn nói không?',
    1, datetime('now')),

-- Words – Topic: IELTS Academic (Id=3)
(3, 'substantial', '/səbˈstæn.ʃəl/', 'adjective',
    'Đáng kể, quan trọng, lớn về quy mô',
    'There has been a substantial increase in online learning.',
    'Đã có sự gia tăng đáng kể trong việc học trực tuyến.',
    0, datetime('now')),

(3, 'nonetheless', '/ˌnʌn.ðəˈles/', 'adverb',
    'Tuy nhiên, dẫu vậy, mặc dù thế',
    'The task was difficult; nonetheless, she completed it.',
    'Nhiệm vụ rất khó; dẫu vậy, cô ấy đã hoàn thành nó.',
    0, datetime('now'));
```

---

## 5. EF Core Entity Classes (C# Code-First)

### Topic.cs

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNN.Data.Entities;

[Table("Topics")]
public class Topic
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên chủ đề không được để trống")]
    [MaxLength(100, ErrorMessage = "Tên chủ đề tối đa 100 ký tự")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public ICollection<Word> Words { get; set; } = new List<Word>();
}
```

### Word.cs

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnNN.Data.Entities;

[Table("Words")]
public class Word
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Topic))]
    public int TopicId { get; set; }

    [Required(ErrorMessage = "Từ vựng không được để trống")]
    [MaxLength(100, ErrorMessage = "Từ/cụm từ tối đa 100 ký tự")]
    public string Term { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Phonetic { get; set; }

    [MaxLength(50)]
    public string? PartOfSpeech { get; set; }

    [Required(ErrorMessage = "Nghĩa của từ không được để trống")]
    [MaxLength(500)]
    public string Meaning { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ExampleSentence { get; set; }

    [MaxLength(500)]
    public string? ExampleTranslation { get; set; }

    public bool IsMastered { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public Topic Topic { get; set; } = null!;
}
```

### AppDbContext.cs

```csharp
using LearnNN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnNN.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Word>  Words  => Set<Word>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Topic configuration
        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasIndex(t => t.Name).IsUnique();
            entity.Property(t => t.CreatedAt).HasDefaultValueSql("datetime('now')");
        });

        // Word configuration
        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasIndex(w => w.TopicId);
            entity.HasIndex(w => w.Term);
            entity.HasIndex(w => w.IsMastered);
            entity.Property(w => w.IsMastered).HasDefaultValue(false);
            entity.Property(w => w.CreatedAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(w => w.Topic)
                  .WithMany(t => t.Words)
                  .HasForeignKey(w => w.TopicId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed Data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Topic>().HasData(
            new Topic { Id = 1, Name = "Công nghệ",      Description = "Từ vựng về công nghệ thông tin, lập trình" },
            new Topic { Id = 2, Name = "Giao tiếp",      Description = "Từ vựng giao tiếp hàng ngày, văn phòng"    },
            new Topic { Id = 3, Name = "IELTS Academic", Description = "Từ vựng học thuật dùng trong kỳ thi IELTS"  }
        );

        modelBuilder.Entity<Word>().HasData(
            new Word { Id=1, TopicId=1, Term="algorithm",   Phonetic="/ˈæl.ɡə.rɪ.ðəm/", PartOfSpeech="noun",      Meaning="Thuật toán",              ExampleSentence="The sorting algorithm runs in O(n log n) time.", IsMastered=false },
            new Word { Id=2, TopicId=1, Term="framework",   Phonetic="/ˈfreɪm.wɜːk/",   PartOfSpeech="noun",      Meaning="Khung phần mềm",           ExampleSentence=".NET is a powerful framework.",                  IsMastered=true  },
            new Word { Id=3, TopicId=1, Term="repository",  Phonetic="/rɪˈpɒz.ɪ.tər.i/",PartOfSpeech="noun",      Meaning="Kho lưu trữ mã nguồn",     ExampleSentence="Push your code to the Git repository.",          IsMastered=false },
            new Word { Id=4, TopicId=2, Term="apologize",   Phonetic="/əˈpɒl.ə.dʒaɪz/", PartOfSpeech="verb",      Meaning="Xin lỗi, tạ lỗi",         ExampleSentence="I apologize for the delay.",                     IsMastered=false },
            new Word { Id=5, TopicId=2, Term="clarify",     Phonetic="/ˈklær.ɪ.faɪ/",   PartOfSpeech="verb",      Meaning="Làm rõ, giải thích rõ hơn",ExampleSentence="Could you clarify what you mean?",               IsMastered=true  },
            new Word { Id=6, TopicId=3, Term="substantial", Phonetic="/səbˈstæn.ʃəl/",  PartOfSpeech="adjective", Meaning="Đáng kể, quan trọng",      ExampleSentence="There has been a substantial increase.",         IsMastered=false },
            new Word { Id=7, TopicId=3, Term="nonetheless", Phonetic="/ˌnʌn.ðəˈles/",   PartOfSpeech="adverb",    Meaning="Tuy nhiên, dẫu vậy",       ExampleSentence="The task was difficult; nonetheless, she completed it.", IsMastered=false }
        );
    }
}
```

---

## 6. Lưu ý quan trọng về Database

> [!WARNING]
> **Blazor Server + DbContext**: Sử dụng `AddDbContextFactory<AppDbContext>` thay vì `AddDbContext` để tránh lỗi concurrency trong Blazor Server. Mỗi operation tạo scope riêng.

> [!NOTE]
> **Cascade Delete**: Đã cấu hình `ON DELETE CASCADE` – xóa Topic sẽ xóa toàn bộ Word liên quan. Cân nhắc hiển thị cảnh báo trước khi xóa Topic có Word.

> [!NOTE]
> **SQLite Limitations**: SQLite dùng `TEXT` cho datetime (không có kiểu DATETIME2). EF Core tự xử lý mapping. Khi scale lên production, cân nhắc chuyển sang PostgreSQL hoặc SQL Server.

> [!TIP]
> **DateTime UTC**: Tất cả datetime lưu theo UTC. Convert sang local time (UTC+7) khi hiển thị trên UI để đúng múi giờ Việt Nam.
