# 🗺️ PROJECT_MAP – Bản đồ Kiến trúc Dự án LearnNN_WebBlazor

> **Phiên bản**: 1.0
> **Ngày khởi tạo**: 2026-09-22
> **Kiến trúc**: Layered Architecture (3-Tier) trên nền Blazor Server (.NET 10)

---

## Mục 1: 🌳 Cây thư mục hiện tại (Directory Tree)

> Cập nhật lần cuối: 2026-09-24

```
LearnNN_WebBlazor/                          # 🏠 Repository root
│
├── 📁 .agents/                             # ⚙️ Cấu hình AI Agent (rules, skills)
│   └── 📁 rules/                           #    Các quy tắc hành vi cho AI
│       ├── 📄 code-commenting.md           #    Quy ước comment code (EN, 5W1H, XML Docs)
│       ├── 📄 project-architecture.md      #    Quy tắc kiến trúc & file conventions
│       └── 📄 update-status-task.md
│
├── 📁 .github/                             # 🚀 GitHub Workflows & CI/CD
│   └── 📁 workflows/
│       └── 📄 dotnet.yml                   #    Workflow build, test & publish Blazor Web App
│
├── 📁 CrawData/                            # 📦 Dữ liệu thô & script xử lý import (JSON, CSV, Python)
│   ├── 📄 template_importData.csv          #    File mẫu CSV import dữ liệu cho web
│   ├── 📄 toeic_600_essential_words_en_vi.json # Dữ liệu 600 từ vựng TOEIC (Anh - Việt)
│   ├── 📄 convert_json_to_csv.py           #    Script Python chuyển đổi JSON sang CSV import
│   └── 📄 toeic_600_words_import.csv       #    File kết quả CSV sẵn sàng import vào Web
│
├── 📁 docs/                                # 📚 Tài liệu dự án
│   ├── 📄 PROJECT_MAP.md                   # ★ BẢN ĐỒ KIẾN TRÚC (file này)
│   ├── 📁 design/                          #    Tài liệu thiết kế hệ thống
│   │   ├── 📄 00_history.md                #    Lịch sử thay đổi thiết kế
│   │   ├── 📄 01_project_overview.md       #    Tổng quan dự án
│   │   ├── 📄 02_database_design.md        #    Thiết kế database
│   │   ├── 📄 03_architecture.md           #    Kiến trúc phân tầng
│   │   └── 📄 database_design.xml          #    Diagram database (XML)
│   └── 📁 task/                            #    Quản lý tiến độ công việc
│       ├── 📄 README.md                    #    Bảng tổng quan task
│       ├── 📁 01_project_setup/            #    ✅ Task 01 – Project Setup
│       ├── 📁 02_database_entities/        #    ✅ Task 02 – Database & Entities
│       ├── 📁 03_service_layer/            #    ✅ Task 03 – Service Layer
│       ├── 📁 04_layout_navigation/        #    ✅ Task 04 – Layout & Navigation
│       ├── 📁 05_topic_management/         #    ⬜ Task 05 – Topic Management
│       ├── 📁 06_word_list/                #    ⬜ Task 06 – Word List
│       ├── 📁 07_word_form_modal/          #    ⬜ Task 07 – Word Form Modal
│       ├── 📁 08_shared_components/        #    ✅ Task 08 – Shared Components
│       ├── 📁 09_integration_testing/      #    ✅ Task 09 – Integration & Testing
│       ├── 📁 12_import_export_data/       #    ✅ Task 12 – Import/Export Data
│       └── 📁 13_vocabulary_study_hub/     #    ✅ Task 13 – Vocabulary Study Hub, Games & Quizzes
│
├── 📁 Source/                              # 💻 MÃ NGUỒN CHÍNH (Blazor Web App)
│   ├── 📄 LearnNN_WebBlazor.csproj         #    Project file (.NET 10)
│   ├── 📄 Program.cs                       #    Entry point – DI, Middleware, Pipeline
│   ├── 📄 appsettings.json                 #    Cấu hình chính (ConnectionString, Logging)
│   ├── 📄 appsettings.Development.json     #    Cấu hình môi trường Development
│   │
│   ├── 📁 Data/                            # 🗄️ Data Access Layer
│   │   ├── 📄 AppDbContext.cs              #    DbContext + Fluent API + Seed Data
│   │   └── 📁 Entities/                    #    Database Entity classes
│   │       ├── 📄 Topic.cs                 #    Entity: Chủ đề từ vựng
│   │       └── 📄 Word.cs                  #    Entity: Từ vựng
│   │
│   ├── 📁 Models/                          # 📦 ViewModels / DTOs
│   │   ├── 📄 AlertType.cs                 #    Enum: Kiểu thông báo (Success, Warning, Error, Info)
│   │   ├── 📄 WordFilterModel.cs           #    Filter model cho Word (search, paging)
│   │   ├── 📄 WordCsvRecord.cs             #    Model ánh xạ import/export CSV
│   │   └── 📁 Study/                       #    Models & DTOs cho phân hệ học tập & games
│   │       ├── 📄 StudySessionOptions.cs   #    Tùy chọn cấu hình phiên học (Topic, Filter, Count)
│   │       ├── 📄 QuizQuestionDto.cs       #    DTO câu hỏi trắc nghiệm 4 đáp án
│   │       ├── 📄 MatchCardDto.cs          #    DTO thẻ bài ghép đôi (Word Match)
│   │       └── 📄 QuizResultDto.cs         #    DTO kết quả & đánh giá bài kiểm tra
│   │
│   ├── 📁 Services/                        # ⚡ Business Logic Layer
│   │   ├── 📄 IVocabularyService.cs        #    Interface – Contract cho vocabulary CRUD
│   │   ├── 📄 VocabularyService.cs         #    Implementation – Logic xử lý business
│   │   ├── 📄 IStudyService.cs             #    Interface – Hoạt động học từ, sinh game & quiz
│   │   └── 📄 StudyService.cs              #    Implementation – Xáo trộn, sinh distractors, cập nhật tiến độ
│   │
│   ├── 📁 Components/                      # 🎨 UI Layer (Blazor Components)
│   │   ├── 📄 App.razor                    #    Root component (HTML shell)
│   │   ├── 📄 Routes.razor                 #    Router configuration
│   │   ├── 📄 _Imports.razor               #    Global using directives
│   │   ├── 📁 Layout/                      #    Layout components
│   │   │   ├── 📄 MainLayout.razor         #    Layout chính của ứng dụng
│   │   │   ├── 📄 MainLayout.razor.css     #    Scoped CSS cho MainLayout
│   │   │   ├── 📄 NavMenu.razor            #    Thanh điều hướng (Home, Study, Words, Topics)
│   │   │   ├── 📄 NavMenu.razor.css        #    Scoped CSS cho NavMenu
│   │   │   ├── 📄 ReconnectModal.razor     #    Modal reconnect (Blazor Server)
│   │   │   ├── 📄 ReconnectModal.razor.css #    Scoped CSS cho ReconnectModal
│   │   │   └── 📄 ReconnectModal.razor.js  #    JS interop cho ReconnectModal
│   │   ├── 📁 Pages/                       #    Routable page components
│   │   │   ├── 📄 Home.razor               #    Trang chủ / Dashboard
│   │   │   ├── 📄 Counter.razor            #    Demo counter (template mặc định)
│   │   │   ├── 📄 Weather.razor            #    Demo weather (template mặc định)
│   │   │   ├── 📄 Error.razor              #    Trang lỗi
│   │   │   ├── 📄 NotFound.razor           #    Trang 404
│   │   │   ├── 📁 Study/                   #    Pages phân hệ học từ vựng & games
│   │   │   │   ├── 📄 StudyHub.razor       #    Trang Hub trung tâm chọn chủ đề & chế độ học (/study)
│   │   │   │   ├── 📄 FlashcardStudy.razor #    Trang học Flashcard 3D lật thẻ + TTS (/study/flashcards)
│   │   │   │   ├── 📄 WordMatchGame.razor  #    Game nối từ tiếng Anh - Việt ghép đôi (/study/match)
│   │   │   │   ├── 📄 WordScrambleGame.razor #  Game sắp xếp chữ cái tiếng Anh (/study/scramble)
│   │   │   │   └── 📄 QuizPractice.razor   #    Trắc nghiệm 4 đáp án & tổng kết kết quả (/study/quiz)
│   │   │   ├── 📁 Topics/                  #    Pages quản lý chủ đề
│   │   │   │   └── 📄 TopicManage.razor    #    Trang quản lý chủ đề (CRUD, validation, xóa cascade)
│   │   │   └── 📁 Words/                   #    Pages quản lý từ vựng
│   │   │       ├── 📄 WordList.razor       #    Trang danh sách từ vựng (bảng, tìm kiếm, lọc, phân trang, toggle)
│   │   │       └── 📄 WordFormModal.razor  #    Modal form thêm/sửa từ vựng với validation
│   │   └── 📁 Shared/                      #    Shared/reusable UI components
│   │       ├── 📄 AlertMessage.razor       #    Component thông báo kết quả (auto-dismiss)
│   │       ├── 📄 ConfirmDeleteModal.razor #    Modal xác nhận xóa tái sử dụng cho các trang
│   │       └── 📄 LoadingSpinner.razor     #    Component loading spinner tái sử dụng
│   │
│   ├── 📁 Migrations/                      # 🔄 EF Core Migration files (auto-generated)
│   │   ├── 📄 20260922..._InitialCreate.Designer.cs
│   │   ├── 📄 20260922..._InitialCreate.cs
│   │   └── 📄 AppDbContextModelSnapshot.cs
│   │
│   ├── 📁 Properties/                      # ⚙️ Project properties
│   │   └── 📄 launchSettings.json          #    Cấu hình launch (ports, profiles)
│   │
│   └── 📁 wwwroot/                         # 🌐 Static files (served publicly)
│       ├── 📄 app.css                      #    Custom CSS styles
│       ├── 📄 study.css                    #    CSS 3D flip card, matching game, scramble, quiz
│       ├── 📄 study.js                     #    JS Interop phát âm Web Speech API & audio effects
│       ├── 📄 favicon.png                  #    Favicon
│       └── 📁 lib/                         #    Third-party client libraries
│           └── 📁 bootstrap/              #    Bootstrap 5
│
├── 📄 LearnNN_WebBlazor.slnx               # Solution file định dạng XML (.NET 10 / Visual Studio 2022+)
├── 📄 README.md                            # Tài liệu tổng quan dự án & hướng dẫn chạy
├── 📄 .gitignore                           # Git ignore rules
└── 📁 .git/                                # Git repository data
```

---

## Mục 2: 📋 Bảng đặc tả Trách nhiệm (Directory Specification)

### 2.1 Thư mục gốc Repository

| Thư mục | Trách nhiệm (Single Responsibility) | ✅ ĐƯỢC để ở đây | ❌ TUYỆT ĐỐI KHÔNG để ở đây |
|---------|--------------------------------------|-------------------|------------------------------|
| `/` (root) | Chứa cấu hình repository-level | `.gitignore`, `README.md`, `LICENSE`, `.editorconfig`, `LearnNN_WebBlazor.slnx` | Source code, thư viện, tài liệu chi tiết |
| `.agents/` | Cấu hình AI Agent | Rules (.md), Skills, Plugins | Source code, tài liệu dự án |
| `docs/` | Tài liệu dự án | Tài liệu thiết kế, task management, `PROJECT_MAP.md` | Source code, file cấu hình ứng dụng |
| `Source/` | Mã nguồn ứng dụng Blazor | Toàn bộ C#/.razor source code | Tài liệu, scripts DevOps, file không liên quan |

### 2.2 Thư mục `docs/`

| Thư mục | Trách nhiệm | ✅ ĐƯỢC để ở đây | ❌ TUYỆT ĐỐI KHÔNG để ở đây |
|---------|-------------|-------------------|------------------------------|
| `docs/design/` | Tài liệu thiết kế hệ thống | Spec kiến trúc, DB design, diagram, wireframe | Task files, meeting notes, changelog code |
| `docs/task/` | Quản lý tiến độ task | Task folders (đánh số `XX_snake_case/`), `README.md` tổng quan | Design docs, source code |

### 2.3 Thư mục `Source/` – Data Access Layer

| Thư mục | Trách nhiệm | ✅ ĐƯỢC để ở đây | ❌ TUYỆT ĐỐI KHÔNG để ở đây |
|---------|-------------|-------------------|------------------------------|
| `Source/Data/` | Data Access – DbContext, cấu hình EF Core | `AppDbContext.cs`, cấu hình Fluent API | Entity classes (→ để vào `Entities/`), Services, ViewModels |
| `Source/Data/Entities/` | Database Entity classes (POCO) | Entity classes map trực tiếp tới bảng DB (`Topic.cs`, `Word.cs`) | DTOs, ViewModels, Business logic, Repositories |
| `Source/Migrations/` | EF Core migrations (auto-generated) | Migration files từ `dotnet ef migrations add` | File viết tay, seed data scripts |

### 2.4 Thư mục `Source/` – Business Logic Layer

| Thư mục | Trách nhiệm | ✅ ĐƯỢC để ở đây | ❌ TUYỆT ĐỐI KHÔNG để ở đây |
|---------|-------------|-------------------|------------------------------|
| `Source/Services/` | Business logic, xử lý nghiệp vụ | Interface (`I*Service.cs`), Implementation (`*Service.cs`) | Entity classes, Razor components, Controllers, DbContext |
| `Source/Models/` | ViewModels, DTOs, Form models | `*Model.cs`, `*Dto.cs`, `*ViewModel.cs`, Enums liên quan | Entity classes (→ `Data/Entities/`), Business logic |

### 2.5 Thư mục `Source/` – UI Layer (Blazor Components)

| Thư mục | Trách nhiệm | ✅ ĐƯỢC để ở đây | ❌ TUYỆT ĐỐI KHÔNG để ở đây |
|---------|-------------|-------------------|------------------------------|
| `Source/Components/` | Root Blazor components | `App.razor`, `Routes.razor`, `_Imports.razor` | Page components, layout files |
| `Source/Components/Layout/` | Layout & navigation | `MainLayout.razor`, `NavMenu.razor`, scoped CSS/JS | Page components, shared components |
| `Source/Components/Pages/` | Routable page components | `Home.razor`, `Error.razor`, thư mục feature (`Topics/`, `Words/`) | Shared components, layout, services |
| `Source/Components/Pages/Topics/` | Trang quản lý chủ đề | `TopicManage.razor` và scoped CSS/JS liên quan | Components dùng chung, Word pages |
| `Source/Components/Pages/Words/` | Trang quản lý từ vựng | `WordList.razor`, `WordFormModal.razor` và scoped CSS/JS | Components dùng chung, Topic pages |
| `Source/Components/Shared/` | Shared/Reusable UI components | `ConfirmDeleteModal.razor`, `LoadingSpinner.razor`, `AlertMessage.razor` | Page-specific components, layout, services |

### 2.6 Thư mục `Source/` – Static & Config

| Thư mục | Trách nhiệm | ✅ ĐƯỢC để ở đây | ❌ TUYỆT ĐỐI KHÔNG để ở đây |
|---------|-------------|-------------------|------------------------------|
| `Source/wwwroot/` | Static files served publicly | CSS, images, favicon, client-side JS | C# code, Razor files, server-side logic |
| `Source/wwwroot/lib/` | Third-party client libraries | Bootstrap, jQuery, các thư viện CSS/JS bên thứ 3 | Custom code, ảnh dự án |
| `Source/Properties/` | Project launch/build properties | `launchSettings.json` | Source code, cấu hình ứng dụng |

---

## Mục 3: 📏 Quy ước đặt tên & Tổ chức file (Conventions)

### 3.1 Quy tắc đặt tên

| Đối tượng | Convention | Ví dụ | Ghi chú |
|-----------|-----------|-------|---------|
| **Thư mục Source code** | `PascalCase` | `Components/`, `Services/`, `Data/` | Theo chuẩn .NET |
| **Thư mục docs/task** | `snake_case` có số thứ tự | `01_project_setup/`, `02_database_entities/` | Prefix `XX_` để sắp xếp |
| **File C# (.cs)** | `PascalCase` | `VocabularyService.cs`, `AppDbContext.cs` | 1 class = 1 file, tên file = tên class |
| **File Razor (.razor)** | `PascalCase` | `WordList.razor`, `MainLayout.razor` | Tên file = tên component |
| **File CSS scoped** | `PascalCase` + `.razor.css` | `MainLayout.razor.css` | Phải khớp tên với file `.razor` tương ứng |
| **File JS interop** | `PascalCase` + `.razor.js` | `ReconnectModal.razor.js` | Phải khớp tên với file `.razor` tương ứng |
| **File CSS global** | `kebab-case` hoặc `camelCase` | `app.css` | Đặt trong `wwwroot/` |
| **File tài liệu** | `snake_case` hoặc `XX_snake_case` | `01_project_overview.md` | Prefix số thứ tự cho docs có trình tự |
| **Interface C#** | `I` + `PascalCase` | `IVocabularyService` | Prefix `I` theo chuẩn .NET |
| **Entity class** | `PascalCase` (danh từ số ít) | `Topic`, `Word` | Không dùng suffix `Entity` |
| **Model/DTO class** | `PascalCase` + `Model`/`Dto` | `WordFilterModel`, `WordFormModel` | Phải có suffix rõ ràng |
| **Namespace** | Theo cấu trúc thư mục | `LearnNN_WebBlazor.Data.Entities` | Root namespace: `LearnNN_WebBlazor` |

### 3.2 Tiêu chí phân loại file

| Câu hỏi quyết định | CÓ → Đặt ở | KHÔNG → Tiếp tục |
|---------------------|-------------|-------------------|
| File là entity ánh xạ trực tiếp tới bảng DB? | `Source/Data/Entities/` | ↓ |
| File là DbContext hoặc cấu hình EF Core? | `Source/Data/` | ↓ |
| File là ViewModel, DTO, hoặc Filter model? | `Source/Models/` | ↓ |
| File là interface hoặc implementation service? | `Source/Services/` | ↓ |
| File là trang có route (`@page`)? | `Source/Components/Pages/<Feature>/` | ↓ |
| File là component dùng chung (≥2 pages sử dụng)? | `Source/Components/Shared/` | ↓ |
| File là layout hoặc navigation? | `Source/Components/Layout/` | ↓ |
| File là CSS/JS/image tĩnh? | `Source/wwwroot/` | ↓ |
| File là tài liệu thiết kế? | `docs/design/` | ↓ |
| File là tài liệu task? | `docs/task/XX_feature_name/` | ↓ |
| Không khớp bất kỳ mục nào? | **DỪNG LẠI → Đề xuất vị trí mới & ghi vào Change Log** | — |

### 3.3 Quy tắc tổ chức Feature

- **Feature nhỏ** (1-2 file): Đặt trực tiếp trong thư mục layer tương ứng (`Services/`, `Models/`)
- **Feature phức tạp** (≥3 file cùng domain): Tạo subfolder trong `Pages/` (ví dụ: `Pages/Words/`, `Pages/Topics/`)
- **Component dùng chung**: Chỉ khi ≥2 pages sử dụng → chuyển vào `Components/Shared/`
- **Khi thêm Entity mới**: Phải tạo file trong `Data/Entities/`, cập nhật `AppDbContext.cs`, tạo migration

---

## Mục 4: 📝 Nhật ký Kiến trúc AI (AI Change Log)

> Mọi AI Agent sau khi hoàn thành task có tạo file mới hoặc thay đổi cấu trúc **BẮT BUỘC** phải thêm 1 dòng vào bảng dưới đây.

| Ngày | Task / Yêu cầu | Files/Folders tạo mới hoặc sửa đổi | Lý do & Vị trí đặt |
|------|----------------|-------------------------------------|---------------------|
| 2026-09-22 | Task 01 – Project Setup | `Source/LearnNN_WebBlazor.csproj`, `Source/Program.cs`, `Source/appsettings.json`, `Source/appsettings.Development.json` | Khởi tạo project Blazor Web App .NET 10 với EF Core SQLite |
| 2026-09-22 | Task 02 – Database & Entities | `Source/Data/AppDbContext.cs`, `Source/Data/Entities/Topic.cs`, `Source/Data/Entities/Word.cs`, `Source/Migrations/` | Tạo entity classes, DbContext với Fluent API, Seed Data, và Initial Migration |
| 2026-09-22 | Task 03 – Service Layer | `Source/Services/IVocabularyService.cs`, `Source/Services/VocabularyService.cs`, `Source/Models/WordFilterModel.cs` | Tạo Business Logic Layer với CRUD Topics/Words, filter, paging |
| 2026-09-22 | Khởi tạo PROJECT_MAP.md | `docs/PROJECT_MAP.md` | Tạo bản đồ kiến trúc chuẩn mực cho dự án, làm tài liệu tham chiếu bắt buộc cho mọi AI Agent |
| 2026-09-22 | Tinh gọn PROJECT_MAP.md | `docs/PROJECT_MAP.md` | Lược bỏ bảng Quy tắc Vàng, Quy trình chuẩn và Phụ lục Stack/Architecture để file gọn nhẹ, tập trung phần lõi tra cứu |
| 2026-09-22 | Task 04 – Layout & Navigation | `Source/Components/Layout/MainLayout.razor`, `Source/Components/Layout/NavMenu.razor`, `Source/Components/Pages/Home.razor`, `Source/Components/Pages/Words/WordList.razor`, `Source/Components/Pages/Topics/TopicManage.razor`, `Source/wwwroot/app.css`, `Source/Components/App.razor`, `Source/Components/_Imports.razor` | Thiết kế MainLayout responsive (desktop/mobile), NavMenu với branding và icon, Home Dashboard thống kê từ IVocabularyService, CSS tùy chỉnh hiện đại |
| 2026-09-22 | Task 05 & 06 – Topic Management & Word List | `Source/Components/Pages/Topics/TopicManage.razor`, `Source/Components/Pages/Words/WordList.razor`, `Source/Components/Pages/Words/WordFormModal.razor`, `Source/Components/Shared/ConfirmDeleteModal.razor`, `Source/Services/VocabularyService.cs`, `Source/Components/_Imports.razor`, `Source/wwwroot/app.css` | Triển khai hoàn chỉnh tính năng quản lý chủ đề (/topics) và danh sách từ vựng (/words) với debounce search, bộ lọc, toggle IsMastered, phân trang server-side, modal thêm/sửa và modal xác nhận xóa dùng chung |
| 2026-09-22 | Task 08 & 09 – Shared Components, Integration & Testing | `Source/Models/AlertType.cs`, `Source/Components/Shared/LoadingSpinner.razor`, `Source/Components/Shared/AlertMessage.razor`, `Source/Components/Shared/ConfirmDeleteModal.razor`, `Source/Components/Pages/Topics/TopicManage.razor`, `Source/Components/Pages/Words/WordList.razor`, `Source/Components/Pages/Home.razor`, `README.md` | Hoàn thiện bộ Shared Components (AlertMessage, LoadingSpinner, ConfirmDeleteModal backdrop), tích hợp vào các trang, kiểm thử E2E HTTP endpoints, dọn dẹp debug logging và tài liệu hóa toàn diện |
| 2026-09-23 | Task 10 – Code Commenting Convention | `[NEW] .agents/rules/code-commenting.md`, `Source/Data/Entities/Topic.cs`, `Source/Data/Entities/Word.cs`, `Source/Data/AppDbContext.cs`, `Source/Models/AlertType.cs`, `Source/Models/WordFilterModel.cs`, `Source/Services/IVocabularyService.cs`, `Source/Services/VocabularyService.cs`, `Source/Components/Shared/AlertMessage.razor`, `Source/Components/Shared/ConfirmDeleteModal.razor`, `Source/Components/Shared/LoadingSpinner.razor`, `Source/Components/Pages/Topics/TopicManage.razor`, `Source/Components/Pages/Words/WordList.razor`, `Source/Components/Pages/Words/WordFormModal.razor`, `Source/Program.cs` | Tạo rule comment code thống nhất (EN, 5W1H, XML Docs, section dividers, #region) và áp dụng cho toàn bộ codebase. Thêm file rule `.agents/rules/code-commenting.md` |
| 2026-09-24 | Cập nhật Rule #region | `.agents/rules/code-commenting.md`, `Source/Services/VocabularyService.cs`, `Source/Services/IVocabularyService.cs`, `Source/Program.cs`, `Source/Data/AppDbContext.cs`, `Source/Components/Pages/Topics/TopicManage.razor`, `Source/Components/Pages/Words/WordList.razor`, `docs/design/03_architecture.md` | Cập nhật quy ước phân vùng chức năng trong code C# sang dùng `#region ... #endregion` thay thế cho comment phân đoạn `// ───` |
| 2026-09-24 | Khởi tạo Solution file (.slnx) | `LearnNN_WebBlazor.slnx` | Tạo solution file XML (.slnx) cho repo và add project `Source/LearnNN_WebBlazor.csproj` vào solution |
| 2026-09-24 | Phân tích & Tạo Task Import/Export | `docs/task/12_import_export_data/README.md`, `docs/task/README.md`, `docs/PROJECT_MAP.md` | Phân tích phương án Import/Export dữ liệu (đề xuất CSV/JSON), tạo Task 12 để chuẩn bị triển khai |
| 2026-09-24 | Task 12 - Import/Export Data | `Source/Models/WordCsvRecord.cs`, `Source/Services/IVocabularyService.cs`, `Source/Services/VocabularyService.cs`, `Source/Components/Pages/Words/WordList.razor`, `Source/Components/App.razor` | Triển khai tính năng Import/Export từ vựng ra CSV sử dụng thư viện CsvHelper. Thêm JS Interop tải file. |
| 2026-09-27 | Chuyển đổi dữ liệu TOEIC 600 JSON sang CSV | `CrawData/convert_json_to_csv.py`, `CrawData/toeic_600_words_import.csv` | Tạo script Python chuyển đổi dữ liệu từ vựng TOEIC JSON sang định dạng CSV import của web (UTF-8 BOM, map các trường TopicName, Term, Meaning, Pronunciation, PartOfSpeech, Example, ExampleTranslation, IsMastered) |
| 2026-09-28 | Task 13 – Vocabulary Study Hub, Games & Quizzes | `docs/task/13_vocabulary_study_hub/README.md`, `Source/Models/Study/*`, `Source/Services/IStudyService.cs`, `Source/Services/StudyService.cs`, `Source/Components/Layout/NavMenu.razor`, `Source/Components/Pages/Study/*`, `Source/wwwroot/study.css`, `Source/wwwroot/study.js`, `Source/Components/App.razor`, `Source/Components/Pages/Home.razor` | Xây dựng phân hệ Học từ vựng toàn diện: NavMenu 'Học từ vựng' (/study), StudyHub với bộ lọc & thống kê, Flashcard 3D lật thẻ + TTS Web Speech API, Game Nối từ (Word Match), Game Xếp chữ (Word Scramble), Bài kiểm tra trắc nghiệm 4 đáp án (Quiz) và cập nhật trạng thái học |
| 2026-10-07 | Task 14 – GitHub Actions CI/CD Publish | `.github/workflows/dotnet.yml` | Tối ưu hóa workflow GitHub Actions: build Release, test, dotnet publish ứng dụng Blazor Server .NET 10 và upload artifact lên GitHub Actions |
