# 🎯 Task 18: AI Quiz Explainer & Trap Detector

> **Mức độ ưu tiên**: 🔴 Cao  
> **Phụ thuộc**: Task 16 (Personalization Core & SRS), Task 17 (AI Infrastructure & Manual Bridge)  
> **Trạng thái**: ✅ Hoàn thành  
> **Mục tiêu**: Tích hợp AI giải thích câu sai, phân tích bẫy trắc nghiệm (Trap Detector) trực tiếp trên trang Kiểm tra trắc nghiệm (`/study/quiz`), hỗ trợ cả giải thích từng câu (Single Prompt) và giải thích hàng loạt câu sai (Batch Prompt) qua Manual Bridge và Gemini API.

---

## 1. Tầm quan trọng & Mục đích nghiệp vụ

1. **Biến bài kiểm tra thành bài học sâu**: Thay vì chỉ biết "Đúng" hay "Sai", người học được giải thích cụ thể:
   - Tại sao phương án đúng lại chính xác.
   - Bóc trần loại bẫy trắc nghiệm (bẫy từ loại, bẫy từ đồng âm, bẫy dịch nghĩa nhầm, bẫy thì động từ...).
   - Vì sao các phương án nhiễu (distractors) lại sai.
   - Điểm ngữ pháp / cấu trúc cốt lõi cần nhớ.
   - Lời khuyên sư phạm kiểu "thầy giáo Việt" giúp không bao giờ tái phạm lỗi.
2. **Hỗ trợ đầy đủ 3 chế độ AI (Auto, Manual, Hybrid)**:
   - Với **Manual Bridge**: Người học copy prompt sang ChatGPT/Gemini/Claude web và dán JSON về app với chi phí 0đ, không phụ thuộc API key.
   - Với **Auto (Gemini API)**: Bấm nút là AI phân tích và trả về ngay sau ~1-2s.
   - Với **Hybrid**: Tự động gọi API, nếu chạm giới hạn quota (HTTP 429) sẽ chuyển sang Manual Bridge mà không làm mất bài thi.
3. **Cơ chế Batch Explain (Giải thích theo lô)**:
   - Cuối bài thi, nếu có nhiều câu sai, nút "🤖 Giải thích tất cả câu sai" sẽ gom toàn bộ câu sai vào **1 prompt duy nhất**, người học chỉ cần copy-paste **1 lần** để nhận giải thích cho tất cả câu.
4. **Lưu vết loại bẫy (`TrapType`)**:
   - Ghi nhận `TrapType` vào bảng `AnswerLogs` để phục vụ phân tích điểm yếu học viên và vẽ biểu đồ radar/bẫy ở các pha tiếp theo.

---

## 2. Chi tiết công việc đã thực hiện (Completed Work Items)

### 2.1. Cập nhật Database & Entities (`Source/Data/Entities/`)
- [x] **`AnswerLog.cs`**:
  - Bổ sung thuộc tính `[MaxLength(100)] public string? TrapType { get; set; }` để lưu vết loại bẫy mà AI nhận diện.
- [x] **EF Core Migration**:
  - Tạo và thực thi Migration `20261008201038_AddTrapTypeToAnswerLog` cập nhật cột `TrapType` vào bảng `AnswerLogs`.

### 2.2. Models & DTOs (`Source/Models/`)
- [x] **`Source/Models/Ai/SandboxDto.cs`**:
  - Bổ sung `BatchQuizExplanationDto` và `BatchQuizItemExplanationDto` phục vụ xử lý mảng giải thích hàng loạt từ AI.
  - Hàm tiện ích `ToQuizExplanationDto(...)` chuyển đổi nhanh từ item theo lô sang model hiển thị đơn lẻ.
- [x] **`Source/Models/Study/QuizQuestionDto.cs`**:
  - Bổ sung thuộc tính `Explanation` kiểu `QuizExplanationDto?`.
  - Bổ sung cờ điều khiển giao diện `IsExplanationExpanded` (mặc định `true`) và `IsExplaining` (trạng thái loading spinner).

### 2.3. Service Layer & Prompt Engineering (`Source/Services/`)
- [x] **`IPromptBuilder.cs` & `PromptBuilder.cs`**:
  - Bổ sung phương thức `BuildBatchQuizExplanationPrompt(IReadOnlyList<QuizQuestionDto> questions)`: Đóng gói toàn bộ $N$ câu hỏi cần phân tích thành một prompt sư phạm chuẩn, định rõ ngữ cảnh, các lựa chọn, đáp án chọn/đúng và ràng buộc JSON schema array.
  - Chuẩn hóa prompt đơn lẻ `BuildQuizExplanationPrompt`.
- [x] **`IStudyService.cs` & `StudyService.cs`**:
  - Bổ sung phương thức `UpdateAnswerLogTrapTypeAsync(Guid sessionId, int wordId, string trapType)` để đồng bộ loại bẫy phát hiện vào bản ghi `AnswerLog` của phiên kiểm tra.

### 2.4. UI Components & Trải Nghiệm Người Dùng (`Source/Components/`)
- [x] **`Source/Components/Shared/QuizExplanationCard.razor`**:
  - Thẻ hiển thị chuyên biệt:
    - 🎯 **Trap Badge**: Đổi màu thông minh theo loại bẫy (Vàng: Từ loại; Lam: Đồng âm; Đỏ: Dịch nghĩa; Xanh: Thì; Xám: Tổng quát).
    - ✅ **Tại sao đáp án này chính xác**: Khối nền xanh lá làm nổi bật lý do đúng.
    - ⚠️ **Bóc trần bẫy & Vì sao các lựa chọn khác sai**: Khối nền vàng cam phân tích distractor.
    - 🔖 **Quy tắc ngữ pháp / Trật tự từ**: Khối badge code nổi bật.
    - 💡 **Mẹo sư phạm từ Thầy giáo**: Callout quote lời khuyên dễ hiểu bằng tiếng Việt.
    - Nút thu gọn / mở rộng (Collapsible) mượt mà.
- [x] **`Source/Components/Pages/Study/QuizPractice.razor`**:
  - **Khi trả lời từng câu (Active Question)**:
    - Hiển thị nút "🤖 Bóc trần bẫy & Giải thích AI" (hoặc "Giải thích câu này" nếu làm đúng).
    - Hiển thị spinner "AI đang bóc bẫy..." khi đang gọi AI.
    - Nhúng `QuizExplanationCard` ngay bên dưới câu hỏi.
  - **Màn hình tổng kết kết quả (Quiz Results)**:
    - Banner cảnh báo câu sai với nút "🤖 Giải thích tất cả {N} câu sai" (Batch Explain).
    - Nút "Bóc bẫy AI" / "Xem giải thích AI" cho từng câu trong Danh sách chi tiết (Review list).
  - **Kết nối Cầu nối thủ công (`AiManualBridgeModal`)**:
    - Điều phối mượt mà cả luồng Single Explain và Batch Explain khi ở chế độ `Manual` hoặc `Hybrid` bị vượt quota.
    - Tự động kiểm tra cú pháp, gỡ lỗi và phân phối kết quả về từng câu hỏi.

---

## 3. Kết Quả Kiểm Thử (Verification)

1. **Biên dịch & Build**:
   - `dotnet build` đạt 0 Error, 0 Warning trên .NET 10.
2. **Cơ sở dữ liệu**:
   - Migration `20261008201038_AddTrapTypeToAnswerLog` đã được áp dụng vào `LearnNN_VocabDB.db`.
3. **Luồng nghiệp vụ AI**:
   - Hỗ trợ đầy đủ Single Question Explain và Batch Quiz Explain.
   - Cache-first hoạt động qua `InputHash` của `AiService`.
   - Kết nối hai chiều với `AiManualBridgeModal` và parser khoan dung `LenientJsonParser`.

---

## 4. Tiêu chí Hoàn thành (Definition of Done)

- [x] Giao diện Quiz kiểm tra từ vựng có nút và thẻ bóc tách bẫy AI trực quan.
- [x] Tương thích 100% với cả 3 chế độ AI (Manual, Auto, Hybrid).
- [x] Hỗ trợ giải thích hàng loạt câu sai (Batch) để tối ưu hóa thao tác copy-paste.
- [x] `AnswerLog` lưu vết `TrapType`.
- [x] Toàn bộ mã nguồn tuân thủ quy tắc comment XML Docs, EN, 5W1H và `#region`.
