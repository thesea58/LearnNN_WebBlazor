# 📋 Task 05 – Quản lý Chủ đề (Topic Management)
## TopicManage.razor – CRUD Topics

> **Trạng thái**: ⬜ Chưa bắt đầu  
> **Độ ưu tiên**: 🟡 Trung bình  
> **Phụ thuộc**: Task 03 (Service Layer), Task 04 (Layout)  
> **Tài liệu tham chiếu**: [03_architecture.md](../../design/03_architecture.md)

---

## Mục tiêu

Tạo trang quản lý chủ đề từ vựng với đầy đủ CRUD. Giao diện đơn giản, nhanh gọn để người dùng tạo và quản lý danh mục topic trước khi thêm từ vựng.

---

## Danh sách công việc

### 5.1 Tạo TopicManage.razor
- [ ] Route: `/topics`
- [ ] Tiêu đề trang: "📁 Quản lý Chủ đề"
- [ ] Bảng danh sách Topics:
  | Cột | Mô tả |
  |---|---|
  | # | Số thứ tự |
  | Tên chủ đề | `Name` |
  | Mô tả | `Description` |
  | Số từ | Count Words trong Topic |
  | Ngày tạo | `CreatedAt` (format: dd/MM/yyyy) |
  | Hành động | Nút Sửa / Xóa |

### 5.2 Chức năng Thêm Topic
- [ ] Nút "➕ Thêm chủ đề" mở inline form hoặc modal
- [ ] Form fields:
  - `Name` (Required, max 100 ký tự)
  - `Description` (Optional, max 250 ký tự)
- [ ] Validation:
  - Tên không được để trống
  - Tên không được trùng (gọi `TopicNameExistsAsync`)
- [ ] Sau khi thêm → refresh danh sách, hiện thông báo thành công

### 5.3 Chức năng Sửa Topic
- [ ] Click nút "Sửa" → mở form edit với dữ liệu hiện tại
- [ ] Cho phép sửa `Name` và `Description`
- [ ] Validate trùng tên (loại trừ chính nó: `excludeId`)
- [ ] Cập nhật `UpdatedAt` khi lưu

### 5.4 Chức năng Xóa Topic
- [ ] Click nút "Xóa" → hiện modal xác nhận
- [ ] Hiển thị cảnh báo: "Topic này có X từ vựng. Xóa sẽ mất tất cả từ vựng liên quan!"
- [ ] Xác nhận → xóa Topic (cascade delete Words)
- [ ] Hủy → đóng modal

### 5.5 Kiểm tra
- [ ] Thêm topic mới thành công
- [ ] Sửa topic đã có thành công
- [ ] Xóa topic (có words) → cascade delete đúng
- [ ] Validation hiển thị lỗi khi tên trống hoặc trùng
- [ ] Bảng hiển thị số từ đúng cho mỗi topic

---

## Kết quả đầu ra (Deliverables)
- `Components/Pages/Topics/TopicManage.razor`
