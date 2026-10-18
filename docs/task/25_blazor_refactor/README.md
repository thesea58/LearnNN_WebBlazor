# ♻️ Task 25 – Blazor Architecture Refactor (theo mục 6 của `mobile-desktop-lock.md`)

> **Nhánh**: `web_mobile` · **Chạy SAU 24.01 (baseline PC), TRƯỚC 24.02**.
> **Rủi ro cao**: sửa toàn codebase nhưng PC phải giữ nguyên 100%. Mọi file refactor phải qua so ảnh baseline.
> **Prompt chạy**: [`AGENT_PROMPT.md`](AGENT_PROMPT.md) · **Nhật ký**: [`PROGRESS.md`](PROGRESS.md)

## Sub-task (mỗi nhóm = 1 commit riêng)
| ID | Nhóm | Phạm vi & tiêu chí | TT |
|---|---|---|---|
| 25.01 | **Kiểm kê vi phạm** | Quét toàn bộ `Source/Components/**` và `Source/Services/**`, lập bảng vi phạm (file, dòng, nhóm, mức rủi ro) vào `PROGRESS.md`; không sửa code | ✅ |
| 25.02 | **`@key` cho mọi `@foreach`** | Mọi vòng lặp render danh sách có `@key` duy nhất (Id); không đổi thứ tự/logic | ✅ |
| 25.03 | **Async an toàn** | Loại `.Result`/`.Wait()`/`async void` không cần thiết; dùng `await` | ⬜ |
| 25.04 | **Dispose & Event** | Component đăng ký event/Timer/`IJSObjectReference` triển khai `IDisposable`/`IAsyncDisposable`, hủy đăng ký | ⬜ |
| 25.05 | **JS Interop đúng chỗ** | JS chỉ trong `OnAfterRenderAsync(firstRender)`/handler; không trong `OnInitializedAsync`; không `onresize`→C# | ⬜ |
| 25.06 | **`[Parameter]` bất biến** | Không gán lại parameter trong component; dùng `EventCallback<T>`/biến cục bộ | ⬜ |
| 25.07 | **ErrorBoundary & Render** | Bọc vùng nội dung chính (MainLayout body) và widget độc lập; bỏ `StateHasChanged` dư; `ShouldRender` chỉ khi đo được lợi ích | ⬜ |
| 25.08 | **Virtualize (có điều kiện)** | Chỉ danh sách >50 dòng trên một trang không phân trang; `/words` giữ paging | ⬜ |
| 25.09 | **Code-behind & C# hiện đại (tùy chọn, rủi ro thấp)** | Tách `.razor.cs` cho component có `@code` > ~150 dòng; primary constructor cho DI; không đổi hành vi | ⬜ |

## Quy tắc an toàn
- Mỗi nhóm: refactor → `dotnet build` → `dotnet test` → so ảnh PC (1366×768, 1920×1080) → commit `refactor: [Task 25.XX] <subject>`.
- Fail so ảnh/test → revert phần đó (`git checkout -- <file>`), ghi `PROGRESS.md`, tối đa 3 lần/nhóm rồi `🚫`.
- Không đổi schema DB, không đổi hành vi nghiệp vụ, không đổi UI.
- Không đụng nhóm khác trong cùng commit (atomic).
