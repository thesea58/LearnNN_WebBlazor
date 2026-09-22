# 📋 Task 08 – Shared Components
## ConfirmDeleteModal, LoadingSpinner, AlertMessage

> **Trạng thái**: ✅ Hoàn thành  
> **Độ ưu tiên**: 🟡 Trung bình  
> **Phụ thuộc**: Task 01 (Project Setup)  
> **Tài liệu tham chiếu**: [03_architecture.md](../../design/03_architecture.md)

---

## Mục tiêu

Xây dựng các component tái sử dụng (reusable) dùng chung trên nhiều trang: modal xác nhận xóa, loading spinner, thông báo alert.

---

## Danh sách công việc

### 8.1 ConfirmDeleteModal.razor
- [x] Component modal Bootstrap xác nhận trước khi xóa
- [x] Parameters:
  - `string Title` – Tiêu đề modal (ví dụ: "Xóa từ vựng")
  - `string Message` – Nội dung cảnh báo (ví dụ: "Bạn có chắc muốn xóa từ 'algorithm'?")
  - `EventCallback OnConfirm` – Callback khi nhấn "Xóa"
  - `EventCallback OnCancel` – Callback khi nhấn "Hủy"
- [x] Nút:
  - "🗑️ Xóa" (btn-danger)
  - "Hủy" (btn-secondary)
- [x] Animation: fade in/out
- [x] Click backdrop → đóng modal

### 8.2 LoadingSpinner.razor
- [x] Component hiển thị spinner khi đang tải dữ liệu
- [x] Parameters:
  - `string? Message` – Thông báo kèm theo (ví dụ: "Đang tải dữ liệu...")
  - `bool IsVisible` – Hiển thị hay ẩn
- [x] Sử dụng Bootstrap spinner-border
- [x] Centered layout

### 8.3 AlertMessage.razor
- [x] Component hiển thị thông báo kết quả thao tác
- [x] Parameters:
  - `string Message` – Nội dung thông báo
  - `AlertType Type` (enum: Success, Warning, Error, Info)
  - `bool IsVisible`
  - `EventCallback OnDismiss` – Callback khi đóng alert
- [x] Auto-dismiss sau 3-5 giây (tùy chọn)
- [x] Sử dụng Bootstrap alert classes:
  - Success → `alert-success` (xanh)
  - Warning → `alert-warning` (vàng)
  - Error → `alert-danger` (đỏ)
  - Info → `alert-info` (xanh nhạt)
- [x] Nút ✕ để đóng thủ công

### 8.4 Kiểm tra
- [x] ConfirmDeleteModal mở/đóng đúng, callback hoạt động
- [x] LoadingSpinner hiển thị/ẩn đúng
- [x] AlertMessage hiển thị đúng type, auto-dismiss hoạt động

---

## Kết quả đầu ra (Deliverables)
- `Source/Models/AlertType.cs`
- `Source/Components/Shared/ConfirmDeleteModal.razor`
- `Source/Components/Shared/LoadingSpinner.razor`
- `Source/Components/Shared/AlertMessage.razor`
