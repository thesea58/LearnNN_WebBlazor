# 📱 Task 24 – Web Mobile (Responsive + PWA + Mobile UX)

> **Nhánh Git**: `web_mobile`
> **Mục tiêu**: Biến LearnNN thành web dùng mượt trên điện thoại (360–430px), cài được như app (PWA), thao tác cảm ứng tốt, chịu được mạng chập chờn (Blazor Server).
> **Cách chạy**: Agy CLI đọc [`AGENT_PROMPT.md`](AGENT_PROMPT.md) và lặp chu trình Sửa → Test → Ghi log → Commit cho từng sub-task bên dưới.
> **Nhật ký thực thi**: [`PROGRESS.md`](PROGRESS.md) (agent ghi mỗi vòng lặp).

## Quy ước trạng thái
`⬜` chưa làm · `🔄` đang làm · `✅` xong (đã qua test) · `🚫` bị chặn (ghi lý do vào PROGRESS.md)

## Viewport kiểm thử chuẩn
| Tên | Kích thước | Ghi chú |
|---|---|---|
| iPhone SE | 375×667 | Màn nhỏ nhất cần hỗ trợ (cũng kiểm tra 360×640 Android) |
| iPhone 14 | 390×844 | Có safe-area / notch |
| Pixel 7 | 412×915 | Android phổ biến |
| iPad Mini | 768×1024 | Breakpoint tablet |
| Desktop | 1280×800 | Không được hồi quy |

## Danh sách Sub-task

| ID | Sub-task | Tiêu chí hoàn thành (Acceptance Criteria) | Ưu tiên | Phụ thuộc | TT |
|---|---|---|---|---|---|
| 24.01 | **Baseline Audit & Test Harness** | Có project test (`Tests/`) dùng Playwright .NET chạy được headless; script chụp screenshot mọi route ở 5 viewport; báo cáo lỗi tràn ngang/overflow/touch target <44px vào `PROGRESS.md` | 🔴 | — | ⬜ |
| 24.02 | **Viewport, Safe-area & Global CSS** | `<meta viewport viewport-fit=cover>`, `env(safe-area-inset-*)`, không có scroll ngang ở bất kỳ route nào ≤ 430px, font input ≥16px (chặn iOS zoom), `touch-action` hợp lý | 🔴 | 24.01 | ⬜ |
| 24.03 | **Mobile Navigation** | Dưới 768px: thanh bottom-nav (Home, Study, Words, Topics, AI) cố định, active state rõ; NavMenu cũ ẩn; không che nội dung cuối trang; desktop giữ nguyên sidebar | 🔴 | 24.02 | ⬜ |
| 24.04 | **Words & Topics mobile** | `/words`, `/topics`: filter thu gọn (collapse/offcanvas), mặc định Card view trên mobile, modal thêm/sửa full-screen, nút ≥44px, phân trang gọn | 🔴 | 24.02, 24.03 | ⬜ |
| 24.05 | **Study Hub & Flashcard touch** | `/study/*`: flashcard hỗ trợ tap lật + swipe trái/phải (đánh giá SRS), 4 nút SRS dính đáy màn hình trong tầm ngón cái, game Match/Scramble/Quiz không tràn màn 360px | 🔴 | 24.02, 24.03 | ⬜ |
| 24.06 | **AI pages mobile** | `/ai/inbox`, `/ai/settings`, `AiManualBridgeModal`: modal full-screen, nút Copy 1 chạm hiển thị rõ, textarea không bị bàn phím che (visualViewport) | 🟡 | 24.02 | ⬜ |
| 24.07 | **PWA (Installable)** | `manifest.webmanifest`, icon 192/512 + maskable, `apple-touch-icon`, theme-color, service worker cache static assets; Lighthouse PWA installable pass; Add-to-Home-Screen chạy fullscreen | 🔴 | 24.02 | ⬜ |
| 24.08 | **Resilience Blazor Server trên mobile** | Reconnect modal thân thiện mobile (không chặn UI vô hạn), tự reconnect khi quay lại tab/đổi mạng, trang offline fallback từ service worker, không mất state quan trọng (phiên học) khi mất kết nối ngắn | 🟡 | 24.07 | ⬜ |
| 24.09 | **Performance mobile** | Lighthouse Mobile (throttled 4G): Performance ≥ 85, LCP < 2.5s, CLS < 0.1 trên `/`, `/words`, `/study`; bật nén Brotli/Gzip, cache header static, lazy-load phần nặng | 🟡 | 24.02 | ⬜ |
| 24.10 | **Accessibility & Touch QA** | Lighthouse A11y ≥ 90, contrast AA, focus-visible, aria-label cho nút icon, mọi tap target ≥ 44×44px (test tự động bằng Playwright) | 🟡 | 24.04, 24.05 | ⬜ |
| 24.11 | **Regression & Docs & Release** | Toàn bộ test xanh ở 5 viewport; desktop không hồi quy; cập nhật `README.md`, `docs/design/04_deployment_guide.md` (mục PWA), `PROJECT_MAP.md`, `docs/task/README.md`; merge-ready | 🔴 | tất cả | ⬜ |

## Definition of Done (áp dụng cho MỌI sub-task)
1. `dotnet build` không lỗi, không warning mới.
2. `dotnet test` xanh (gồm test mới của sub-task).
3. Playwright: không có overflow ngang và không có lỗi console ở viewport liên quan.
4. Desktop 1280×800 không hồi quy.
5. Đã cập nhật checklist ở bảng trên + ghi `PROGRESS.md`.
6. Đã commit theo quy ước: `feat(mobile): [Task 24.XX] <subject>`.
7. Có file mới/đổi cấu trúc → cập nhật `docs/PROJECT_MAP.md` (cây + Change Log).
