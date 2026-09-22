# 📋 Task 04 – Layout & Navigation
## MainLayout, NavMenu, Trang chủ

> **Trạng thái**: ⬜ Chưa bắt đầu  
> **Độ ưu tiên**: 🟡 Trung bình  
> **Phụ thuộc**: Task 01 (Project Setup)  
> **Tài liệu tham chiếu**: [03_architecture.md](../../design/03_architecture.md)

---

## Mục tiêu

Tùy chỉnh layout chính của ứng dụng: sidebar navigation, header, footer. Tạo trang chủ (Home) với thông tin tổng quan. Sử dụng Bootstrap 5 clean.

---

## Danh sách công việc

### 4.1 Cập nhật MainLayout.razor
- [ ] Sidebar navigation với branding "LearnNN 📘"
- [ ] Responsive layout (collapse sidebar trên mobile)
- [ ] Footer với thông tin phiên bản

### 4.2 Cập nhật NavMenu.razor
- [ ] Menu items với icon:
  - 🏠 Trang chủ → `/`
  - 📝 Từ vựng → `/words`
  - 📁 Chủ đề → `/topics`
- [ ] Highlight menu item đang active
- [ ] Collapse menu trên mobile

### 4.3 Tạo Home.razor (Dashboard)
- [ ] Route: `/`
- [ ] Hiển thị thông tin tổng quan:
  - Tổng số từ vựng
  - Số từ đã thuộc / chưa thuộc
  - Số chủ đề
- [ ] Card layout với Bootstrap 5
- [ ] Nút shortcut: "Thêm từ mới", "Xem danh sách"

### 4.4 Cập nhật CSS
- [ ] Tùy chỉnh `wwwroot/app.css`:
  - Color scheme phù hợp ứng dụng học tập
  - Typography sạch sẽ, dễ đọc
  - Hover effects cho buttons và menu items

### 4.5 Kiểm tra
- [ ] Layout hiển thị đúng trên desktop và mobile
- [ ] Navigation hoạt động, routing chính xác
- [ ] Home page hiển thị dữ liệu từ Service

---

## Routing Map

| URL | Component | Mô tả |
|---|---|---|
| `/` | `Home.razor` | Dashboard tổng quan |
| `/words` | `WordList.razor` | Danh sách từ vựng (Task 06) |
| `/topics` | `TopicManage.razor` | Quản lý chủ đề (Task 05) |

---

## Kết quả đầu ra (Deliverables)
- `Components/Layout/MainLayout.razor` (cập nhật)
- `Components/Layout/NavMenu.razor` (cập nhật)
- `Components/Pages/Home.razor` (tạo mới hoặc cập nhật)
- `wwwroot/app.css` (cập nhật styles)
