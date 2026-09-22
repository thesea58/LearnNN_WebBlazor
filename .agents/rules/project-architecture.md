---
trigger: always_on
---

# ARCHITECTURE & FILE CONVENTIONS RULE

1. **On-demand Context:** Khi cần tạo file mới, sửa cấu trúc hoặc tìm vị trí đặt code, BẮT BUỘC đọc file `docs/PROJECT_MAP.md`. Không tự tiện bịa thư mục.
2. **Layering:**
   - UI / Pages: `Source/Components/Pages/<Feature>/`
   - Shared UI: `Source/Components/Shared/` (chỉ khi dùng ≥ 2 pages)
   - Business Service: `Source/Services/`
   - Entities: `Source/Data/Entities/`
   - Models/DTOs: `Source/Models/`
3. **Update Duty:** Sau khi tạo file mới hoặc đổi cấu trúc, Agent PHẢI cập nhật lại cây thư mục (Mục 1) và thêm 1 dòng vào AI Change Log (Mục 4) trong `docs/PROJECT_MAP.md`.