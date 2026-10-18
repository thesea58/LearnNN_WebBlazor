# 📓 Task 25 – Progress Log

## Trạng thái
- **Sub-task đang làm**: Hoàn thành 25.07; chuyển sang 25.08 (Virtualize / List bounds) · **Vòng**: 7 · **Fail liên tiếp**: 0

## Bảng vi phạm (từ 25.01)
| # | File | Dòng | Nhóm | Rủi ro | Mô tả vi phạm & giải pháp | TT |
|---|---|---|---|---|---|---|
| 1 | `Source/Components/Pages/Home.razor` | 211 | Nhóm 25.02 | Thấp | `@foreach (var word in recentWords)` render `<tr>` thiếu `@key`. Thêm `@key="word.Id"`. | ✅ |
| 2 | `Source/Components/Pages/Home.razor` | 266 | Nhóm 25.02 | Thấp | `@foreach (var topic in topicsList)` render danh sách nhóm chủ đề thiếu `@key`. Thêm `@key="topic.Id"`. | ✅ |
| 3 | `Source/Components/Pages/Weather.razor` | 26 | Nhóm 25.02 | Thấp | `@foreach (var forecast in forecasts)` render bảng thời tiết thiếu `@key`. Thêm `@key="forecast.Date"`. | ✅ |
| 4 | `Source/Components/Pages/Ai/AiInbox.razor` | 379 | Nhóm 25.02 | Thấp | `@foreach (var c in sandboxEnrichResult.Collocations)` badge thiếu `@key`. Thêm `@key="c"`. | ✅ |
| 5 | `Source/Components/Pages/Ai/AiInbox.razor` | 394 | Nhóm 25.02 | Thấp | `@foreach (var wf in sandboxEnrichResult.WordFamily)` họ từ thiếu `@key`. Thêm `@key="wf"`. | ✅ |
| 6 | `Source/Components/Pages/Study/QuizPractice.razor` | 187 | Nhóm 25.02 | TB | `@for (int i = 0; i < questions.Count; i++)` chi tiết đáp án kèm QuizExplanationCard thiếu `@key`. Thêm `@key="q.WordId"`. | ✅ |
| 7 | `Source/Components/Pages/Study/QuizPractice.razor` | 288 | Nhóm 25.02 | TB | `@for (int optIndex = 0; optIndex < currentQ.Options.Count; optIndex++)` nút trắc nghiệm thiếu `@key`. Thêm `@key="capturedIndex"`. | ✅ |
| 8 | `Source/Components/Pages/Study/StudyHub.razor` | 134 | Nhóm 25.02 | Thấp | `@foreach (var topic in topics)` dropdown `<option>` thiếu `@key`. Thêm `@key="topic.Id"`. | ✅ |
| 9 | `Source/Components/Pages/Study/WordMatchGame.razor` | 125 | Nhóm 25.02 | Cao | `@foreach (var card in cards)` lưới ghép từ thiếu `@key`. Thêm `@key="card.Id"`. | ✅ |
| 10 | `Source/Components/Pages/Study/WordScrambleGame.razor` | 141 | Nhóm 25.02 | TB | `@for (int i = 0; i < currentWordClean.Length; i++)` slot ký tự thiếu `@key`. Thêm `@key="slotIndex"`. | ✅ |
| 11 | `Source/Components/Pages/Study/WordScrambleGame.razor` | 164 | Nhóm 25.02 | TB | `@foreach (var chip in availableChips)` chip ký tự thiếu `@key`. Thêm `@key="chip.Id"`. | ✅ |
| 12 | `Source/Components/Pages/Topics/TopicManage.razor` | 81 | Nhóm 25.02 | Cao | `@for (int i = 0; i < topics.Count; i++)` Table view chứa TopicProgressCircle thiếu `@key`. Thêm `@key="topic.Id"`. | ✅ |
| 13 | `Source/Components/Pages/Topics/TopicManage.razor` | 145 | Nhóm 25.02 | Cao | `@for (int i = 0; i < topics.Count; i++)` Card view chứa TopicProgressCircle thiếu `@key`. Thêm `@key="topic.Id"`. | ✅ |
| 14 | `Source/Components/Pages/Words/WordFormModal.razor` | 56 | Nhóm 25.02 | Thấp | `@foreach (var topic in Topics)` dropdown `<option>` modal thiếu `@key`. Thêm `@key="topic.Id"`. | ✅ |
| 15 | `Source/Components/Pages/Words/WordList.razor` | 70 | Nhóm 25.02 | Thấp | `@foreach (var topic in topics)` dropdown `<option>` filter toolbar thiếu `@key`. Thêm `@key="topic.Id"`. | ✅ |
| 16 | `Source/Components/Pages/Words/WordList.razor` | 178 | Nhóm 25.02 | Cao | `@for (int i = 0; i < words.Count; i++)` Table view danh sách từ vựng chính thiếu `@key`. Thêm `@key="word.Id"`. | ✅ |
| 17 | `Source/Components/Pages/Words/WordList.razor` | 287 | Nhóm 25.02 | Cao | `@for (int i = 0; i < words.Count; i++)` Card view danh sách từ vựng chính thiếu `@key`. Thêm `@key="word.Id"`. | ✅ |
| 18 | `Source/Components/Pages/Words/WordList.razor` | 407 | Nhóm 25.02 | Thấp | `@for (int p = 1; p <= totalPages; p++)` nút phân trang thiếu `@key`. Thêm `@key="targetPage"`. | ✅ |
| 19 | `Source/Components/Pages/Words/WordList.razor` | 547 | Nhóm 25.03 | TB | `debounceTimer.Elapsed += async (sender, args) => ...` tạo `async void` tiềm ẩn crash circuit. Tách named handler kèm try/catch. | ✅ |
| 20 | `Source/Components/Pages/Words/WordList.razor` | 547, 917 | Nhóm 25.04 | TB | `debounceTimer.Elapsed` lambda ẩn danh không thể `debounceTimer.Elapsed -=` trong `Dispose()`. Tách named handler để unregister. | ✅ |
| 21 | `Source/Components/Shared/AiManualBridgeModal.razor` | 174-196 | Nhóm 25.04 | Thấp | `CopyPromptAsync` có `await Task.Delay(2000)` rồi gọi `StateHasChanged()` không CancellationToken khi modal đóng. Implement IDisposable + CTS. | ✅ |
| 22 | `Source/Components/Pages/Topics/TopicManage.razor` | 344 | Nhóm 25.05 | Thấp | `viewModeHelper.getViewMode` kiểm tra `< 768px` thay vì breakpoint thống nhất `< 1024px`. Đồng bộ với Task 24. | ✅ |
| 23 | `Source/Components/Pages/Words/WordList.razor` | 574 | Nhóm 25.05 | Thấp | Tương tự dòng 344 của TopicManage. Đồng bộ với Task 24. | ✅ |
| 24 | `Source/Components/Shared/QuizExplanationCard.razor` | 105, 116 | Nhóm 25.06 | Cao | `[Parameter] public bool IsExpanded` bị gán đột biến `IsExpanded = !IsExpanded;`. Đổi sang chỉ emit `IsExpandedChanged.InvokeAsync`. | ✅ |
| 25 | `Source/Components/Shared/ViewModeSwitcher.razor` | 28, 42 | Nhóm 25.06 | Cao | `[Parameter] public ViewMode CurrentMode` bị gán `CurrentMode = newMode;`. Đổi sang chỉ emit `CurrentModeChanged.InvokeAsync`. | ✅ |
| 26 | `Source/Components/Layout/MainLayout.razor` | 22-24 | Nhóm 25.07 | Cao | `@Body` chưa được bọc `<ErrorBoundary>`. Bọc `<ErrorBoundary>` kèm fallback UI thân thiện và nút Thử lại. | ✅ |
| 27 | `Source/Components/Pages/Study/QuizPractice.razor` | 664 | Nhóm 25.07 | Thấp | `StateHasChanged()` dư thừa trong `finally` của event handler `@onclick`. Loại bỏ. | ✅ |
| 28 | `Source/Components/Pages/Study/QuizPractice.razor` | 719 | Nhóm 25.07 | Thấp | `StateHasChanged()` dư thừa trong `finally` của event handler `@onclick`. Loại bỏ. | ✅ |
| 29 | `Source/Components/Pages/Study/QuizPractice.razor` | 800 | Nhóm 25.07 | Thấp | `StateHasChanged()` dư thừa trong `finally` của EventCallback submit. Loại bỏ. | ✅ |
| 30 | `Source/Components/Pages/Ai/AiSettings.razor` | 357 | Nhóm 25.07 | Thấp | `StateHasChanged()` dư thừa trong `finally` của event handler `@onclick`. Loại bỏ. | ✅ |
| 31 | `Source/Components/Pages/Topics/TopicManage.razor` | 380 | Nhóm 25.08 | TB | Nạp toàn bộ chủ đề không giới hạn khi số lượng lớn. Cân nhắc phân trang hoặc Virtualize. | ⬜ |
| 32 | `Source/Components/Pages/Ai/AiInbox.razor` | 462-466 | Nhóm 25.08 | TB | Nạp tối đa 100 yêu cầu không phân trang. Cân nhắc Virtualize. | ⬜ |
| 33 | `Source/Components/Pages/Home.razor` | 301 | Nhóm 25.08 | Thấp | `topicsList = await VocabularyService.GetAllTopicsAsync();` không `.Take(5)` như recentWords. Giới hạn top 5. | ⬜ |

## Nhật ký
| Vòng | Thời gian | Sub-task | Việc đã làm | Build/Test/So ảnh PC | Commit |
|---|---|---|---|---|---|
| 1 | 2026-10-19 01:14 | 25.01 | Khảo sát kiểm kê 33 vi phạm kiến trúc thuộc 7 nhóm (25.02 -> 25.08) trên toàn bộ codebase; không sửa code | N/A (chỉ đọc) | `7a95f9b` |
| 2 | 2026-10-19 01:23 | 25.02 | Bổ sung `@key` duy nhất cho toàn bộ 18 vòng lặp `@foreach`/`@for` tại 10 file Razor component | Passed 73/73 tests (Build 0 warnings, so ảnh PC 1366x768 & 1920x1080 khớp 100%, Reviewer PASS) | `be19925` |
| 3 | 2026-10-19 01:31 | 25.03 | Thay thế lambda ẩn danh async void của debounceTimer trong WordList.razor bằng named handler OnDebounceTimerElapsed bọc try/catch an toàn | Passed 73/73 tests (Build 0 warnings, so ảnh PC 1366x768 & 1920x1080 khớp 100%, Reviewer PASS) | `1b96f68` |
| 4 | 2026-10-19 01:37 | 25.04 | Hủy đăng ký debounceTimer.Elapsed trong WordList.razor Dispose(); triển khai IDisposable và CancellationTokenSource hủy Task.Delay khi đóng modal trong AiManualBridgeModal.razor | Passed 73/73 tests (Build 0 warnings, so ảnh PC 1366x768 & 1920x1080 khớp 100%, Reviewer PASS) | `88d8495` |
| 5 | 2026-10-19 01:43 | 25.05 | Đồng bộ breakpoint tự động phát hiện ViewMode (card < 1024px, table >= 1024px) trong study.js và Razor components theo mobile-desktop-lock.md | Passed 73/73 tests (Build 0 warnings, so ảnh PC 1366x768 & 1920x1080 khớp 100%, Reviewer PASS) | `fb22bde` |
| 6 | 2026-10-19 01:49 | 25.06 | Loại bỏ đột biến [Parameter] trực tiếp trong QuizExplanationCard và ViewModeSwitcher, hỗ trợ local fallback và phát EventCallback | Passed 73/73 tests (Build 0 warnings, so ảnh PC 1366x768 & 1920x1080 khớp 100%, Reviewer PASS) | `e1f7df1` |
| 7 | 2026-10-19 01:55 | 25.07 | Bọc ErrorBoundary quanh @Body trong MainLayout (kèm auto-recover trên OnParametersSet) và dọn dẹp StateHasChanged() dư thừa trong finally block của các async handler | Passed 73/73 tests (Build 0 warnings, so ảnh PC 1366x768 & 1920x1080 khớp 100%, Reviewer PASS) | pending |
