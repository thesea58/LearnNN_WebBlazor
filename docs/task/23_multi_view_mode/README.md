# 📋 Task 23 – Chế độ xem đa dạng (Multi-View Mode: Table & Card Grid)
## Xem bảng bằng nhiều kiểu trình bày (Kiểu dòng & Kiểu thẻ icon), tối ưu màn hình cảm ứng Android & iPhone

> **Trạng thái**: ✅ Hoàn thành  
> **Độ ưu tiên**: 🔴 Cao (Tối ưu trải nghiệm cốt lõi trên Mobile & Desktop)  
> **Ngày thực hiện**: 2026-10-09  
> **Phụ thuộc**: Task 05 (Topic Management), Task 06 (Word List)  
> **Tài liệu tham chiếu**: [PROJECT_MAP.md](../../PROJECT_MAP.md), [03_architecture.md](../../design/03_architecture.md)

---

## 1. Mục tiêu

Nâng cấp trải nghiệm duyệt và học từ vựng/chủ đề bằng cách cho phép người dùng linh hoạt chuyển đổi giữa **2 kiểu trình bày**:
1. **Kiểu dòng (Row / Table View)**: Bảng dữ liệu chuẩn với mật độ thông tin cao, dễ đối soát và sắp xếp nhiều trường thông tin trên Desktop.
2. **Kiểu icon / thẻ lưới (Card Grid View)**: Thẻ từ vựng/chủ đề trực quan sinh động, tối ưu 100% cho màn hình cảm ứng điện thoại **Android & iPhone**, tích hợp nút phát âm Web Speech API (TTS) nghe trực tiếp trên thẻ, nút toggle trạng thái kích thước lớn thân thiện ngón tay cái.

---

## 2. Danh sách công việc đã thực hiện

### 2.1 Định nghĩa Model & Shared Component
- [x] Tạo enum `ViewMode.cs` trong `Source/Models/` gồm 2 giá trị: `Table = 0`, `Card = 1`.
- [x] Tạo Shared Component `ViewModeSwitcher.razor` trong `Source/Components/Shared/` với thiết kế Segmented Control dạng pill, icon trực quan (`bi-list-ul` và `bi-grid-fill`).

### 2.2 Tối ưu hóa Thiết bị di động (Android & iPhone)
- [x] **Smart Responsive Default**: Tự động nhận diện độ rộng màn hình qua JS helper (`window.viewModeHelper` trong `Source/wwwroot/study.js`), nếu mở trên điện thoại/tablet nhỏ (`< 768px`) hệ thống tự động kích hoạt **Kiểu thẻ icon** làm mặc định.
- [x] **Ghi nhớ tùy chọn**: Lưu trạng thái (`table` hoặc `card`) vào `localStorage` của trình duyệt để giữ nguyên chế độ ưa thích của người dùng qua các phiên làm việc.
- [x] **Touch Targets lớn (≥ 42px)**: Nút loa nghe phát âm và nút đổi trạng thái "Đã thuộc" có diện tích bấm lớn, tránh bấm nhầm trên màn hình cảm ứng.
- [x] **Xử lý âm thanh trên iOS Safari & Android Chrome**: Gọi `speechSynthesis.cancel()` trước khi `speak()`, gắn trực tiếp vào sự kiện click để vượt qua chính sách User Activation của trình duyệt di động.
- [x] **Khử lỗi Sticky Hover**: Sử dụng `@media (hover: hover)` cho hiệu ứng hover máy tính và `@media (hover: none):active` phản hồi đàn hồi cho điện thoại.

### 2.3 Nâng cấp Trang Danh sách Từ vựng (`WordList.razor`)
- [x] Tích hợp `ViewModeSwitcher` vào thanh tiêu đề kho từ vựng.
- [x] Giữ nguyên Kiểu dòng (Table View) 9 cột hiện tại kèm nút loa nghe phát âm nhanh.
- [x] Xây dựng Kiểu thẻ icon (Card Grid View): Hiển thị Term, Phonetic, nút loa 🔊, Meaning, Example & Translation, Topic badge, Part of speech badge, nút đổi trạng thái học tập full-width và cụm nút Sửa/Xóa.
- [x] Phân trang (Pagination) hoạt động đồng bộ trên cả 2 chế độ xem.

### 2.4 Nâng cấp Trang Quản lý Chủ đề (`TopicManage.razor`)
- [x] Tích hợp `ViewModeSwitcher` vào thanh tiêu đề chủ đề.
- [x] Bổ sung Kiểu thẻ chủ đề (Folder Card Grid View) trực quan, hiển thị icon folder lớn, mô tả, số lượng từ liên kết `/words?topicId=...`, ngày tạo và nút Sửa/Xóa.

---

## 3. Kết quả đầu ra (Deliverables)
- `Source/Models/ViewMode.cs`
- `Source/Components/Shared/ViewModeSwitcher.razor`
- `Source/wwwroot/study.js` (Thêm `viewModeHelper`)
- `Source/wwwroot/app.css` (Thêm style `.word-card`, `.topic-card`, `.view-mode-switcher`, touch buttons)
- `Source/Components/Pages/Words/WordList.razor`
- `Source/Components/Pages/Topics/TopicManage.razor`
