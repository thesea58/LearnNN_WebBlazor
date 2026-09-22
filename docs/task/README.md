# 📋 LearnNN – Task Management Overview
## Quản lý Tiến độ Công việc

> **Dự án**: LearnNN – Vocabulary Management App  
> **Tổng số Tasks**: 9  
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
| 08 | [Shared Components](08_shared_components/) | ConfirmDeleteModal, LoadingSpinner, AlertMessage | 🟡 TB | Task 01 | ⬜ Chưa bắt đầu |
| 09 | [Integration & Testing](09_integration_testing/) | Kiểm tra E2E, responsive, error handling, performance, cleanup | 🔴 Cao | Task 01→08 | ⬜ Chưa bắt đầu |

---

## Biểu đồ Phụ thuộc (Dependency Graph)

```
Task 01 (Project Setup)
  ├──► Task 02 (Database & Entities)
  │      └──► Task 03 (Service Layer)
  │             ├──► Task 05 (Topic Management)
  │             ├──► Task 06 (Word List) ◄── Task 05
  │             └──► Task 07 (Word Form Modal) ◄── Task 06
  ├──► Task 04 (Layout & Navigation)
  │      ├──► Task 05
  │      └──► Task 06
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
