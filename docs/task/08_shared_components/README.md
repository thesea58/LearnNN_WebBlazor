# 📋 Task 08 – Shared Components
## ConfirmDeleteModal, LoadingSpinner, AlertMessage

> **Trạng thái**: ⬜ Chưa bắt đầu  
> **Độ ưu tiên**: 🟡 Trung bình  
> **Phụ thuộc**: Task 01 (Project Setup)  
> **Tài liệu tham chiếu**: [03_architecture.md](../../design/03_architecture.md)

---

## Mục tiêu

Xây dựng các component tái sử dụng (reusable) dùng chung trên nhiều trang: modal xác nhận xóa, loading spinner, thông báo alert.

---

## Danh sách công việc

### 8.1 ConfirmDeleteModal.razor
- [ ] Component modal Bootstrap xác nhận trước khi xóa
- [ ] Parameters:
  - `string Title` – Tiêu đề modal (ví dụ: "Xóa từ vựng")
  - `string Message` – Nội dung cảnh báo (ví dụ: "Bạn có chắc muốn xóa từ 'algorithm'?")
  - `EventCallback OnConfirm` – Callback khi nhấn "Xóa"
  - `EventCallback OnCancel` – Callback khi nhấn "Hủy"
- [ ] Nút:
  - "🗑️ Xóa" (btn-danger)
  - "Hủy" (btn-secondary)
- [ ] Animation: fade in/out
- [ ] Click backdrop → đóng modal

### 8.2 LoadingSpinner.razor
- [ ] Component hiển thị spinner khi đang tải dữ liệu
- [ ] Parameters:
  - `string? Message` – Thông báo kèm theo (ví dụ: "Đang tải dữ liệu...")
  - `bool IsVisible` – Hiển thị hay ẩn
- [ ] Sử dụng Bootstrap spinner-border
- [ ] Centered layout

### 8.3 AlertMessage.razor
- [ ] Component hiển thị thông báo kết quả thao tác
- [ ] Parameters:
  - `string Message` – Nội dung thông báo
  - `AlertType Type` (enum: Success, Warning, Error, Info)
  - `bool IsVisible`
  - `EventCallback OnDismiss` – Callback khi đóng alert
- [ ] Auto-dismiss sau 3-5 giây (tùy chọn)
- [ ] Sử dụng Bootstrap alert classes:
  - Success → `alert-success` (xanh)
  - Warning → `alert-warning` (vàng)
  - Error → `alert-danger` (đỏ)
  - Info → `alert-info` (xanh nhạt)
- [ ] Nút ✕ để đóng thủ công

### 8.4 Kiểm tra
- [ ] ConfirmDeleteModal mở/đóng đúng, callback hoạt động
- [ ] LoadingSpinner hiển thị/ẩn đúng
- [ ] AlertMessage hiển thị đúng type, auto-dismiss hoạt động

---

## Kết quả đầu ra (Deliverables)
- `Components/Shared/ConfirmDeleteModal.razor`
- `Components/Shared/LoadingSpinner.razor`
- `Components/Shared/AlertMessage.razor`
