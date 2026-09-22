# 📋 Task 09 – Tích hợp & Kiểm tra Tổng thể
## Integration Testing & Final Review

> **Trạng thái**: ⬜ Chưa bắt đầu  
> **Độ ưu tiên**: 🔴 Cao  
> **Phụ thuộc**: Task 01 → 08 (tất cả tasks trước)  
> **Tài liệu tham chiếu**: Tất cả tài liệu design

---

## Mục tiêu

Tích hợp tất cả các thành phần, kiểm tra end-to-end toàn bộ flow ứng dụng, sửa bug, tối ưu UI/UX và đảm bảo ứng dụng sẵn sàng sử dụng.

---

## Danh sách công việc

### 9.1 Kiểm tra End-to-End Flow

#### Flow 1: Quản lý Chủ đề
- [ ] Tạo topic mới "Du lịch" → thành công
- [ ] Sửa topic "Du lịch" → "Du lịch & Khám phá" → thành công
- [ ] Xóa topic không có từ → thành công, không lỗi
- [ ] Xóa topic có từ → hiện cảnh báo, cascade delete đúng

#### Flow 2: Quản lý Từ vựng
- [ ] Thêm từ mới "adventure" vào topic "Du lịch" → thành công
- [ ] Sửa từ "adventure" → cập nhật phonetic, meaning → thành công
- [ ] Toggle IsMastered → trạng thái thay đổi đúng
- [ ] Xóa từ → xác nhận → xóa khỏi danh sách

#### Flow 3: Tìm kiếm & Lọc
- [ ] Tìm kiếm "alg" → hiện "algorithm"
- [ ] Lọc Topic "Công nghệ" → chỉ hiện 3 từ
- [ ] Lọc "Đã thuộc" → chỉ hiện từ IsMastered = true
- [ ] Kết hợp: Topic "Giao tiếp" + "Chưa thuộc" → đúng kết quả
- [ ] Reset filter → hiện tất cả

#### Flow 4: Phân trang
- [ ] Thêm > 20 từ → phân trang xuất hiện
- [ ] Chuyển trang → dữ liệu đúng
- [ ] Filter + phân trang → reset về trang 1

### 9.2 Kiểm tra UI/UX
- [ ] Responsive: Test trên 3 kích thước (Desktop 1920px, Tablet 768px, Mobile 375px)
- [ ] Sidebar collapse đúng trên mobile
- [ ] Modal hiển thị đúng trên mobile
- [ ] Bảng horizontal scroll trên mobile (nếu cần)
- [ ] Loading spinner hiển thị khi chờ dữ liệu
- [ ] Alert message hiển thị và auto-dismiss

### 9.3 Kiểm tra Error Handling
- [ ] Xóa file `.db` → ứng dụng không crash, hiện thông báo lỗi hoặc tự tạo lại
- [ ] Submit form với dữ liệu không hợp lệ → validation errors rõ ràng
- [ ] Tạo topic trùng tên → thông báo "Tên chủ đề đã tồn tại"

### 9.4 Kiểm tra Performance
- [ ] Trang Word List load < 2 giây với 100 records
- [ ] Toggle IsMastered phản hồi < 500ms
- [ ] Tìm kiếm debounce hoạt động (không spam API)

### 9.5 Code Review & Cleanup
- [ ] Xóa code TODO/FIXME còn sót
- [ ] Đảm bảo naming convention nhất quán
- [ ] Kiểm tra DI lifetime (Scoped vs Transient)
- [ ] Kiểm tra async/await không có deadlock
- [ ] Xóa console.log / Debug.WriteLine còn sót

### 9.6 Tài liệu hóa
- [ ] Cập nhật README.md gốc của project
- [ ] Ghi lại hướng dẫn chạy (setup guide)
- [ ] Cập nhật [00_history.md](../../design/00_history.md) với các thay đổi

---

## Checklist chạy ứng dụng

```bash
# 1. Clone / mở project
cd d:\DEV_NET\LearnNN_WebBlazor

# 2. Restore packages
dotnet restore

# 3. Tạo/cập nhật database
dotnet ef database update

# 4. Chạy ứng dụng
dotnet run

# 5. Mở trình duyệt
# → https://localhost:5001 hoặc http://localhost:5000
```

---

## Kết quả đầu ra (Deliverables)
- Ứng dụng chạy ổn định end-to-end
- Bug-free trên các flow chính
- Responsive trên desktop & mobile
- README.md với hướng dẫn cài đặt & chạy
