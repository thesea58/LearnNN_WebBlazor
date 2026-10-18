# 🤖 AGENT_PROMPT – Task 25 (Blazor Refactor, PC-safe)

> Chạy ở gốc repo, nhánh `web_mobile`, **sau khi 24.01 (baseline PC) = ✅**.

## PROMPT

Bạn là Orchestrator. Hoàn thành mọi sub-task trong `docs/task/25_blazor_refactor/README.md` theo vòng lặp **Sửa → Test → So ảnh PC → Ghi log → Commit**, không hỏi người dùng trừ điều kiện dừng.

### Khởi động
1. Nhánh phải là `web_mobile`. Đọc `AGENTS.md`, mọi `.agents/rules/*.md` (đặc biệt `mobile-desktop-lock.md` mục 0, 5, 6), `docs/PROJECT_MAP.md`, README + PROGRESS của Task 25.
2. Kiểm tra Task 24.01 = ✅ và có baseline ở `Tests/LearnNN_WebBlazor.Tests/Baselines/`. Nếu chưa → DỪNG và báo người dùng.
3. `dotnet build` + `dotnet test` baseline phải xanh trước khi sửa.

### Mỗi vòng
1. Lấy sub-task `🔄`/`⬜` đầu tiên (25.01 → 25.09). Đánh `🔄`.
2. Giao việc bằng `invoke_subagent`:
   - `research`: liệt kê chính xác file/dòng vi phạm của nhóm hiện tại (chỉ đọc).
   - `self` (Developer): sửa tối thiểu, **không đổi hành vi/giao diện**, chỉ nhóm hiện tại.
   - `self` (Verifier, tách biệt): chạy build/test/so ảnh, không sửa code production.
   - `self` (Reviewer): xem diff có lệch khỏi nhóm, có đổi logic/markup nhìn thấy không; PASS/FAIL.
3. Orchestrator tự chạy: `dotnet build LearnNN_WebBlazor.slnx`, `dotnet test LearnNN_WebBlazor.slnx` (gồm so ảnh PC 1366×768 & 1920×1080, app chạy nền ở `http://localhost:5099`, tắt sau khi xong).
4. Xanh + Reviewer PASS → cập nhật README/PROGRESS, `git add` đúng file, commit `refactor: [Task 25.XX] <subject>` (không add `*.db`, `bin/`, `obj/`). Đỏ → `git checkout -- <file>` phần lỗi, ghi PROGRESS, thử lại (tối đa 3 lần/nhóm rồi `🚫`).
5. 25.01 chỉ lập bảng vi phạm, không sửa code.

### Cấm
Đổi UI/CSS, schema DB, logic nghiệp vụ; `Source/*.db`; sửa test/baseline để "cho pass" (nếu ảnh khác, phải tìm nguyên nhân hoặc revert).

### Dừng
- Xong: tất cả `✅`/`🚫` → cập nhật `docs/task/README.md`, báo cáo tổng kết (số vi phạm đã sửa, commit, `🚫`).
- Khẩn: thiếu baseline, ≥3 nhóm liên tiếp `🚫`, hoặc test hồi quy PC không thể làm xanh.
