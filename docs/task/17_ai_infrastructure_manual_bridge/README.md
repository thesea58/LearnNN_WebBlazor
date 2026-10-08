# 🎯 Task 17: Hạ Tầng AI Đa Kênh & Cầu Nối Thủ Công (AI Infrastructure & Manual Bridge)

> **Mức độ ưu tiên**: 🔴 Cao  
> **Phụ thuộc**: Task 16  
> **Mục tiêu**: Xây dựng hạ tầng tích hợp AI đa kênh toàn diện cho LearnNN, hỗ trợ 3 chế độ (Auto, Manual, Hybrid), bộ parser JSON khoan dung (LenientJsonParser), bộ sinh prompt sư phạm (PromptBuilder), modal cầu nối thủ công copy/paste (AiManualBridgeModal), hàng đợi AI Inbox (/ai/inbox), và phòng thử nghiệm AI Sandbox.

---

## 1. Tầm quan trọng của Task 17
1. **Hoạt động không cần API Key**: Với **Manual AI Bridge**, người học có thể sử dụng sức mạnh của các mô hình AI tiên tiến nhất (Gemini Advanced, ChatGPT, Claude) ngay trên giao diện web mà không tốn chi phí và không bị giới hạn API rate limit.
2. **Sẵn sàng cho Tự động hóa**: Khi có API Key, kênh `GeminiApiExecutor` kích hoạt ngay lập tức. Chế độ `Hybrid` mang lại độ tin cậy tuyệt đối: khi API bị chạm giới hạn quota (HTTP 429) hoặc mạng lỗi, hệ thống tự động hạ cấp xuống Manual Bridge mà không làm mất bài thi/học.
3. **Bảo mật tuyệt đối**: Không lưu API Key trong mã nguồn hay commit lên GitHub. Hệ thống nạp key an toàn qua .NET User Secrets hoặc Biến môi trường.
4. **Trải nghiệm mượt mà**: Bộ phân tích JSON khoan dung tự động bóc tách khối markdown, sửa dấu ngoặc kép thông minh, dọn dẹp dấu phẩy thừa và tự sinh Prompt Sửa Lỗi nếu chatbot trả lời sai.

---

## 2. Chi tiết công việc đã thực hiện (Completed Work Items)

### 2.1. Cập nhật Database & Entities (`Source/Data/Entities/`)
- [x] **`AiRequest.cs`**:
  - `Id`, `RequestId` (UNIQUE index, mã tương quan dạng `REQ-YYYYMMDD-XXXX`), `Kind` (QuizExplanation, WordEnrichment...), `PromptVersion`, `InputHash` (SHA256 phục vụ caching tự động), `PromptText`, `Channel` (Manual / Api), `Status` (Pending / Completed / Failed / Invalid), `ResponseJson`, `ModelLabel`, `ErrorMessage`, `CreatedAt`, `CompletedAt`.
- [x] **`AppDbContext.cs`**:
  - Đăng ký `DbSet<AiRequest> AiRequests`.
  - Cấu hình index tối ưu cho `RequestId`, `InputHash`, `Status`, `CreatedAt`.
  - Tạo và thực thi EF Core Migration: `AddAiInfrastructure`.

### 2.2. Models & DTOs (`Source/Models/Ai/`)
- [x] **`AiEnums.cs`**: Định nghĩa `AiMode` (Manual, Auto, Hybrid), `AiChannel` (Manual, Api), `AiRequestStatus` (Pending, Completed, Failed, Invalid).
- [x] **`AiOptions.cs`**: Model ràng buộc cấu hình (`Ai:Mode`, `Ai:ApiKey`, `Ai:FastModel`, `Ai:SmartModel`, `Ai:PreferredChatbotUrl`).
- [x] **`AiPromptDto.cs`**: DTO định nghĩa tham số đầu vào và kết quả render prompt (`AiPromptDefinition`, `RenderedAiPrompt`).
- [x] **`AiValidationResult.cs` & `AiServiceResponse.cs`**: Đóng gói kết quả kiểm định, payload kiểu mạnh và prompt sửa lỗi (`FixPrompt`).
- [x] **`SandboxDto.cs`**: DTO cấu trúc `QuizExplanationDto` và `WordEnrichmentDto` phục vụ giải thích bẫy thi và làm giàu từ vựng.

### 2.3. Service Layer – Hạ Tầng AI (`Source/Services/Ai/`)
- [x] **`ILenientJsonParser.cs` & `LenientJsonParser.cs`**:
  - Bóc tách khối markdown ```` ```json ... ``` ```` kể cả khi chatbot chào hỏi trước/sau.
  - Tự động thay thế dấu smart quotes (`“`, `”`, `‘`, `’`) thành dấu chuẩn.
  - Regex loại bỏ dấu phẩy thừa cuối object/array (trailing commas).
  - So khớp `request_id` để ngăn dán nhầm kết quả.
  - Tự động sinh `FixPrompt` khi phát hiện lỗi cú pháp.
- [x] **`IPromptBuilder.cs` & `PromptBuilder.cs`**:
  - Xây dựng prompt TOEIC sư phạm theo chuẩn tiếng Việt.
  - Hàm chuyên biệt: `BuildQuizExplanationPrompt`, `BuildWordEnrichmentPrompt`.
  - Hàm băm `ComputeHash` SHA256 cho caching.
- [x] **`IAiExecutor.cs`, `ManualBridgeExecutor.cs` & `GeminiApiExecutor.cs`**:
  - `ManualBridgeExecutor`: Điều phối luồng thủ công, đăng ký yêu cầu Pending.
  - `GeminiApiExecutor`: Gọi Google AI Studio REST API với chế độ JSON (`responseMimeType: application/json`), xử lý lỗi 429 Quota Exceeded.
- [x] **`IAiService.cs` & `AiService.cs`**:
  - Facade trung tâm: Cache First thông minh qua `InputHash`.
  - Điều phối 3 chế độ (Auto, Manual, Hybrid với cơ chế tự động hạ cấp an toàn).
  - Tiếp nhận kết quả thủ công qua `CompleteManualRequestAsync`.
  - Cung cấp số lượng yêu cầu chờ cho NavMenu badge.

### 2.4. Giao Diện Người Dùng & Tương Tác Blazor (`Source/Components/`)
- [x] **`ai-bridge.js`**: JS Interop hỗ trợ sao chép Clipboard 1-chạm an toàn và mở tab chatbot web.
- [x] **`AiManualBridgeModal.razor`**:
  - Modal tái sử dụng: Bước 1 sao chép prompt + mở chatbot; Bước 2 dán JSON phản hồi.
  - Nút "Để sau (Lưu vào AI Inbox)", hiển thị lỗi và nút "Sao chép Prompt Sửa Lỗi".
- [x] **`AiInbox.razor` (/ai/inbox)**:
  - Tab 1: Danh sách yêu cầu đang chờ (`Pending`) kèm nút mở lại modal dán kết quả.
  - Tab 2: Lịch sử các yêu cầu đã hoàn tất (`Completed`) kèm xem chi tiết JSON.
  - Tab 3: **AI Sandbox** thử nghiệm trực quan luồng giải thích Quiz và làm giàu từ vựng qua cả Manual Bridge và Gemini API.
- [x] **`AiSettings.razor` (/ai/settings)**:
  - Chọn chế độ AI (Manual, Auto, Hybrid).
  - Hiển thị trạng thái API Key & cam kết bảo mật GitHub kèm lệnh hướng dẫn .NET User Secrets.
  - Tùy chọn mở nhanh Chatbot Web (Gemini, ChatGPT, Claude).
- [x] **`NavMenu.razor`**:
  - Thêm mục "AI Inbox" kèm Badge đỏ hiển thị số yêu cầu đang chờ.
  - Thêm mục "Cài đặt AI".

---

## 3. Kết Quả Kiểm Thử (Verification Results)
- Script kiểm thử tự động `AiTest` đã chạy thành công 100%:
  1. `PromptBuilder`: Sinh RequestId, InputHash và văn bản prompt đầy đủ.
  2. `LenientJsonParser`: Xử lý mượt mà văn bản có lời chào, smart quotes và trailing commas.
  3. Kiểm soát an toàn: Phát hiện chính xác trường hợp dán sai RequestId và sinh FixPrompt tương ứng.
  4. Phục hồi lỗi: Bắt lỗi cú pháp JSON hỏng và tạo FixPrompt bằng tiếng Việt.
- Ứng dụng build thành công với 0 Warning, 0 Error trên .NET 10.

---

## 4. Tiêu chí Hoàn thành (Definition of Done)
- [x] Toàn bộ code tuân thủ quy tắc comment XML Docs, EN, 5W1H và `#region`.
- [x] Database migration đã được cập nhật an toàn.
- [x] Sẵn sàng 100% để tích hợp vào Task 18 (AI Quiz Explainer & Trap Detector).
