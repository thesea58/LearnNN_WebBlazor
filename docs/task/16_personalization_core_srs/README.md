# 🎯 Task 16: Xương sống Cá nhân hóa & Thuật toán Lặp lại Ngắt quãng (Personalization Core & SRS Engine)

> **Mức độ ưu tiên**: 🔴 Cao nhất (Mang tính quyết định nền tảng)  
> **Phụ thuộc**: Task 02, Task 03, Task 13  
> **Mục tiêu**: Chuyển đổi cơ chế đánh dấu `IsMastered` đơn giản sang mô hình học tập khoa học **Spaced Repetition System (SRS - SuperMemo 2)**, tạo bảng phân loại kỹ năng `SkillTag` và lưu toàn bộ vết học tập `AnswerLog` làm nguyên liệu cho hệ thống phân tích và AI.

---

## 1. Tầm quan trọng Quyết định (Why this is Critical)
Theo kết quả thống nhất thiết kế:
1. **Không có nhật ký học (`AnswerLog`)**: Mọi hoạt động làm bài sẽ biến mất, hệ thống không có dữ liệu thực tế để biết người dùng yếu ở đâu (chỉ đoán mò).
2. **Không có phân loại kỹ năng (`SkillTag`)**: Hệ thống không thể cá nhân hóa hay vẽ bản đồ năng lực (Radar chart).
3. **Không có SRS (`WordProgress`)**: Người học sẽ học trước quên sau, không biết hôm nay cần ôn lại từ nào.

---

## 2. Chi tiết công việc (Work Items)

### 2.1. Cập nhật Database & Entities (`Source/Data/Entities/`)
- [ ] **`LearnerProfile.cs`**:
  - `Id`, `TargetScore` (VD: 650), `ExamDate`, `DailyGoalMinutes` (VD: 30), `CurrentStage` (S1->S4), `CreatedAt`, `UpdatedAt`.
- [ ] **`SkillTag.cs`**:
  - `Id`, `Code` (UNIQUE: VD `VOC.WORD_FORM`, `GRAM.TENSE.PAST`), `Name`, `Category` (`Vocabulary`, `Grammar`, `Listening`, `Reading`), `ParentId` (hỗ trợ phân cấp cây kỹ năng).
- [ ] **`TagMastery.cs`**:
  - `Id`, `SkillTagId`, `MasteryScore` (0.0 -> 1.0), `TotalAttempts`, `CorrectAttempts`, `LastPracticedAt`.
- [ ] **`WordProgress.cs`** (Thay thế cho `IsMastered` cứng):
  - `Id`, `WordId`, `DueDate` (ngày cần ôn tập tiếp theo), `IntervalDays` (khoảng cách ngày), `EaseFactor` (mặc định 2.5), `Repetitions` (số lần ôn thành công liên tiếp), `Lapses` (số lần quên/trả lời sai), `LastStudiedAt`.
- [ ] **`AnswerLog.cs`**:
  - `Id`, `SessionId` (Guid), `ItemType` (Word / Question), `ItemId`, `SkillTagId`, `IsCorrect`, `ResponseTimeMs`, `SelectedAnswer`, `CreatedAt`.
- [ ] **`AppDbContext.cs`**:
  - Khai báo các `DbSet<>`, cấu hình Fluent API, Foreign Keys, Index tối ưu hóa truy vấn (`DueDate`, `SkillTagId`, `SessionId`).
  - Tạo EF Core Migration (`AddPersonalizationCoreAndSrs`).
  - Seed dữ liệu danh mục `SkillTag` ban đầu (Bộ từ vựng, Ngữ pháp cơ bản).

---

### 2.2. Service Layer – Nghiệp vụ SRS & Vết học tập (`Source/Services/`)
- [ ] **`ISrsEngineService.cs` & `SrsEngineService.cs`**:
  - Triển khai thuật toán **SuperMemo-2 (SM-2)**:
    - Input: Đánh giá chất lượng nhớ (0: Quên hoàn toàn, 3: Nhớ khó khăn, 4: Nhớ tốt, 5: Nhớ rất dễ).
    - Output: `NextDueDate`, `NewIntervalDays`, `NewEaseFactor`.
  - Hàm `GetDueWordsAsync(int limit)`: Lấy danh sách từ đến hạn ôn tập hôm nay (`DueDate <= DateTime.UtcNow`).
  - Hàm `RecordReviewResultAsync(int wordId, int qualityRating)`: Cập nhật tiến độ SRS của từ.
- [ ] **`IMasteryTrackingService.cs` & `MasteryTrackingService.cs`**:
  - Hàm `LogAnswerAsync(...)`: Ghi nhận nhật ký câu trả lời vào `AnswerLog`.
  - Hàm `UpdateTagMasteryAsync(int skillTagId, bool isCorrect)`: Cập nhật điểm thành thạo lũy tiến theo tag.
  - Hàm `GetSkillRadarAsync()`: Trả về thống kê điểm mạnh / điểm yếu theo từng danh mục.
- [ ] **Tích hợp toàn diện với `StudyService.cs`**:
  - Nâng cấp các game hiện có (Flashcards, Match, Scramble, Quiz): Mỗi khi người dùng trả lời đúng/sai, tự động ghi `AnswerLog` và cập nhật `WordProgress` tương ứng thay vì chỉ gán `IsMastered = true`.
  - Duy trì tương thích ngược: Thuộc tính `Word.IsMastered` được tự động tính: `IsMastered = IntervalDays >= 21`.

---

### 2.3. Cập nhật UI Hiện có (`Source/Components/`)
- [ ] **Study Hub (`Pages/Study/StudyHub.razor`)**:
  - Bổ sung chỉ số **"Từ vựng đến hạn ôn tập hôm nay" (Due Today)** nổi bật trên màn hình.
  - Thêm nút "Ôn tập ngay" dẫn thẳng vào phiên Flashcard với các từ đến hạn.
- [ ] **FlashcardStudy (`Pages/Study/FlashcardStudy.razor`)**:
  - Thay đổi nút đánh giá: Thêm các mức độ nhớ chuẩn SRS (Quên / Khó / Tốt / Dễ) để cập nhật chính xác thuật toán SM-2.

---

## 3. Tiêu chí Hoàn thành (Definition of Done)
1. Migration chạy thành công vào SQLite mà không làm mất dữ liệu 600 từ TOEIC hiện có.
2. Học từ trên Flashcard hoặc chơi Quiz tự động sinh bản ghi trong `AnswerLog` và cập nhật `WordProgress`.
3. Sau khi học, từ vựng được tính chính xác `DueDate` tiếp theo theo thuật toán SM-2.
4. Mọi code tuân thủ quy tắc comment (`code-commenting.md`), phân vùng `#region` và kiến trúc phân tầng.
