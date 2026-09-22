# 📋 Task 06 – Danh sách Từ vựng (Word List)
## WordList.razor – Hiển thị, Tìm kiếm, Lọc, Phân trang

> **Trạng thái**: ⬜ Chưa bắt đầu  
> **Độ ưu tiên**: 🔴 Cao (Trang chính của ứng dụng)  
> **Phụ thuộc**: Task 03 (Service Layer), Task 04 (Layout), Task 05 (Topics)  
> **Tài liệu tham chiếu**: [03_architecture.md](../../design/03_architecture.md), [02_database_design.md](../../design/02_database_design.md)

---

## Mục tiêu

Xây dựng trang danh sách từ vựng – trang chính và quan trọng nhất của ứng dụng. Bao gồm: bảng hiển thị, thanh tìm kiếm, bộ lọc, phân trang, toggle trạng thái, và các nút thao tác.

---

## Danh sách công việc

### 6.1 Tạo WordList.razor
- [ ] Route: `/words`
- [ ] Tiêu đề: "📝 Danh sách Từ vựng"
- [ ] Hiển thị tổng số từ: "Hiển thị X / Y từ"

### 6.2 Thanh công cụ (Toolbar)
- [ ] **Ô tìm kiếm** (input text):
  - Tìm theo `Term` hoặc `Meaning`
  - Debounce 300ms (không gọi API mỗi lần gõ phím)
  - Nút xóa tìm kiếm (clear)
- [ ] **Dropdown lọc Topic**:
  - Option: "Tất cả chủ đề" + danh sách Topics
  - Thay đổi → reload danh sách
- [ ] **Dropdown lọc trạng thái**:
  - "Tất cả" / "Chưa thuộc" / "Đã thuộc"
- [ ] **Nút "➕ Thêm từ mới"**:
  - Mở WordFormModal (Task 07)

### 6.3 Bảng dữ liệu (Table)
- [ ] Các cột hiển thị:

  | Cột | Field | Ghi chú |
  |---|---|---|
  | # | STT | Số thứ tự theo pagination |
  | Từ vựng | `Term` | **Bold**, font lớn hơn |
  | Phiên âm | `Phonetic` | Italic, màu xám |
  | Từ loại | `PartOfSpeech` | Badge màu (noun=blue, verb=green, adj=orange...) |
  | Nghĩa | `Meaning` | Hiển thị tối đa 80 ký tự, tooltip full |
  | Ví dụ | `ExampleSentence` | Hiển thị tối đa 60 ký tự |
  | Chủ đề | `Topic.Name` | Badge |
  | Trạng thái | `IsMastered` | Toggle switch / checkbox |
  | Hành động | - | Nút Sửa / Xóa |

### 6.4 Toggle IsMastered
- [ ] Click toggle trên bảng → gọi `ToggleMasteredAsync(wordId)`
- [ ] Cập nhật UI ngay lập tức (không cần reload toàn bộ)
- [ ] Hiển thị:
  - ✅ "Đã thuộc" (badge xanh)
  - ⬜ "Chưa thuộc" (badge xám)

### 6.5 Nút hành động
- [ ] **Sửa** (✏️): Mở WordFormModal ở chế độ Edit
- [ ] **Xóa** (🗑️): Mở ConfirmDeleteModal → xác nhận → xóa

### 6.6 Phân trang (Pagination)
- [ ] Server-side pagination (20 items/page)
- [ ] Hiển thị: "Trang X / Y" + nút Previous/Next
- [ ] Reset về trang 1 khi thay đổi filter/search

### 6.7 Trạng thái rỗng (Empty State)
- [ ] Khi không có từ nào: Hiển thị message "Chưa có từ vựng nào. Hãy thêm từ đầu tiên!"
- [ ] Khi search không có kết quả: "Không tìm thấy từ phù hợp."

### 6.8 Loading State
- [ ] Hiển thị spinner khi đang tải dữ liệu
- [ ] Disable các nút thao tác khi đang xử lý

### 6.9 Kiểm tra
- [ ] Tìm kiếm "algorithm" → hiện đúng kết quả
- [ ] Lọc Topic "Công nghệ" → chỉ hiện từ của topic đó
- [ ] Lọc "Đã thuộc" → chỉ hiện từ IsMastered = true
- [ ] Toggle trạng thái → giá trị thay đổi trong DB
- [ ] Phân trang hoạt động đúng
- [ ] Kết hợp filter + search + pagination

---

## Kết quả đầu ra (Deliverables)
- `Components/Pages/Words/WordList.razor`
