# 📥 Task 12: Import/Export Dữ Liệu (Vocabulary)

## 1. Mô tả công việc (Task Description)
Tính năng Import/Export dữ liệu cho phép người dùng dễ dàng đưa một số lượng lớn từ vựng vào hệ thống (thường từ file Excel/CSV do người dùng tự soạn) và xuất dữ liệu hiện tại ra để sao lưu hoặc chia sẻ.

## 2. Phân tích & Lựa chọn định dạng
Sau khi đánh giá các phương án (CSV, Excel, JSON, XML), hệ thống sẽ sử dụng 2 định dạng chính:
- **CSV (Comma-Separated Values)**: Sử dụng cho tính năng **Import/Export danh sách từ vựng**.
  - *Lý do*: Nhẹ, dễ xử lý, người dùng có thể dùng Excel soạn thảo rồi lưu dưới dạng `.csv`. Phù hợp để nhập liệu hàng loạt.
  - *Thư viện đề xuất*: `CsvHelper` (nhẹ, phổ biến, dễ dùng).
- **JSON (JavaScript Object Notation)**: Sử dụng cho tính năng **Backup/Restore toàn bộ dữ liệu** (bao gồm cả Topic và Word).
  - *Lý do*: Bảo toàn cấu trúc phân cấp (Topic -> Words), không cần thư viện ngoài (sử dụng `System.Text.Json` có sẵn).

## 3. Chi tiết Implementation

### 3.1. CSV Import/Export (Quản lý Từ vựng)
- **Export**:
  - Tạo endpoint hoặc action trên Blazor trả về file `words_export.csv`.
  - Các cột: `Term`, `Meaning`, `Pronunciation`, `Example`, `TopicName`, `IsMastered`.
- **Import**:
  - Hỗ trợ tải lên file `.csv` (thông qua component `<InputFile>` của Blazor).
  - Xử lý: Đọc file, kiểm tra xem `TopicName` đã tồn tại chưa (nếu chưa thì tạo mới), sau đó thêm `Word` vào Topic đó.
  - Xử lý lỗi: Bỏ qua các dòng lỗi (thiếu Term/Meaning) và hiển thị tóm tắt (Ví dụ: "Đã import thành công 50 từ, lỗi 2 dòng").

### 3.2. Cập nhật UI
- Thêm nút **"📥 Import"** và **"📤 Export"** tại trang `/words` (`WordList.razor`).
- Nút "Import" sẽ mở một Modal cho phép chọn file `.csv` và có kèm theo link "Tải file mẫu (Template)".

### 3.3. Các bước thực hiện (Checklist)
- [x] Cài đặt package `CsvHelper`.
- [x] Cập nhật `IVocabularyService` và `VocabularyService` thêm các hàm: `ExportWordsToCsvAsync()`, `ImportWordsFromCsvAsync(Stream fileStream)`.
- [x] Cập nhật `WordList.razor`:
  - Thêm UI Buttons.
  - Xử lý logic tải file export về máy client (Dùng JS Interop để trigger tải file).
  - Thêm Modal Import chứa `<InputFile>` và xử lý tải lên.
- [x] Thêm validation cho quá trình Import (file quá lớn, sai định dạng).
- [x] Viết Unit Test/Integration Test cho chức năng Import/Export (nếu cần).

## 4. UI/UX Note
- Khi import số lượng lớn, cần hiển thị `LoadingSpinner`.
- Sau khi import, hiển thị `AlertMessage` báo cáo số lượng thành công/thất bại.
