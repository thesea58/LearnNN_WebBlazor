# 📋 LearnNN – Lịch sử Chỉnh sửa Tài liệu Thiết kế
## Document Revision History

> **Dự án**: LearnNN – Vocabulary Management App  
> **Workspace**: `d:\DEV_NET\LearnNN_WebBlazor`

---

## Bảng Lịch sử

| # | Ngày | Phiên bản | Tài liệu | Loại thay đổi | Mô tả thay đổi | Tác giả |
|---|---|---|---|---|---|---|
| 1 | 2026-09-22 | 1.0 | [01_project_overview.md](01_project_overview.md) | 🆕 Tạo mới | Đánh giá prompt ban đầu (7.5/10), xác định scope V1/V2, tech stack chính thức | LearnNN Team |
| 2 | 2026-09-22 | 1.0 | [02_database_design.md](02_database_design.md) | 🆕 Tạo mới | Thiết kế DB: ERD 2 bảng (Topics, Words), DDL SQL, EF Core entities, Seed Data 3 topics + 7 words | LearnNN Team |
| 3 | 2026-09-22 | 1.0 | [03_architecture.md](03_architecture.md) | 🆕 Tạo mới | Kiến trúc phân tầng, cấu trúc thư mục, IVocabularyService API, Program.cs config, routing map | LearnNN Team |
| 4 | 2026-09-22 | 1.0 | [database_design.xml](database_design.xml) | 🆕 Tạo mới | XML schema đầy đủ: entities, columns, constraints, indexes, relationships, seed data, future expansion | LearnNN Team |
| 5 | 2026-09-22 | 1.1 | Tất cả tài liệu | ✏️ Cập nhật | Chuyển database từ SQL Server sang **SQLite** (file-based): DDL, connection string, NuGet packages, default values, EF Core config | LearnNN Team |
| 6 | 2026-09-22 | 1.2 | [08_shared_components/](../task/08_shared_components/) | ➕ Bổ sung | Triển khai bộ Shared Components: ConfirmDeleteModal (backdrop click), LoadingSpinner, AlertMessage (auto-dismiss) & enum AlertType | LearnNN Team |
| 7 | 2026-09-22 | 1.3 | [09_integration_testing/](../task/09_integration_testing/) | ✏️ Cập nhật | Tích hợp toàn diện, kiểm thử E2E HTTP endpoints, kiểm tra logging, dọn dẹp code và hoàn tất README.md dự án | LearnNN Team |

---

## Ký hiệu Loại thay đổi

| Ký hiệu | Ý nghĩa |
|---|---|
| 🆕 Tạo mới | Tài liệu được tạo lần đầu |
| ✏️ Cập nhật | Chỉnh sửa nội dung hiện có |
| ➕ Bổ sung | Thêm phần/mục mới vào tài liệu |
| 🗑️ Xóa | Xóa bỏ tài liệu hoặc nội dung |
| 🔄 Tái cấu trúc | Sắp xếp lại cấu trúc tài liệu |

---

## Danh mục Tài liệu hiện tại

| File | Mô tả | Trạng thái |
|---|---|---|
| [00_history.md](00_history.md) | Lịch sử chỉnh sửa tài liệu (file này) | ✅ Hoàn thành |
| [01_project_overview.md](01_project_overview.md) | Tổng quan dự án, đánh giá prompt, scope | ✅ Hoàn thành |
| [02_database_design.md](02_database_design.md) | Thiết kế cơ sở dữ liệu (MD) | ✅ Hoàn thành |
| [03_architecture.md](03_architecture.md) | Kiến trúc hệ thống & cấu trúc project | ✅ Hoàn thành |
| [database_design.xml](database_design.xml) | Thiết kế cơ sở dữ liệu (XML) | ✅ Hoàn thành |
