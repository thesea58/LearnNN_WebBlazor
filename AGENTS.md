# AGENTS.md – LearnNN_WebBlazor

Blazor Server (.NET 10) + SQLite. Agent PHẢI đọc các file sau trước khi làm việc:

1. [`docs/PROJECT_MAP.md`](docs/PROJECT_MAP.md) – kiến trúc, nơi đặt file, Change Log (cập nhật sau mỗi thay đổi cấu trúc).
2. [`.agents/rules/project-architecture.md`](.agents/rules/project-architecture.md) – layering & file conventions.
3. [`.agents/rules/mobile-desktop-lock.md`](.agents/rules/mobile-desktop-lock.md) – **bảo toàn giao diện/hành vi PC (≥1024px)**, quy chuẩn mobile, quy trình test Playwright.
4. [`.agents/rules/code-commenting.md`](.agents/rules/code-commenting.md) – comment code.
5. [`.agents/rules/git-commit-convention.md`](.agents/rules/git-commit-convention.md) – commit.
6. [`.agents/rules/update-status-task.md`](.agents/rules/update-status-task.md) – cập nhật `docs/task/README.md`.

Quy tắc bất biến: không commit `Source/*.db`, `bin/`, `obj/`; không hard-code secret; test không dùng DB thật của người dùng.
Task đang chạy: `docs/task/24_web_mobile/` (kèm `AGENT_PROMPT.md`) và `docs/task/25_blazor_refactor/`.
