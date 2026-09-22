# 📋 Task 07 – Form Thêm/Sửa Từ vựng (Word Form Modal)
## WordFormModal.razor – Add & Edit Word

> **Trạng thái**: ⬜ Chưa bắt đầu  
> **Độ ưu tiên**: 🔴 Cao  
> **Phụ thuộc**: Task 03 (Service Layer), Task 06 (Word List)  
> **Tài liệu tham chiếu**: [02_database_design.md](../../design/02_database_design.md)

---

## Mục tiêu

Tạo modal form dùng chung cho cả Thêm mới và Chỉnh sửa từ vựng. Sử dụng `EditForm` + `DataAnnotationsValidator` của Blazor, hỗ trợ validation real-time.

---

## Danh sách công việc

### 7.1 Tạo WordFormModal.razor
- [ ] Component dạng Bootstrap Modal
- [ ] Parameters:
  - `Word? EditWord` – null = Thêm mới, có giá trị = Chỉnh sửa
  - `EventCallback OnSaved` – callback khi lưu thành công
  - `EventCallback OnCancelled` – callback khi hủy
- [ ] Title động: "➕ Thêm từ mới" hoặc "✏️ Sửa từ: {Term}"

### 7.2 Form Fields
- [ ] Các trường nhập liệu:

  | Field | Label | Kiểu | Validation | Ghi chú |
  |---|---|---|---|---|
  | TopicId | Chủ đề | Dropdown select | Required | Load từ GetAllTopicsAsync() |
  | Term | Từ/Cụm từ | Input text | Required, MaxLength(100) | |
  | Phonetic | Phiên âm IPA | Input text | MaxLength(100) | Placeholder: /ˈæp.əl/ |
  | PartOfSpeech | Từ loại | Dropdown select | Optional | Options: noun, verb, adjective, adverb, phrase |
  | Meaning | Nghĩa | Textarea | Required, MaxLength(500) | |
  | ExampleSentence | Câu ví dụ | Textarea | MaxLength(500) | |
  | ExampleTranslation | Dịch ví dụ | Textarea | MaxLength(500) | |

### 7.3 Validation
- [ ] Sử dụng `EditForm` + `DataAnnotationsValidator`
- [ ] Hiển thị `ValidationMessage` bên dưới mỗi field
- [ ] `ValidationSummary` ở đầu form (tùy chọn)
- [ ] Disable nút "Lưu" khi form invalid

### 7.4 Xử lý Submit
- [ ] **Thêm mới**: Gọi `CreateWordAsync(word)` → set `CreatedAt = UtcNow`
- [ ] **Chỉnh sửa**: Gọi `UpdateWordAsync(word)` → set `UpdatedAt = UtcNow`
- [ ] Sau khi lưu → đóng modal, gọi `OnSaved` callback
- [ ] Loading state: Hiển thị spinner trên nút "Lưu" khi đang xử lý
- [ ] Error handling: Hiển thị thông báo lỗi nếu save thất bại

### 7.5 Nút hành động
- [ ] **Lưu** (btn-primary): Submit form
- [ ] **Hủy** (btn-secondary): Đóng modal, reset form

### 7.6 Kiểm tra
- [ ] Mở modal Thêm → form trống, title "Thêm từ mới"
- [ ] Mở modal Sửa → form có dữ liệu, title "Sửa từ: algorithm"
- [ ] Submit form trống → validation errors hiển thị
- [ ] Submit form hợp lệ → lưu thành công, modal đóng
- [ ] Dropdown Topic hiển thị đúng danh sách

---

## Kết quả đầu ra (Deliverables)
- `Components/Pages/Words/WordFormModal.razor`
