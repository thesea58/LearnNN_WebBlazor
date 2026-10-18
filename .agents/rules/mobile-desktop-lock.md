# MOBILE ADAPTATION & DESKTOP-LOCK RULE

> Áp dụng cho mọi thay đổi `.razor`, `.razor.css`, `app.css`, `study.css`, JS interop trên nhánh `web_mobile` và các nhánh sau nó.
> Phiên bản đã thống nhất với chủ dự án (xem `docs/task/24_web_mobile/` và `docs/task/25_blazor_refactor/`).

## 0. PRIME DIRECTIVE
Giao diện và **hành vi** trên PC (≥ 1024px) phải được bảo toàn. Không vỡ bố cục, lệch cột, đổi luồng thao tác của người dùng máy tính.
- **Ngoại lệ đã được duyệt**: (a) breakpoint layout đổi 768 → **1024px** (tablet dùng giao diện mobile); (b) guard chống double-submit trong handler (mục 4.6).
- Mọi thay đổi khác ảnh hưởng PC phải được chứng minh vô hại bằng so sánh screenshot (mục 5).

## 1. Breakpoints (thống nhất)
| Vùng | Điều kiện | Giao diện |
|---|---|---|
| Mobile + Tablet | `max-width: 1023.98px` | Bottom-nav, Card view mặc định, modal thu gọn |
| PC | `min-width: 1024px` | Sidebar cố định + topbar như hiện tại |
| Điện thoại hẹp | `max-width: 767.98px` | Tinh chỉnh thêm (font, padding, 1 cột) |
- Hỗ trợ đầy đủ ≥ **360px portrait**. 320px và landscape: chỉ cần không vỡ/không tràn ngang.
- CSS layout hiện tại viết kiểu `min-width: 768px` (PC nằm trong media query). Khi đổi lên 1024 chỉ được đổi **con số breakpoint** trong `MainLayout.razor.css` và `NavMenu.razor.css`; không đổi khai báo bên trong khối PC.

## 2. CSS không phá hủy (Non-Destructive)
1. Không xóa/đổi `display`, `width`, `padding`, `margin`, `grid-template-columns` của khối PC. Phần mobile thêm bằng `@media (max-width: …)` **ở cuối file**.
2. Ưu tiên CSS Isolation `*.razor.css` của chính component. Với component con/modal dùng `::deep`. Rule chung mới (bottom-nav, safe-area) mới được vào `app.css`; style học tập vào `study.css`.
3. `!important` chỉ được dùng cho (a) `font-size: 16px` của input/select/textarea trên mobile, (b) ghi đè kích thước modal của Bootstrap trên mobile.
4. Dùng `100dvh` (không `100vh`) trong khối mobile; thêm `env(safe-area-inset-*)` cho bottom-nav/topbar; meta viewport có `viewport-fit=cover`.
5. `:hover` chỉ đổi màu và phải nằm trong `@media (hover: hover) and (pointer: fine)`; không dùng hover để lộ nút quan trọng.

## 3. Bảo toàn Razor & Binding
- Không đổi cấu trúc `EditForm`, `EditContext`, `@bind-Value`, `@ref`, `@onclick` khi thêm wrapper/class.
- Không `display:none` component nặng; tách vùng hiển thị nhưng dùng chung dữ liệu nguồn (không gọi service 2 lần).

## 4. Quy chuẩn thành phần giao diện (mobile/tablet < 1024px)
1. **Điều hướng**: chỉ **bottom-nav** (không drawer). 5 mục: Home, Study, Words, Topics, AI (AI mở `/ai/inbox`; `/ai/settings` vào qua nút bánh răng trong trang AI). **Ẩn bottom-nav** trong `/study/flashcards`, `/study/match`, `/study/scramble`, `/study/quiz`; topbar có nút thoát rõ ràng. Nội dung không bị bottom-nav che (padding-bottom + safe-area).
2. **Danh sách Words/Topics**: mặc định **Card view** khi < 1024px nếu người dùng chưa chọn (vẫn cho đổi Table). Ở Table trên mobile áp mẫu `data-label`: giữ `<table>`, thêm `data-label` cho `<td>`; ≤ 767.98px ẩn `<thead>`, `<tr>` thành block, `<td>` flex `space-between`, `td::before { content: attr(data-label) }`. Cho phép giảm cỡ chữ, thu gọn khoảng trống để hiển thị nhiều từ.
3. **Modal** (`WordFormModal`, `ConfirmDeleteModal`, `AiManualBridgeModal`): `width: calc(100vw - 24px) !important; max-width: 100%; max-height: 90dvh; margin: 12px auto;`, header/footer **sticky**, phần thân cuộn; nút Lưu/Đóng luôn thấy khi bàn phím ảo mở (dùng `visualViewport` nếu cần).
4. **Flashcard**: tap lật + 4 nút SRS dính đáy. Swipe là tùy chọn trong cài đặt, **mặc định tắt**.
5. **Input**: `font-size: 16px !important` (≤ 767.98px) cho `input/select/textarea`; dùng `inputmode`/`type` đúng (`numeric`, `tel`, `email`) khi có ô tương ứng.
6. **Touch target** ≥ 44×44px, khoảng cách liền kề ≥ 8px cho mọi button/icon/phân trang. **Double-submit**: mọi nút gọi `async Task` lưu/xóa/import/AI/chấm SRS phải có `isBusy` + `disabled="@isBusy"` **và** bỏ qua lời gọi thứ hai trong handler (`if (isBusy) return;`) vì `disabled` chỉ có hiệu lực sau khi render về client.

## 5. Tự kiểm thử (bắt buộc trước khi kết luận xong)
Dùng Playwright, 3 cấu hình: **Chromium desktop**, **Chromium Pixel 7**, **WebKit iPhone SE (375×667) / iPhone 14 (390×844)**. Thêm kiểm tra thủ công trên điện thoại thật qua `start-mobile.bat` (do chủ dự án thực hiện).

**Bước 1 – Hồi quy PC**: so sánh tự động (`toHaveScreenshot`, sai lệch ≤ ~0.1%) với **baseline chụp TRƯỚC khi sửa** ở 1366×768 và 1920×1080 cho mọi route. Sidebar/Header/Toolbar giữ vị trí, bảng đủ cột, không có lỗi console mới.

**Bước 2 – Mobile**: ở 360×640, 375×667, 390×844 (và 768×1024 cho tablet):
- Không tràn ngang: dùng script dưới.
- Bottom-nav hiện/ẩn đúng theo route; modal có nút Lưu/Đóng bấm được; touch target ≥ 44px; không lỗi console.

```js
(() => {
  const doc = document.documentElement, w = doc.clientWidth, offenders = [];
  const skip = el => el.closest('[aria-hidden="true"], .modal:not(.show)') ||
    getComputedStyle(el).position === 'fixed' ||
    [...(function*(n){ while ((n = n.parentElement)) yield n; })(el)]
      .some(p => ['auto','scroll'].includes(getComputedStyle(p).overflowX) && p.scrollWidth > p.clientWidth);
  document.querySelectorAll('body *').forEach(el => {
    const r = el.getBoundingClientRect();
    if ((r.right > w + 1 || r.left < -1) && r.width > 0 && !skip(el))
      offenders.push({ tag: el.tagName, cls: String(el.className), overflowPx: Math.round(r.right - w) });
  });
  return { isOverflowing: doc.scrollWidth > w + 1, offenders };
})();
```
Đạt khi `isOverflowing === false` và `offenders` rỗng. Nếu fail: thêm `max-width:100%; box-sizing:border-box; flex-wrap:wrap` trong media query mobile.

## 6. Kiến trúc Blazor & hiệu năng (áp dụng toàn codebase – thực hiện ở Task 25)
- Presentational vs Container; không sửa `[Parameter]` trong chính component, dùng `EventCallback<T>`.
- Async chuẩn, **cấm** `.Result`/`.Wait()`.
- Mọi `@foreach` render danh sách phải có `@key` duy nhất. `<Virtualize>` chỉ dùng khi một trang hiển thị > 50 dòng; danh sách đã phân trang server-side thì giữ paging.
- Tránh `StateHasChanged()` dư; `ShouldRender()` cho component nặng.
- Bọc vùng chính/widget độc lập trong `<ErrorBoundary>`.
- Component đăng ký event (`LocationChanged`, Timer, state event) hoặc giữ `IJSObjectReference` phải triển khai `IDisposable`/`IAsyncDisposable` và hủy đăng ký.
- Chỉ gọi JS trong `OnAfterRenderAsync(firstRender)` hoặc event handler; **không** trong `OnInitializedAsync` (prerender).
- **Cấm** `window.onresize` gọi về C#; nếu cần dùng `matchMedia('(max-width: 1023.98px)')` + `change` listener.
- Primary constructor, code-behind `.razor.cs`, pattern matching: dùng cho code mới/đang sửa; chỉ refactor hàng loạt trong Task 25 và phải qua so sánh screenshot PC.
