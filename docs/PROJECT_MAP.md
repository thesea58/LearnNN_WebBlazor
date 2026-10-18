# 🗺️ PROJECT_MAP – Bản đồ Kiến trúc Dự án LearnNN_WebBlazor

> **Phiên bản**: 1.0
> **Ngày khởi tạo**: 2026-09-22
> **Kiến trúc**: Layered Architecture (3-Tier) trên nền Blazor Server (.NET 10)

---

## Mục 1: 🌳 Cây thư mục hiện tại (Directory Tree)

> Cập nhật lần cuối: 2026-10-07

```
LearnNN_WebBlazor/                          # 🏠 Repository root
│
├── 📁 .agents/                             # ⚙️ Cấu hình AI Agent (rules, skills)
│   └── 📁 rules/                           #    Các quy tắc hành vi cho AI
│       ├── 📄 code-commenting.md           #    Quy ước comment code (EN, 5W1H, XML Docs)
│       ├── 📄 git-commit-convention.md     #    Quy ước commit Git (Conventional Commits, semantic prefix)
│       ├── 📄 mobile-desktop-lock.md       #    Quy tắc thích ứng mobile & khóa giao diện PC (breakpoint 1024, bottom-nav, test Playwright)
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
│   │   ├── 📄 04_deployment_guide.md       #    ★ Hướng dẫn Deploy ra Public & Truy cập Điện thoại
│   │   ├── 📄 draft.md                     #    DRAFT phác thảo chức năng (lấy gốc → TOEIC, AI Gemini) – chờ review
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
│       ├── 📁 13_vocabulary_study_hub/     #    ✅ Task 13 – Vocabulary Study Hub, Games & Quizzes
│       ├── 📁 16_personalization_core_srs/ #    ✅ Task 16 – Personalization Core & SRS Engine
│       ├── 📁 17_ai_infrastructure_manual_bridge/ # ✅ Task 17 – AI Infrastructure & Manual Bridge
│       ├── 📁 18_ai_quiz_explainer_trap_detector/ # ✅ Task 18 – AI Quiz Explainer & Trap Detector
│       ├── 📁 23_multi_view_mode/          #    ✅ Task 23 – Multi-View Mode (Table & Card Grid)
│       ├── 📁 24_web_mobile/               #    ⬜ Task 24 – Web Mobile (README backlog, AGENT_PROMPT.md vòng lặp Agy CLI, PROGRESS.md nhật ký, evidence/)
│       └── 📁 25_blazor_refactor/          #    ⬜ Task 25 – Blazor Architecture Refactor (README, AGENT_PROMPT.md, PROGRESS.md)
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
│   │       ├── 📄 Word.cs                  #    Entity: Từ vựng
│   │       ├── 📄 LearnerProfile.cs        #    Entity: Hồ sơ & mục tiêu người học
│   │       ├── 📄 SkillTag.cs              #    Entity: Cây kỹ năng phân cấp
│   │       ├── 📄 TagMastery.cs            #    Entity: Độ thành thạo theo tag
│   │       ├── 📄 WordProgress.cs          #    Entity: Lịch trình lặp lại ngắt quãng SRS (SM-2)
│   │       ├── 📄 AnswerLog.cs             #    Entity: Nhật ký vết học tập từng câu
│   │       └── 📄 AiRequest.cs             #    Entity: Yêu cầu AI đa kênh & cache phản hồi
│   │
│   ├── 📁 Models/                          # 📦 ViewModels / DTOs
│   │   ├── 📄 AlertType.cs                 #    Enum: Kiểu thông báo (Success, Warning, Error, Info)
│   │   ├── 📄 ViewMode.cs                  #    Enum: Chế độ trình bày danh sách (Table, Card)
│   │   ├── 📄 WordFilterModel.cs           #    Filter model cho Word (search, paging)
│   │   ├── 📄 WordCsvRecord.cs             #    Model ánh xạ import/export CSV
│   │   ├── 📁 Ai/                          #    Models & DTOs cho phân hệ AI
│   │   │   ├── 📄 AiEnums.cs               #    Enum: AiMode, AiChannel, AiRequestStatus
│   │   │   ├── 📄 AiOptions.cs             #    Options cấu hình binding từ IConfiguration
│   │   │   ├── 📄 AiPromptDto.cs           #    DTO định nghĩa & render prompt
│   │   │   ├── 📄 AiValidationResult.cs    #    Kết quả parse, validate & fix prompt
│   │   │   ├── 📄 AiServiceResponse.cs     #    Phản hồi thống nhất Auto/Manual/Hybrid
│   │   │   └── 📄 SandboxDto.cs            #    DTO kiểm thử Quiz Explanation & Word Enrichment
│   │   ├── 📁 Personalization/             #    Models & DTOs cho SRS & Cá nhân hóa
│   │   │   ├── 📄 SrsRating.cs             #    Enum: 4 mức nhớ SM-2 (Blackout, Difficult, Good, Easy)
│   │   │   ├── 📄 SrsReviewResultDto.cs    #    DTO kết quả tính toán SM-2
│   │   │   ├── 📄 SrsStatisticsDto.cs      #    DTO thống kê tiến độ SRS (Due, Learning, Mastered)
│   │   │   ├── 📄 SrsIntervalPreviewDto.cs #    DTO dự toán số ngày ôn cho 4 nút flashcard
│   │   │   └── 📄 SkillRadarDto.cs         #    DTO phân tích năng lực radar kỹ năng
│   │   └── 📁 Study/                       #    Models & DTOs cho phân hệ học tập & games
│   │       ├── 📄 StudySessionOptions.cs   #    Tùy chọn cấu hình phiên học (Topic, Filter, DueSrsOnly)
│   │       ├── 📄 QuizQuestionDto.cs       #    DTO câu hỏi trắc nghiệm 4 đáp án
│   │       ├── 📄 MatchCardDto.cs          #    DTO thẻ bài ghép đôi (Word Match)
│   │       └── 📄 QuizResultDto.cs         #    DTO kết quả & đánh giá bài kiểm tra
│   │
│   ├── 📁 Services/                        # ⚡ Business Logic Layer
│   │   ├── 📄 IVocabularyService.cs        #    Interface – Contract cho vocabulary CRUD
│   │   ├── 📄 VocabularyService.cs         #    Implementation – Logic xử lý business
│   │   ├── 📄 ISrsEngineService.cs         #    Interface – Thuật toán SM-2, tính DueDate & thống kê SRS
│   │   ├── 📄 SrsEngineService.cs          #    Implementation – Lập lịch SRS SM-2 & đồng bộ IsMastered
│   │   ├── 📄 IMasteryTrackingService.cs   #    Interface – Ghi vết học tập AnswerLog & tính độ thành thạo
│   │   ├── 📄 MasteryTrackingService.cs    #    Implementation – Xử lý log & công thức Dampened Accuracy
│   │   ├── 📄 IStudyService.cs             #    Interface – Hoạt động học từ, sinh game, quiz & log vết
│   │   ├── 📄 StudyService.cs              #    Implementation – Xáo trộn, sinh distractors, cập nhật SRS game
│   │   └── 📁 Ai/                          #    Services & Executors tích hợp AI
│   │       ├── 📄 ILenientJsonParser.cs    #    Interface parser JSON khoan dung
│   │       ├── 📄 LenientJsonParser.cs     #    Implementation parser JSON tự sửa lỗi cú pháp & prompt sửa
│   │       ├── 📄 IPromptBuilder.cs        #    Interface bộ sinh prompt chuẩn sư phạm
│   │       ├── 📄 PromptBuilder.cs         #    Implementation sinh prompt & băm SHA256 cache
│   │       ├── 📄 IAiExecutor.cs           #    Interface điều phối kênh thực thi
│   │       ├── 📄 ManualBridgeExecutor.cs  #    Kênh cầu nối thủ công copy/paste
│   │       ├── 📄 GeminiApiExecutor.cs     #    Kênh gọi trực tiếp Google AI Studio REST API
│   │       ├── 📄 IAiService.cs            #    Interface facade trung tâm
│   │       └── 📄 AiService.cs             #    Implementation cache-first, đa kênh, tự động hạ cấp
│   │
│   ├── 📁 Components/                      # 🎨 UI Layer (Blazor Components)
│   │   ├── 📄 App.razor                    #    Root component (HTML shell)
│   │   ├── 📄 Routes.razor                 #    Router configuration
│   │   ├── 📄 _Imports.razor               #    Global using directives
│   │   ├── 📁 Layout/                      #    Layout components
│   │   │   ├── 📄 MainLayout.razor         #    Layout chính của ứng dụng
│   │   │   ├── 📄 MainLayout.razor.css     #    Scoped CSS cho MainLayout
│   │   │   ├── 📄 MobileBottomNav.razor    #    Thanh điều hướng đáy màn hình cho mobile (<1024px)
│   │   │   ├── 📄 MobileBottomNav.razor.css #   Scoped CSS cho MobileBottomNav
│   │   │   ├── 📄 NavMenu.razor            #    Thanh điều hướng (Home, Study, Words, Topics, AI Inbox)
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
│   │   │   ├── 📁 Ai/                      #    Pages phân hệ AI & Cài đặt
│   │   │   │   ├── 📄 AiInbox.razor        #    Hộp thư AI, hàng đợi Pending & AI Sandbox (/ai/inbox)
│   │   │   │   └── 📄 AiSettings.razor     #    Cài đặt chế độ AI, bảo mật key & chatbot web (/ai/settings)
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
│   │       ├── 📄 LoadingSpinner.razor     #    Component loading spinner tái sử dụng
│   │       ├── 📄 ViewModeSwitcher.razor   #    Component chuyển đổi chế độ xem (Dòng / Thẻ icon)
│   │       ├── 📄 TopicProgressCircle.razor #   Vòng tròn tỉ lệ tiến độ học từ vựng SVG (Đã thuộc, Cần ôn lại, Chưa học)
│   │       ├── 📄 AiManualBridgeModal.razor #   Modal cầu nối thủ công copy/paste dùng chung
│   │       └── 📄 QuizExplanationCard.razor #   Thẻ hiển thị giải thích AI & bóc bẫy trắc nghiệm chuyên biệt
│   │
│   ├── 📁 Migrations/                      # 🔄 EF Core Migration files (auto-generated)
│   │   ├── 📄 20260922..._InitialCreate.Designer.cs
│   │   ├── 📄 20260922..._InitialCreate.cs
│   │   ├── 📄 20261008..._AddPersonalizationCoreAndSrs.cs
│   │   ├── 📄 20261008..._AddAiInfrastructure.cs
│   │   ├── 📄 20261008..._AddTrapTypeToAnswerLog.cs
│   │   └── 📄 AppDbContextModelSnapshot.cs
│   │
│   ├── 📁 Properties/                      # ⚙️ Project properties
│   │   └── 📄 launchSettings.json          #    Cấu hình launch (ports, profiles)
│   │
│   └── 📁 wwwroot/                         # 🌐 Static files (served publicly)
│       ├── 📄 manifest.webmanifest         #    PWA Web App Manifest (tên, màu theme, biểu tượng)
│       ├── 📄 service-worker.js            #    Service Worker (cache static shell, bypass SignalR circuits, offline fallback)
│       ├── 📄 offline.html                 #    Trang ngoại tuyến độc lập khi mất kết nối mạng (Task 24.10)
│       ├── 📁 icons/                       #    PWA Icons & Favicons
│       │   ├── 📄 icon-192.png             #    PWA standard icon (192x192)
│       │   ├── 📄 icon-512.png             #    PWA standard icon (512x512)
│       │   ├── 📄 icon-maskable-192.png    #    PWA maskable icon cho Android (192x192)
│       │   ├── 📄 icon-maskable-512.png    #    PWA maskable icon cho Android (512x512)
│       │   └── 📄 apple-touch-icon.png     #    iOS Apple Touch Icon (180x180)
│       ├── 📄 app.css                      #    Custom CSS styles
│       ├── 📄 study.css                    #    CSS 3D flip card, matching game, scramble, quiz
│       ├── 📄 study.js                     #    JS Interop phát âm Web Speech API & audio effects
│       ├── 📄 ai-bridge.js                 #    JS Interop clipboard 1-chạm & mở tab chatbot
│       ├── 📄 favicon.png                  #    Favicon
│       └── 📁 lib/                         #    Third-party client libraries
│           └── 📁 bootstrap/              #    Bootstrap 5
│
├── 📄 LearnNN_WebBlazor.slnx               # Solution file định dạng XML (.NET 10 / Visual Studio 2022+)
├── 📄 AGENTS.md                            # Điểm vào cho AI Agent: trỏ tới PROJECT_MAP & các rule
├── 📄 start-mobile.bat                     # Script 1-Click khởi chạy app & mở Dev Tunnel ra điện thoại (Windows)
├── 📁 Tests/                               # 🧪 AUTOMATED TESTS & BASELINES
│   └── 📁 LearnNN_WebBlazor.Tests/         #    Project test Playwright .NET + NUnit + SkiaSharp (.NET 10)
│       ├── 📄 LearnNN_WebBlazor.Tests.csproj # File cấu hình test project
│       ├── 📄 TestServerFixture.cs         # Quản lý vòng đời web server cổng 5099 + test DB
│       ├── 📄 PcBaselineTests.cs           # Kiểm thử hồi quy thị giác PC (1366x768 & 1920x1080)
│       ├── 📄 MultiEngineHarnessTests.cs   # Kiểm thử đa engine (Chromium, WebKit, Firefox) & mobile viewports
│       ├── 📄 MobileNavigationTests.cs     # Kiểm thử thanh điều hướng đáy (BottomNav) & nút thoát topbar
│       ├── 📄 WordsAndTopicsMobileTests.cs # Kiểm thử giao diện di động Words & Topics (stack table, touch target)
│       ├── 📄 StudyMobileTests.cs          # Kiểm thử học từ vựng mobile (tap lật flashcard, sticky SRS, chống tràn)
│       ├── 📄 ModalAndAiMobileTests.cs     # Kiểm thử công thái học modal & chống tràn các trang AI (Task 24.06)
│       ├── 📄 DoubleSubmitGuardTests.cs    # Kiểm thử cơ chế chống double-submit khi click nhanh (Task 24.07)
│       ├── 📄 InputAndTouchErgonomicsTests.cs # Kiểm thử công thái học input, scroll-margin, touch gaps (Task 24.08)
│       ├── 📄 PwaMobileTests.cs            # Kiểm thử tính hợp lệ của PWA (manifest, icons, service worker, meta tags) (Task 24.09)
│       ├── 📄 BlazorResilienceTests.cs     # Kiểm thử tính kiên cường kết nối Blazor Server & offline (Task 24.10)
│       ├── 📄 MobilePerformanceTests.cs   # Kiểm thử hiệu năng mobile, nén Brotli/Gzip và Web Vitals LCP/CLS (Task 24.11)
│       ├── 📁 Helpers/                     # Tiện ích test (Blazor readiness, Image comparison, Overflow check)
│       └── 📁 Baselines/                   # 20 ảnh chụp màn hình PC chuẩn cho 10 routes
│
├── 📄 start-mobile.ps1                     # Script PowerShell xử lý Dev Tunnel persistent & sinh mã QR terminal
├── 📄 Dockerfile                           # Docker multi-stage build cho .NET 10 Blazor Server
├── 📄 docker-compose.yml                   # Docker Compose cấu hình container & persistent volume
├── 📄 .dockerignore                        # Docker ignore rules
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
| `Tests/` | Tự động hóa kiểm thử & Baselines | Project test Playwright, test DB riêng, ảnh chụp baseline | Mã nguồn ứng dụng chính, dữ liệu thật |

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
| 2026-10-07 | Task 15 – Nghiên cứu & Phác thảo chức năng | `docs/design/draft.md`, `docs/task/README.md` | Hoàn thiện bản thiết kế (v1.0) qua phỏng vấn /grill-me: Single-user, tập trung TOEIC L&R, Manual AI Bridge trước, tích hợp toàn diện SRS, ngân hàng đề import chuẩn |
| 2026-10-07 | Lập Kế hoạch Công việc & WBS (Task 16-22) | `docs/task/README.md`, `[NEW] docs/task/16_personalization_core_srs/README.md`, `docs/PROJECT_MAP.md` | Phân rã bản thiết kế thành chuỗi 7 task thực thi khoa học (Task 16 -> 22). Tạo đặc tả Task 16 là ưu tiên cao nhất mang tính quyết định toàn bộ hệ thống. |
| 2026-10-09 | Task 16 – Personalization Core & SRS Engine | `Source/Data/Entities/LearnerProfile.cs`, `SkillTag.cs`, `TagMastery.cs`, `WordProgress.cs`, `AnswerLog.cs`, `Source/Data/AppDbContext.cs`, `Source/Migrations/*AddPersonalizationCoreAndSrs*`, `Source/Models/Personalization/*`, `Source/Services/ISrsEngineService.cs`, `SrsEngineService.cs`, `IMasteryTrackingService.cs`, `MasteryTrackingService.cs`, `Source/Services/StudyService.cs`, `Source/Components/Pages/Study/*`, `Source/wwwroot/study.css` | Triển khai hoàn chỉnh Xương sống cá nhân hóa & Thuật toán lặp lại ngắt quãng SuperMemo-2 (SM-2): 5 Entities mới, cây kỹ năng taxonomy, thuật toán SM-2 kèm bảo vệ chuỗi trong Game, lưu vết AnswerLog, bộ 4 nút Flashcard SRS Anki-style, banner Due Today trên Study Hub |
| 2026-10-09 | Task 23 – Multi-View Mode (Table & Card Grid) | `Source/Models/ViewMode.cs`, `Source/Components/Shared/ViewModeSwitcher.razor`, `Source/Components/Pages/Words/WordList.razor`, `Source/Components/Pages/Topics/TopicManage.razor`, `Source/wwwroot/study.js`, `Source/wwwroot/app.css`, `docs/task/23_multi_view_mode/README.md` | Bổ sung tính năng xem danh sách theo 2 kiểu trình bày (Kiểu dòng & Kiểu thẻ icon), tối ưu màn hình cảm ứng Android & iPhone (touch target >= 42px, tự động nhận diện mobile < 768px, lưu localStorage, phát âm audio TTS trực tiếp trên thẻ) |
| 2026-10-09 | Task 17 – AI Infrastructure & Manual Bridge | `Source/Data/Entities/AiRequest.cs`, `Source/Data/AppDbContext.cs`, `Source/Migrations/*AddAiInfrastructure*`, `Source/Models/Ai/*`, `Source/Services/Ai/*`, `Source/Components/Pages/Ai/*`, `Source/Components/Shared/AiManualBridgeModal.razor`, `Source/wwwroot/ai-bridge.js`, `Source/Components/Layout/NavMenu.razor`, `Source/Program.cs`, `Source/appsettings.json`, `docs/task/17_ai_infrastructure_manual_bridge/README.md` | Xây dựng hạ tầng tích hợp AI đa kênh: 3 chế độ (Auto, Manual, Hybrid tự động hạ cấp an toàn khi 429), bộ parser JSON khoan dung (LenientJsonParser), PromptBuilder băm SHA256 cache, modal copy/paste (AiManualBridgeModal), AI Inbox (/ai/inbox) kèm AI Sandbox và trang Cài đặt AI (/ai/settings) |
| 2026-10-09 | Task 18 – AI Quiz Explainer & Trap Detector | `Source/Data/Entities/AnswerLog.cs`, `Source/Migrations/*AddTrapTypeToAnswerLog*`, `Source/Models/Ai/SandboxDto.cs`, `Source/Models/Study/QuizQuestionDto.cs`, `Source/Services/Ai/IPromptBuilder.cs`, `PromptBuilder.cs`, `Source/Services/IStudyService.cs`, `StudyService.cs`, `Source/Components/Shared/QuizExplanationCard.razor`, `Source/Components/Pages/Study/QuizPractice.razor`, `docs/task/18_ai_quiz_explainer_trap_detector/README.md` | Tích hợp AI giải thích câu hỏi và bóc trần bẫy trắc nghiệm trực tiếp trên Quiz (/study/quiz) với cả 3 chế độ (Auto, Manual, Hybrid). Hỗ trợ giải thích đơn lẻ & giải thích hàng loạt câu sai (Batch Explain), lưu vết TrapType vào AnswerLog, thẻ QuizExplanationCard chuyên biệt có nút thu gọn/mở rộng. |
| 2026-10-09 | Thêm Rule Git Commit Convention | `[NEW] .agents/rules/git-commit-convention.md` | Chuẩn hóa quy ước thông điệp Git Commit (Conventional Commits, semantic prefix, task reference) cho AI Agent và nhà phát triển |
| 2026-10-11 | Task 18 – QA Audit & Hardening | `Source/Components/Pages/Study/QuizPractice.razor`, `Source/Services/IStudyService.cs`, `Source/Services/StudyService.cs`, `Source/Services/Ai/PromptBuilder.cs`, `docs/task/18_ai_quiz_explainer_trap_detector/README.md` | Gia cố hệ thống theo kết quả rà soát QA: xử lý SQLite batch concurrency transaction, guard click đúp, an toàn biên index & null-check, thông báo lỗi AI trực quan, bộ lọc ngoại lệ JS audio. |
| 2026-10-11 | Vòng tròn tiến độ học từ vựng cho Topics | `Source/Components/Shared/TopicProgressCircle.razor`, `Source/Components/Pages/Topics/TopicManage.razor`, `Source/Services/VocabularyService.cs` | Thêm vòng tròn tỉ lệ học từ vựng dạng donut chart SVG (xám: chưa học/nền, xanh lá: đã thuộc, cam: cần ôn lại) ở góc thẻ và bảng chủ đề (/topics) theo yêu cầu người dùng. |
| 2026-10-11 | Nghiên cứu Deploy Public & Mobile | `[NEW] docs/design/04_deployment_guide.md`, `[NEW] Dockerfile`, `[NEW] docker-compose.yml`, `[NEW] .dockerignore`, `Source/Program.cs` | Nghiên cứu & xây dựng cẩm nang deploy đa phương án (Cloudflare Tunnel, Wi-Fi LAN, VPS Docker Caddy 24/7, Fly.io) truy cập điện thoại di động (HTTPS, WebSockets, persistent SQLite storage, PWA Add-to-Home-Screen) và cấu hình ForwardedHeaders cho Reverse Proxy. |
| 2026-10-12 | Fix AI API Key Detection & Settings Enhancement | `Source/Program.cs`, `Source/LearnNN_WebBlazor.csproj`, `Source/Services/Ai/GeminiApiExecutor.cs`, `Source/Components/Pages/Ai/AiSettings.razor`, `docs/PROJECT_MAP.md` | Tối ưu cơ chế nạp API Key đa nguồn (User/Machine Registry, .env, aliases GEMINI/GOOGLE/AI_API_KEY, tự động trim quotes, auto chuyển Hybrid), thêm UserSecretsId, thêm ô nhập trực tiếp API key & nút kiểm tra kết nối tức thì trên /ai/settings |
| 2026-10-12 | Upgrade Gemini Models to 3.5 | `Source/appsettings.json`, `Source/Models/Ai/AiOptions.cs`, `Source/Data/Entities/AiRequest.cs`, `docs/PROJECT_MAP.md` | Nâng cấp mô hình Gemini mặc định từ 2.5 lên `gemini-3.5-flash-lite` và `gemini-3.5-flash` theo chính sách cập nhật mới nhất của Google AI Studio |
| 2026-10-18 | Task 24 – Web Mobile (lập kế hoạch) | `[NEW] docs/task/24_web_mobile/README.md`, `AGENT_PROMPT.md`, `PROGRESS.md`, `docs/task/README.md`, nhánh git `web_mobile` | Tạo nhánh `web_mobile`, backlog 11 sub-task (responsive, PWA, mobile UX) và prompt orchestrator + sub-agents để Agy CLI lặp Sửa → Test → Commit liên tục; trạng thái lưu trong `PROGRESS.md` |
| 2026-10-19 | Task 24/25 – Chuẩn hóa rule mobile & kế hoạch | `[NEW] .agents/rules/mobile-desktop-lock.md`, `[NEW] AGENTS.md`, `[NEW] docs/task/25_blazor_refactor/*`, `docs/task/24_web_mobile/README.md`, `AGENT_PROMPT.md`, `docs/task/README.md` | Chốt rule khóa PC (breakpoint 1024, bottom-nav, Card view mặc định, modal sticky, guard double-submit, test Playwright 3 engine + so ảnh baseline); tách Task 25 refactor Blazor chạy sau 24.01 và trước 24.02; Task 24 chia 2 pha |
| 2026-10-19 | Task 24.01 – Test harness & Baseline PC | `[NEW] Tests/LearnNN_WebBlazor.Tests/*`, `Source/Services/StudyService.cs`, `LearnNN_WebBlazor.slnx`, `.gitignore` | Xây dựng test harness Playwright .NET (NUnit, SkiaSharp) kiểm thử 3 engine (Chromium, WebKit, Firefox), tạo 20 ảnh screenshot baseline PC chuẩn (1366x768 & 1920x1080) cho 10 routes với kiểm tra sai lệch pixel ≤ 0.1%, kiểm tra console và script chống tràn ngang |
| 2026-10-19 | Task 25 – Blazor Architecture Refactor | `Source/Components/**`, `Source/wwwroot/study.js`, `docs/task/25_blazor_refactor/*` | Tái cấu trúc chuẩn kiến trúc Blazor được bảo vệ bởi test baseline PC: @key duy nhất cho toàn bộ 18 vòng lặp render, chuyển debounce timer lambda sang named handler an toàn, xử lý Dispose & CancellationTokenSource hủy task delay khi đóng modal, đồng bộ breakpoint 1024px cho view mode, loại bỏ đột biến [Parameter] trong QuizExplanationCard & ViewModeSwitcher, bọc ErrorBoundary quanh @Body kèm auto-recover khi điều hướng, dọn StateHasChanged() dư thừa và giới hạn danh sách dashboard top 5 |
| 2026-10-19 | Task 24.02 – Breakpoint 1024 + Viewport/Safe-area | `Source/Components/App.razor`, `Source/Components/Layout/MainLayout.razor.css`, `Source/Components/Layout/NavMenu.razor.css`, `Source/wwwroot/app.css` | Nâng breakpoint layout từ 768px lên 1024px (giữ nguyên khai báo khối PC), thêm viewport-fit=cover vào meta viewport, cấu hình dynamic viewport height (100dvh), safe-area padding và overflow guard cho mobile/tablet |
| 2026-10-19 | Task 24.03 – Mobile Bottom Navigation | `[NEW] Source/Components/Layout/MobileBottomNav.razor`, `[NEW] Source/Components/Layout/MobileBottomNav.razor.css`, `Source/Components/Layout/MainLayout.razor`, `Source/Components/Layout/MainLayout.razor.css` | Triển khai thanh điều hướng dưới đáy (bottom-nav) 5 mục trên mobile (<1024px), ẩn hoàn toàn trên PC, ẩn trong 4 phiên học kèm nút thoát ở topbar, ẩn sidebar trên mobile và dự phòng padding-bottom cho main |
| 2026-10-19 | Task 24.04 – Words & Topics Mobile | `Source/Components/Pages/Words/WordList.razor`, `Source/Components/Pages/Topics/TopicManage.razor`, `Source/wwwroot/app.css` | Bổ sung mẫu table-responsive-stack với data-label cho bảng từ vựng và chủ đề trên mobile (<=767.98px), tối ưu touch target phân trang >=44px, nút hành động >=44px, cỡ chữ 16px cho form input và padding compact trên điện thoại |
| 2026-10-19 | Task 24.05 – Study Mobile & Flashcard Touch | `Source/Components/Pages/Study/FlashcardStudy.razor`, `Source/wwwroot/study.css`, `Source/wwwroot/study.js` | Tối ưu tap lật thẻ, 4 nút SRS dính đáy (sticky bottom) trong tầm ngón cái, hỗ trợ cử chỉ vuốt tùy chọn (mặc định tắt), tối ưu Match/Scramble/Quiz hiển thị gọn gàng không tràn ở viewport 360px |
| 2026-10-19 | Task 24.06 – Modals & AI Pages Mobile | `Source/wwwroot/app.css`, `Source/Components/App.razor`, `Source/Components/Shared/AiManualBridgeModal.razor`, `Source/Components/Pages/Ai/AiInbox.razor`, `Source/Components/Pages/Ai/AiSettings.razor`, `[NEW] Tests/LearnNN_WebBlazor.Tests/ModalAndAiMobileTests.cs` | Tối ưu công thái học Modal trên mobile (calc(100vw-24px), max-height: 90dvh hoặc visualViewport khi mở bàn phím ảo, header/footer sticky, body cuộn cảm ứng, touch target >= 44px), xử lý chống tràn ngang 360px cho các trang AI (/ai/inbox, /ai/settings) và code blocks, bổ sung bộ test tự động 6/6 test xanh |
| 2026-10-19 | Task 24.07 – Chống Double-Submit | `Source/Components/Pages/**`, `[NEW] Tests/LearnNN_WebBlazor.Tests/DoubleSubmitGuardTests.cs` | Triển khai cơ chế phòng chống double-submit 2 lớp (UI disabled + early-return guard trong handler) cho toàn bộ các nút gọi async (Flashcard SRS rating, WordFormModal, TopicManage save/delete, WordList delete/import, QuizPractice batch mastery/AI explainer, WordScrambleGame answer/skip, AiInbox delete/sandbox, AiSettings test connection), bổ sung bộ test tự động Playwright |
| 2026-10-19 | Task 24.08 – Công thái học Input & Hover | `Source/wwwroot/app.css`, `Source/Components/Pages/Words/WordList.razor`, `Source/Components/Pages/Ai/AiSettings.razor`, `[NEW] Tests/LearnNN_WebBlazor.Tests/InputAndTouchErgonomicsTests.cs` | Tối ưu công thái học nhập liệu & cảm ứng: inputmode="search" trên WordList, type/inputmode="url" trên AiSettings, scroll-margin-bottom 24px chống bàn phím ảo che input, form-check touch target >= 44px, khoảng cách phân trang 8px, @media (hover: none) loại bỏ hoàn toàn sticky hover trên mobile |
| 2026-10-19 | Task 24.09 – PWA Manifest & Service Worker | `Source/Components/App.razor`, `[NEW] Source/wwwroot/manifest.webmanifest`, `[NEW] Source/wwwroot/service-worker.js`, `[NEW] Source/wwwroot/icons/*`, `[NEW] Tests/LearnNN_WebBlazor.Tests/PwaMobileTests.cs` | Cấu hình PWA hoàn chỉnh: tạo manifest.webmanifest (standalone, theme-color #4f46e5), bộ 5 icons PNG chuẩn (192, 512, maskable 192/512, apple-touch-icon 180), liên kết thẻ meta/link trong App.razor, service-worker.js (cache-first static, offline navigation fallback, bypass SignalR circuits), bộ test tự động PwaMobileTests (4/4 test xanh) |
| 2026-10-19 | Task 24.10 – Resilience Blazor Server | `Source/Components/Layout/ReconnectModal.razor`, `Source/Components/Layout/ReconnectModal.razor.css`, `Source/Components/Layout/ReconnectModal.razor.js`, `Source/Program.cs`, `Source/wwwroot/service-worker.js`, `[NEW] Source/wwwroot/offline.html`, `[NEW] Tests/LearnNN_WebBlazor.Tests/BlazorResilienceTests.cs` | Gia cố tính kiên cường Blazor Server: ReconnectModal mobile min(calc(100vw-32px), 22rem), nút thử lại touch target >= 44px, Việt hóa thông điệp; JS auto-reconnect khi quay lại tab/đổi mạng (online, visibilitychange, focus) kèm guard isRetrying; trang offline.html độc lập tự reload khi có mạng; service-worker.js pre-cache & fallback offline; Program.cs mở rộng DisconnectedCircuitRetentionPeriod (10 phút) bảo vệ phiên học; bộ test BlazorResilienceTests (5/5 xanh) |
| 2026-10-19 | Task 24.11 – Performance Mobile | `Source/Program.cs`, `[NEW] Tests/LearnNN_WebBlazor.Tests/MobilePerformanceTests.cs` | Tối ưu hiệu năng mobile: kích hoạt Response Compression Brotli & Gzip (Level Fastest cho text/css/js/json/svg), cấu hình Cache-Control tĩnh (1 tuần cho icons/favicons, no-cache cho service-worker), đo kiểm Web Vitals LCP < 2.5s và CLS < 0.1 trên mobile iPhone 14; bộ test MobilePerformanceTests (5/5 xanh) |




