# 📋 Task 05 – Quản lý Chủ đề (Topic Management)
## TopicManage.razor – CRUD Topics

> **Trạng thái**: ✅ Hoàn thành  
> **Độ ưu tiên**: 🟡 Trung bình  
> **Phụ thuộc**: Task 03 (Service Layer), Task 04 (Layout)  
> **Tài liệu tham chiếu**: [03_architecture.md](../../design/03_architecture.md)

---

## Mục tiêu

Tạo trang quản lý chủ đề từ vựng với đầy đủ CRUD. Giao diện đơn giản, nhanh gọn để người dùng tạo và quản lý danh mục topic trước khi thêm từ vựng.

---

## Danh sách công việc

### 5.1 Tạo TopicManage.razor
- [x] Route: `/topics`
- [x] Tiêu đề trang: "📁 Quản lý Chủ đề"
- [x] Bảng danh sách Topics:
  | Cột | Mô tả |
  |---|---|
  | # | Số thứ tự |
  | Tên chủ đề | `Name` |
  | Mô tả | `Description` |
  | Số từ | Count Words trong Topic |
  | Ngày tạo | `CreatedAt` (format: dd/MM/yyyy) |
  | Hành động | Nút Sửa / Xóa |

### 5.2 Chức năng Thêm Topic
- [x] Nút "➕ Thêm chủ đề" mở inline form hoặc modal
- [x] Form fields:
  - `Name` (Required, max 100 ký tự)
  - `Description` (Optional, max 250 ký tự)
- [x] Validation:
  - Tên không được để trống
  - Tên không được trùng (gọi `TopicNameExistsAsync`)
- [x] Sau khi thêm → refresh danh sách, hiện thông báo thành công

### 5.3 Chức năng Sửa Topic
- [x] Click nút "Sửa" → mở form edit với dữ liệu hiện tại
- [x] Cho phép sửa `Name` và `Description`
- [x] Validate trùng tên (loại trừ chính nó: `excludeId`)
- [x] Cập nhật `UpdatedAt` khi lưu

### 5.4 Chức năng Xóa Topic
- [x] Click nút "Xóa" → hiện modal xác nhận
- [x] Hiển thị cảnh báo: "Topic này có X từ vựng. Xóa sẽ mất tất cả từ vựng liên quan!"
- [x] Xác nhận → xóa Topic (cascade delete Words)
- [x] Hủy → đóng modal

### 5.5 Kiểm tra
- [x] Thêm topic mới thành công
- [x] Sửa topic đã có thành công
- [x] Xóa topic (có words) → cascade delete đúng
- [x] Validation hiển thị lỗi khi tên trống hoặc trùng
- [x] Bảng hiển thị số từ đúng cho mỗi topic

---

## Kết quả đầu ra (Deliverables)
- `Components/Pages/Topics/TopicManage.razor`
