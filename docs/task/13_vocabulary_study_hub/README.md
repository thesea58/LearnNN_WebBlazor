# 🎓 Task 13: Trung tâm Học Từ Vựng (Vocabulary Learning Hub, Games & Quizzes)

## 1. Mô tả công việc (Task Description)
Xây dựng phân hệ **Học từ vựng** toàn diện trên LearnNN, cung cấp nhiều chế độ học tương tác, trò chơi từ vựng giải trí và bài kiểm tra trắc nghiệm đánh giá kiến thức nhằm giúp người dùng ghi nhớ từ vựng lâu hơn và hào hứng học tập mỗi ngày.

## 2. Các tính năng chính (Key Features)

### 2.1. Menu Điều hướng & Hub Trung tâm (`/study`)
- **NavMenu**: Thêm mục "Học từ vựng" (`bi-mortarboard`) chuyển hướng trực tiếp đến `/study`.
- **Trang Hub**:
  - Bộ chọn chủ đề (Dropdown hỗ trợ "Tất cả chủ đề" hoặc chọn 1 chủ đề cụ thể).
  - Lọc theo trạng thái: "Chỉ từ chưa thuộc" (`IsMastered = false`) vs "Tất cả từ vựng".
  - Thống kê thời gian thực: Hiển thị số lượng từ sẵn sàng học theo bộ lọc đã chọn.
  - Danh mục truy cập nhanh vào 3 nhóm: Phương pháp học, Trò chơi và Kiểm tra.

### 2.2. Phương pháp học (Learning Modes)
- **Flashcard 3D (`/study/flashcards`)**:
  - Thẻ 3D với hiệu ứng lật mượt mà (`transform: rotateY(180deg)`).
  - Mặt trước: Từ vựng tiếng Anh, phiên âm quốc tế IPA, nút phát âm Text-to-Speech (Web Speech API).
  - Mặt sau: Nghĩa tiếng Việt, ví dụ câu tiếng Anh, dịch câu ví dụ.
  - Tương tác: Bấm nút hoặc nhấn phím tắt Space (lật mặt), Mũi tên Trái/Phải (chuyển thẻ).
  - Cập nhật tiến độ: Nút "Chưa thuộc" và "Đã thuộc" để cập nhật trực tiếp `IsMastered` vào cơ sở dữ liệu.

### 2.3. Game học từ vựng (Gamified Learning)
- **Word Match - Game Nối từ (`/study/matching`)**:
  - Bảng lưới thẻ xáo trộn ngẫu nhiên gồm các thẻ tiếng Anh và nghĩa tiếng Việt.
  - Cơ chế lật mở thẻ & ghép đôi: Thẻ đúng chuyển màu xanh và hoàn thành; thẻ sai rung lắc (shake) và tự úp lại.
  - Đếm thời gian (Timer), đếm số lượt click (Moves) và combo điểm số.
  - Màn hình chiến thắng khi hoàn thành toàn bộ bảng thẻ.
- **Word Scramble - Game Sắp xếp ký tự (`/study/scramble`)**:
  - Hiển thị nghĩa tiếng Việt, các chữ cái tiếng Anh của từ bị xáo trộn.
  - Người dùng bấm chọn các ký tự theo đúng thứ tự để tạo thành từ chuẩn xác.
  - Hỗ trợ nút Xóa ký tự, Trộn lại ký tự, Gợi ý chữ cái đầu và Bỏ qua.

### 2.4. Bài kiểm tra từ vựng (Vocabulary Quizzes & Tests)
- **Trắc nghiệm 4 đáp án (`/study/quiz`)**:
  - Hệ thống tự động bốc ngẫu nhiên 1 từ cần kiểm tra và lấy 3 từ khác trong cơ sở dữ liệu để làm phương án gây nhiễu (distractors).
  - Hỗ trợ cả 2 chiều: Hỏi từ tiếng Anh -> Chọn nghĩa tiếng Việt, hoặc Hỏi nghĩa tiếng Việt -> Chọn từ tiếng Anh.
  - Màn hình tổng kết (Quiz Result):
    - Điểm số đạt được, tỷ lệ phần trăm chính xác.
    - Bảng danh sách chi tiết các câu làm sai kèm đáp án đúng.
    - Nút thao tác nhanh: "Đánh dấu tất cả câu đúng là đã thuộc" và "Làm lại bài kiểm tra".

## 3. Kiến trúc Triển khai (Architecture & File Mapping)

| Layer | Files | Trách nhiệm |
|-------|-------|-------------|
| **Models / DTOs** | `Source/Models/Study/StudySessionOptions.cs`<br>`Source/Models/Study/QuizQuestionDto.cs`<br>`Source/Models/Study/MatchCardDto.cs`<br>`Source/Models/Study/QuizResultDto.cs` | DTO cấu hình phiên học, cấu trúc câu hỏi quiz, thẻ game ghép đôi và kết quả kiểm tra |
| **Services** | `Source/Services/IStudyService.cs`<br>`Source/Services/StudyService.cs` | Nghiệp vụ shuffle, sinh đáp án trắc nghiệm nhiễu, sinh cặp thẻ matching và cập nhật trạng thái học |
| **UI Components** | `Source/Components/Layout/NavMenu.razor`<br>`Source/Components/Pages/Study/StudyHub.razor`<br>`Source/Components/Pages/Study/FlashcardStudy.razor`<br>`Source/Components/Pages/Study/WordMatchGame.razor`<br>`Source/Components/Pages/Study/WordScrambleGame.razor`<br>`Source/Components/Pages/Study/QuizPractice.razor` | Các trang giao diện học tập, game và bài kiểm tra |
| **Static Assets** | `Source/wwwroot/study.css`<br>`Source/wwwroot/study.js` | CSS hiệu ứng 3D flip card, shake animation, và JS Interop phát âm Web Speech API |

## 4. Checklist Thực hiện

- [x] Tạo tài liệu Task 13 và cập nhật `docs/task/README.md`.
- [x] Xây dựng các Model & DTO trong `Source/Models/Study/`.
- [x] Xây dựng `IStudyService` và `StudyService`, đăng ký DI trong `Program.cs`.
- [x] Tạo `study.css` và `study.js`, nhúng vào `App.razor`.
- [x] Cập nhật `NavMenu.razor` thêm liên kết `/study`.
- [x] Tạo trang Hub trung tâm `StudyHub.razor`.
- [x] Tạo trang học Flashcard 3D `FlashcardStudy.razor`.
- [x] Tạo game Nối từ `WordMatchGame.razor`.
- [x] Tạo game Sắp xếp ký tự `WordScrambleGame.razor`.
- [x] Tạo trang kiểm tra trắc nghiệm `QuizPractice.razor`.
- [x] Cập nhật `docs/PROJECT_MAP.md` (Cây thư mục và AI Change Log).
- [x] Build và kiểm thử toàn diện.
