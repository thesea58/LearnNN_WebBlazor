# 📋 LearnNN – Task Management Overview
## Quản lý Tiến độ Công việc

> **Dự án**: LearnNN – Vocabulary Management App  
> **Tổng số Tasks**: 23  
> **Ngày tạo**: 2026-09-22

---

## Tổng quan Task

| # | Task | Mô tả | Ưu tiên | Phụ thuộc | Trạng thái |
|---|---|---|---|---|---|
| 01 | [Project Setup](01_project_setup/) | Tạo project Blazor, cài NuGet, cấu hình connection string, tạo thư mục | 🔴 Cao | — | ✅ Hoàn thành |
| 02 | [Database & Entities](02_database_entities/) | Entity classes, AppDbContext, Fluent API, Seed Data, Migration | 🔴 Cao | Task 01 | ✅ Hoàn thành |
| 03 | [Service Layer](03_service_layer/) | IVocabularyService, VocabularyService, Filter model, DI registration | 🔴 Cao | Task 02 | ✅ Hoàn thành |
| 04 | [Layout & Navigation](04_layout_navigation/) | MainLayout, NavMenu, Home dashboard, CSS customization | 🟡 TB | Task 01 | ✅ Hoàn thành |
| 05 | [Topic Management](05_topic_management/) | TopicManage.razor – CRUD chủ đề, validate trùng tên | 🟡 TB | Task 03, 04 | ✅ Hoàn thành |
| 06 | [Word List](06_word_list/) | WordList.razor – Bảng, tìm kiếm, lọc, phân trang, toggle trạng thái | 🔴 Cao | Task 03, 04, 05 | ✅ Hoàn thành |
| 07 | [Word Form Modal](07_word_form_modal/) | WordFormModal.razor – Form thêm/sửa từ, EditForm, validation | 🔴 Cao | Task 03, 06 | ✅ Hoàn thành |
| 08 | [Shared Components](08_shared_components/) | ConfirmDeleteModal, LoadingSpinner, AlertMessage | 🟡 TB | Task 01 | ✅ Hoàn thành |
| 09 | [Integration & Testing](09_integration_testing/) | Kiểm tra E2E, responsive, error handling, performance, cleanup | 🔴 Cao | Task 01→08 | ✅ Hoàn thành |
| 10 | Code Commenting Convention | Tạo rule comment code thống nhất (EN, 5W1H, XML Docs, #region phân vùng chức năng) và áp dụng toàn bộ codebase | 🟡 TB | Task 01→09 | ✅ Hoàn thành |
| 11 | Solution File Setup | Tạo file solution định dạng XML (`LearnNN_WebBlazor.slnx`) và add project `Source/LearnNN_WebBlazor.csproj` | 🟢 Thấp | Task 01 | ✅ Hoàn thành |
| 12 | [Import/Export Data](12_import_export_data/) | Thêm tính năng Import/Export danh sách từ vựng từ file CSV và Backup bằng JSON | 🔴 Cao | Task 06 | ✅ Hoàn thành |
| 13 | [Vocabulary Study Hub](13_vocabulary_study_hub/) | Trung tâm học từ vựng: Flashcard 3D, Game nối từ, Xếp chữ, Bài kiểm tra trắc nghiệm | 🔴 Cao | Task 03, 04 | ✅ Hoàn thành |
| 14 | GitHub Actions CI/CD Publish | Cấu hình script workflow build, test và publish artifact Blazor Web App trên GitHub Actions | 🟢 Thấp | Task 01 | ✅ Hoàn thành |
| 15 | [Feature Research Draft](../design/draft.md) | Nghiên cứu & phác thảo chức năng app học tiếng Anh cá nhân hóa (lấy gốc → TOEIC), AI Gemini, phỏng vấn /grill-me hoàn tất | 🔴 Cao | Task 13 | ✅ Hoàn thành |
| 16 | [Personalization Core & SRS](16_personalization_core_srs/) | Bảng phân loại kỹ năng (SkillTag), nhật ký học (AnswerLog), thuật toán lặp lại ngắt quãng SM-2 (WordProgress) | 🔴 Cao nhất | Task 02, 13 | ✅ Hoàn thành |
| 17 | [AI Infrastructure & Manual Bridge](17_ai_infrastructure_manual_bridge/) | Hạ tầng AI đa kênh: PromptBuilder, Manual AI Bridge Modal, AI Inbox, Parser khoan dung & bảo mật | 🔴 Cao | Task 16 | ✅ Hoàn thành |
| 18 | [AI Quiz Explainer & Trap Detector](18_ai_quiz_explainer_trap_detector/) | Tích hợp AI giải thích câu sai và phân tích bẫy trực tiếp trên Quiz (/study/quiz) qua Manual Bridge & API | 🔴 Cao | Task 16, 17 | ✅ Hoàn thành |
| 19 | Import Question Bank & Grammar | Chuẩn hóa định dạng CSV/JSON import đề thi chuẩn, cây bài học ngữ pháp nền tảng cho Part 5/6 | 🟡 TB | Task 16 | ⬜ Chưa bắt đầu |
| 20 | TOEIC Reading Practice (Part 5-7) | Luyện tập phân hóa theo Part 5, 6, 7; giải thích đáp án & bóc trần bẫy, chế độ làm bài bấm giờ | 🟡 TB | Task 17, 18, 19 | ⬜ Chưa bắt đầu |
| 21 | TOEIC Listening & Audio Practice | Luyện nghe Part 1-4, Luyện chép chính tả (Dictation) kèm chấm diff, nghe đa giọng (US/UK/AU) | 🟡 TB | Task 16, 20 | ⬜ Chưa bắt đầu |
| 22 | Mock Test & Score Prediction | Thi thử trắc nghiệm Full (200 câu)/Mini (100 câu), quy đổi điểm TOEIC, radar kỹ năng & dự đoán điểm | 🟢 Thấp | Task 20, 21 | ⬜ Chưa bắt đầu |
| 23 | [Multi-View Mode (Table & Card Grid)](23_multi_view_mode/) | Chế độ xem đa dạng (Kiểu dòng & Kiểu thẻ icon), tối ưu cảm ứng Android/iPhone, phát âm TTS trực tiếp trên thẻ, lưu tùy chọn localStorage | 🔴 Cao | Task 05, 06 | ✅ Hoàn thành |


---

## Biểu đồ Phụ thuộc (Dependency Graph)

```
Task 01 (Project Setup)
  ├──► Task 02 (Database & Entities)
  │      └──► Task 03 (Service Layer)
  │             ├──► Task 05 (Topic Management)
  │             ├──► Task 06 (Word List) ◄── Task 05
  │             ├──► Task 07 (Word Form Modal) ◄── Task 06
  │             └──► Task 13 (Vocabulary Study Hub) ◄── Task 04
  ├──► Task 04 (Layout & Navigation)
  │      ├──► Task 05
  │      ├──► Task 06
  │      └──► Task 13
  └──► Task 08 (Shared Components)
             └──► Task 05, 06, 07

  Task 01→08 ──► Task 09 (Integration & Testing)
```

---

## Thứ tự thực hiện đề xuất

```
Phase 1 – Nền tảng (Foundation)
  ► Task 01 → Task 02 → Task 03

Phase 2 – UI cơ bản (Basic UI)
  ► Task 04 + Task 08 (song song)

Phase 3 – Tính năng chính (Core Features)
  ► Task 05 → Task 06 → Task 07

Phase 4 – Hoàn thiện (Finalize)
  ► Task 09
```

---

## Ký hiệu Trạng thái

| Icon | Trạng thái |
|---|---|
| ⬜ | Chưa bắt đầu |
| 🔄 | Đang thực hiện |
| ✅ | Hoàn thành |
| ❌ | Đã hủy |
| ⏸️ | Tạm dừng |
