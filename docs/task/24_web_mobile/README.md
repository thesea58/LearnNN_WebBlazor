# 📱 Task 24 – Web Mobile (Responsive + PWA + Mobile UX)

> **Nhánh Git**: `web_mobile`
> **Rule bắt buộc**: [`.agents/rules/mobile-desktop-lock.md`](../../../.agents/rules/mobile-desktop-lock.md) (PC ≥1024px được khóa; breakpoint layout = 1024px; chỉ bottom-nav, không drawer).
> **Cách chạy**: Agy CLI đọc [`AGENT_PROMPT.md`](AGENT_PROMPT.md) và lặp Sửa → Test → Ghi log → Commit.
> **Nhật ký**: [`PROGRESS.md`](PROGRESS.md)

## Thứ tự thực thi (QUAN TRỌNG)
```
24.01 (harness + baseline PC)  →  Task 25 (refactor Blazor, có baseline bảo vệ)  →  24.02 … 24.08 (Pha A: responsive)  →  24.09 … 24.13 (Pha B)
```
Agent KHÔNG được làm 24.02+ trước khi Task 25 hoàn thành (hoặc chủ dự án cho phép bỏ qua).

## Quy ước trạng thái
`⬜` chưa làm · `🔄` đang làm · `✅` xong (đã qua test) · `🚫` bị chặn (ghi lý do vào PROGRESS.md)

## Ma trận kiểm thử chuẩn
| Engine | Thiết bị / Viewport | Mục đích |
|---|---|---|
| Chromium desktop | 1366×768, 1920×1080 | **Hồi quy PC (so ảnh baseline)** |
| WebKit (iPhone) | 375×667 (SE), 390×844 (14) | Safari iOS: auto-zoom, dvh, safe-area |
| Chromium (Pixel 7) | 412×915 | Android |
| Chromium | 360×640 (chuẩn tối thiểu), 320×568 (chỉ không vỡ), 768×1024 (tablet = giao diện mobile), landscape 667×375 (chỉ không vỡ) | Biên |

## Pha A – Responsive

| ID | Sub-task | Tiêu chí hoàn thành | Phụ thuộc | TT |
|---|---|---|---|---|
| 24.01 | **Test harness & Baseline PC** | Project `Tests/LearnNN_WebBlazor.Tests/` (Playwright .NET, 3 engine), DB test riêng (SQLite tạm + seed), screenshot baseline mọi route ở 1366×768 & 1920×1080 lưu `Tests/.../Baselines/`; test so ảnh `toHaveScreenshot` (≤0.1%); script overflow chuẩn trong rule; chạy được headless | — | ✅ |
| 24.02 | **Breakpoint 1024 + Viewport/Safe-area** | `MainLayout.razor.css`/`NavMenu.razor.css` đổi 768→1024 (chỉ số, giữ nguyên khối PC); `viewport-fit=cover`; `100dvh`; safe-area; không scroll ngang ở ≤1023px; PC baseline khớp ở 1366/1920 | 24.01, Task 25 | ✅ |
| 24.03 | **Bottom-nav** | 5 mục (Home, Study, Words, Topics, AI→Inbox), active state, ẩn ở `/study/flashcards,match,scramble,quiz` kèm nút thoát ở topbar; nút ⚙ Settings trong trang AI; nội dung không bị che; ẩn hoàn toàn ≥1024px; sidebar PC giữ nguyên | 24.02 | ✅ |
| 24.04 | **Words & Topics mobile** | <1024px mặc định Card view nếu chưa lưu lựa chọn (vẫn đổi được Table); Table dùng `data-label` (thead ẩn ≤767.98px); chữ nhỏ/gọn để hiển thị nhiều từ; filter thu gọn; phân trang ≥44px; PC không đổi | 24.03 | ✅ |
| 24.05 | **Study mobile & Flashcard touch** | Tap lật + 4 nút SRS dính đáy trong tầm ngón cái; swipe là tùy chọn (mặc định TẮT) trong cài đặt; Match/Scramble/Quiz không tràn ở 360px | 24.03 | ✅ |
| 24.06 | **Modals & AI pages mobile** | `WordFormModal`, `ConfirmDeleteModal`, `AiManualBridgeModal`: `calc(100vw-24px)`, `max-height:90dvh`, header/footer sticky, Lưu/Đóng thấy được khi bàn phím mở (visualViewport nếu cần); `/ai/inbox`, `/ai/settings` không tràn | 24.02 | 🔄 |
| 24.07 | **Chống double-submit** | Mọi nút async lưu/xóa/import/AI/chấm SRS có `isBusy` + `disabled` + guard đầu handler; test bấm đúp nhanh chỉ gọi service 1 lần | 24.04–24.06 | ⬜ |
| 24.08 | **Công thái học input & hover** | Input ≤767.98px `font-size:16px`; `inputmode`/`type` đúng; touch target ≥44px & gap ≥8px (test tự động); `:hover` bọc `(hover:hover) and (pointer:fine)` | 24.04–24.06 | ⬜ |

## Pha B – Nâng cao (làm sau Pha A)

| ID | Sub-task | Tiêu chí hoàn thành | Phụ thuộc | TT |
|---|---|---|---|---|
| 24.09 | **PWA** | `manifest.webmanifest`, icon 192/512 + maskable, `apple-touch-icon`, theme-color, service worker cache static; Lighthouse installable pass | Pha A | ⬜ |
| 24.10 | **Resilience Blazor Server** | Reconnect modal thân thiện mobile, tự reconnect khi quay lại tab/đổi mạng, trang offline fallback, không mất phiên học khi mất kết nối ngắn | 24.09 | ⬜ |
| 24.11 | **Performance mobile** | Lighthouse Mobile Perf ≥85, LCP<2.5s, CLS<0.1 trên `/`, `/words`, `/study`; Brotli/Gzip, cache static | Pha A | ⬜ |
| 24.12 | **Accessibility** | Lighthouse A11y ≥90, contrast AA, focus-visible, aria-label nút icon | Pha A | ⬜ |
| 24.13 | **Regression, Docs & Release** | Toàn bộ test xanh ở ma trận; PC baseline khớp; cập nhật `README.md`, `04_deployment_guide.md`, `PROJECT_MAP.md`, `docs/task/README.md`; checklist thử tay điện thoại thật (`start-mobile.bat`) giao cho chủ dự án | tất cả | ⬜ |

## Definition of Done (mọi sub-task)
1. `dotnet build` không lỗi/warning mới; `dotnet test` xanh.
2. **So ảnh PC baseline khớp** (1366×768 & 1920×1080) – điều kiện chặn.
3. Mobile: không tràn ngang, không lỗi console, touch target ≥44px ở viewport liên quan (WebKit + Chromium).
4. Cập nhật bảng này + `PROGRESS.md`.
5. Commit: `feat(mobile): [Task 24.XX] <subject>`.
6. File mới/đổi cấu trúc → cập nhật `docs/PROJECT_MAP.md` (cây + Change Log).
