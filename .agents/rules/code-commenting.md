# CODE COMMENTING CONVENTION

> Based on: Clean Code, Microsoft C# Coding Conventions & XML Docs, A Philosophy of Software Design

---

## 1. Language & Formula

- **Language**: ALL comments MUST be written in **English**.
  - Exception: `ErrorMessage` in DataAnnotations and UI-facing text remain in Vietnamese.
- **5W1H Formula**: Apply flexibly — What (default), Why (when reasoning needed), When/Where (context), How (complex logic). Keep concise; not all 6 elements are required.
- **Golden Rule**: Comments answer **"Why?"**, NOT repeat **"What"** the code already says.

---

## 2. XML Documentation (`<summary>`)

### 2.1 Required Targets

| Target | Required? | Format |
|--------|-----------|--------|
| `class` / `record` / `struct` | ✅ Yes | `<summary>` describing primary responsibility (What + Why). Add a Vietnamese translation line: `/// <para>VN: [bản dịch tiếng Việt]</para>` |
| `interface` | ✅ Yes | `<summary>` describing contract/abstraction. Add VN translation |
| `public method` | ✅ Yes | `<summary>` + `<param>` + `<returns>` + `<exception>` (if throws) |
| `public property` | ✅ When not self-explanatory | `<summary>` single line |
| `[Parameter]` (Blazor) | ✅ Yes | `<summary>` single line in English |
| `private method` | ⚠️ Encouraged | `<summary>` when logic is complex; skip when method name is self-documenting |
| `enum` | ✅ Yes (for type) | `<summary>` for enum type (with VN translation); individual values only when ambiguous |
| `private field` | ❌ No | Field name must be self-documenting. Comment only for special reasons |

### 2.2 Class Summary Format (with Vietnamese Translation)

```csharp
/// <summary>
/// Manages CRUD operations for vocabulary topics and words via EF Core.
/// <para>VN: Quản lý các thao tác CRUD cho chủ đề và từ vựng thông qua EF Core.</para>
/// </summary>
public class VocabularyService : IVocabularyService { }
```

---

## 3. Inline Comments (`//`)

| Rule | Detail |
|------|--------|
| **"Why, not What"** | Only comment when code is not self-explanatory. Prioritize reasoning/context over describing code |
| **Concise** | Max 1-2 lines per comment. If more is needed → refactor code or use `<summary>` |
| **No redundancy** | Never repeat variable/method names. ❌ `// Get all topics` before `GetAllTopicsAsync()` |
| **Section dividers / Function partitioning** | In C# code, use `#region [Section Name]` and `#endregion` to partition functional groups (e.g., `#region Topics`, `#region Private Helpers`). Do NOT use `// ───` comment dividers. |

```csharp
#region Topics
public async Task<List<Topic>> GetAllTopicsAsync() { ... }
#endregion

#region Private Helpers
private static IQueryable<Word> BuildWordFilterQuery(...) { ... }
#endregion
```

---

## 4. Razor / HTML Comments (`<!-- -->`)

| Rule | Detail |
|------|--------|
| **Region markers** | Use `<!-- #region: Section Name -->` and `<!-- #endregion: Section Name -->` for major UI blocks (follows C# region pattern) |
| **Inline HTML** | Use sparingly — only to explain layout/CSS decisions, NOT to label every `<div>` |

### Example

```html
<!-- #region: Filter & Search Toolbar -->
<div class="card ...">
    ...
</div>
<!-- #endregion: Filter & Search Toolbar -->
```

---

## 5. AI-Generated Code Comments (Numbered Steps)

| Rule | Detail |
|------|--------|
| **When to use** | When AI writes code in a "goal → step-by-step" approach |
| **Format** | **A, B, C...** for top-level steps; **I, II, III...** for sub-steps |
| **Placement** | Before the corresponding code block, with concise English description |
| **Preservation** | Do NOT remove numbered step comments during refactoring — they document intent |

```csharp
// A. Validate input before processing
if (string.IsNullOrWhiteSpace(name)) { ... }

// B. Check for duplicate name (excluding current entity during edit)
bool exists = await Service.NameExistsAsync(name, excludeId);

// C. Persist changes
//    I.  Create new entity with timestamps
//    II. Save to database and reload navigation properties
```

---

## 6. Anti-Patterns (What NOT to Comment)

| ❌ Anti-pattern | Reason |
|----------------|--------|
| `// Constructor` | Code is obvious |
| `// Set the name` before `Name = name;` | Repeats code |
| `// TODO:` without owner/date | Creates hidden tech debt |
| Outdated comments that contradict code | Worse than no comment (Clean Code) |
| Block comments `/* ... */` for explanations | Use XML Docs or inline `//` |
