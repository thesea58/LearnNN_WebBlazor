# 🤖 AGENT_PROMPT – Vòng lặp Sửa & Test liên tục cho Task 24 (Web Mobile)

> **Cách dùng (Agy CLI)**: chạy ở thư mục gốc repo, trên nhánh `web_mobile`, rồi dán toàn bộ khối **PROMPT** bên dưới
> (hoặc: `agy "Đọc và thực thi docs/task/24_web_mobile/AGENT_PROMPT.md, phần PROMPT"`).
> Khuyến nghị dùng cùng `/goal` để agent không dừng sớm.

### R. Giao thức Resume & chống mất tiến độ (hết token / hết hạn mức / bị ngắt)
Mọi lần chạy (kể cả lần đầu) PHẢI bắt đầu bằng bước này:
1. Đọc `PROGRESS.md` → xác định sub-task `🔄` và checkpoint cuối.
2. `git status --short`: nếu có thay đổi chưa commit (trừ `*.db`, `bin/`, `obj/`) thuộc sub-task `🔄` → chạy `dotnet build`, `dotnet test`, so ảnh PC. **Xanh** → tiếp tục/commit; **đỏ hoặc không rõ nguồn gốc** → `git stash push -m "recovered-<sub-task>"` (hoặc `git checkout -- <file>`), ghi vào `PROGRESS.md`, làm lại sub-task từ đầu.
3. Dọn tiến trình nền sót: dừng mọi `dotnet`/`node` đang giữ cổng 5099 trước khi chạy app mới (`Get-NetTCPConnection -LocalPort 5099` → `Stop-Process`).
4. Sub-agent chưa trả kết quả ở lần chạy trước coi như mất: giao lại.

**Checkpoint bắt buộc**: ghi `PROGRESS.md` (a) ở ĐẦU vòng – "đang làm 24.XX, dự kiến sửa file A, B"; (b) sau mỗi bước lớn (Developer xong, Tester xong); (c) cuối vòng.
**Ngưỡng chủ động**: khi ngữ cảnh ~70% → hoàn tất hoặc revert vòng hiện tại, cập nhật `PROGRESS.md`, chạy `/compact`, rồi tiếp tục từ `PROGRESS.md`. Nếu sắp hết token mà chưa xong: commit `wip(mobile): [Task 24.XX] checkpoint` (chỉ khi build xanh) hoặc revert; KHÔNG để code dở không ghi nhận. Commit WIP được squash/ghi chú khi sub-task xong.
**Tự chạy lại khi hết hạn mức**: người dùng dùng `/schedule` (cron, vd mỗi 30–60 phút) với prompt: *"Đọc docs/task/24_web_mobile/AGENT_PROMPT.md và thực thi PROMPT theo Giao thức Resume; nếu tất cả sub-task đã ✅/🚫 thì không làm gì."*

---

## PROMPT

Bạn là **Orchestrator** của dự án LearnNN_WebBlazor (Blazor Server .NET 10, SQLite). Nhiệm vụ: hoàn thành **toàn bộ** sub-task trong `docs/task/24_web_mobile/README.md` bằng vòng lặp **Sửa → Test → Ghi log → Commit** liên tục, KHÔNG dừng để hỏi người dùng trừ khi gặp điều kiện dừng (mục 6).

### 0. Khởi động (chỉ 1 lần)
1. `git branch --show-current` phải là `web_mobile`; nếu không, `git checkout web_mobile` (tạo mới nếu chưa có).
2. Đọc BẮT BUỘC: `AGENTS.md`, `.agents/rules/*.md` (đặc biệt **`mobile-desktop-lock.md`** – khóa PC ≥1024px, breakpoint 1024, chỉ bottom-nav), `docs/PROJECT_MAP.md`, `docs/task/24_web_mobile/README.md`, `docs/task/24_web_mobile/PROGRESS.md`.
   **Thứ tự bắt buộc**: 24.01 (harness + baseline PC) → chạy Task 25 (`docs/task/25_blazor_refactor/AGENT_PROMPT.md`) → 24.02…24.08 → 24.09…24.13. Không làm 24.02+ khi Task 25 chưa xong (trừ khi người dùng cho phép).
3. `git status`: nếu có thay đổi không thuộc task (vd. `Source/*.db`, `bin/`, `obj/`), KHÔNG commit chúng; thêm vào `.gitignore` nếu hợp lý.
4. Xác minh `dotnet build` chạy được trước khi bắt đầu (baseline).

### 1. Vòng lặp chính (lặp đến khi mọi sub-task = ✅)
Mỗi vòng:
1. **Chọn việc**: đọc `PROGRESS.md`; lấy sub-task `🔄` đang dở, nếu không có thì lấy sub-task `⬜` đầu tiên có đủ phụ thuộc đã ✅. Đánh dấu `🔄` trong README.
2. **Phân rã & giao việc cho sub-agents** (dùng `invoke_subagent`; chạy song song khi độc lập):
   - 🔎 **`research`** – khảo sát code liên quan (file Razor/CSS/JS/Service), trả về danh sách file cần sửa + rủi ro. Chỉ đọc.
   - 🛠️ **`self` (Developer)** – triển khai thay đổi nhỏ, đúng layering của `PROJECT_MAP.md`; mỗi sub-task tối đa ~300 dòng diff/vòng, nếu lớn hơn thì chia nhiều vòng.
   - 🧪 **`self` (Tester)** – viết/chạy test độc lập với Developer (không sửa code production): Playwright theo viewport chuẩn trong README, bUnit/unit test cho logic, kiểm tra console error, overflow ngang, touch target ≥44px, chụp screenshot lưu vào `docs/task/24_web_mobile/evidence/<id>/` (tên `<route>_<viewport>.png`).
   - 🧐 **`self` (Reviewer)** – review diff theo `code-commenting.md` (EN, XML Docs, #region), kiểm tra hồi quy desktop, bảo mật, không phá hành vi hiện có. Trả về PASS/FAIL + danh sách lỗi.
3. **Chạy kiểm chứng** (Orchestrator tự chạy, không tin báo cáo của sub-agent):
   ```powershell
   dotnet build LearnNN_WebBlazor.slnx
   dotnet test  LearnNN_WebBlazor.slnx
   ```
   rồi chạy bộ Playwright mobile (khởi chạy app nền với `dotnet run --project Source --urls http://localhost:5099`, dừng app sau khi test xong).
4. **Quyết định**:
   - ✅ Tất cả xanh + Reviewer PASS + đạt Acceptance Criteria → sang bước 5.
   - ❌ Có lỗi → ghi vào *Bug Backlog* của `PROGRESS.md`, tăng bộ đếm thất bại, quay lại bước 2 với danh sách lỗi cụ thể (Developer sửa → Tester chạy lại). **Tối đa 5 vòng sửa cho 1 sub-task**; vượt ngưỡng → đánh `🚫`, ghi nguyên nhân gốc + đề xuất, chuyển sub-task kế tiếp.
5. **Hoàn tất sub-task**:
   - Cập nhật README: `✅` cho sub-task; cập nhật `PROGRESS.md` (bảng nhật ký + reset bộ đếm).
   - Có file mới/đổi cấu trúc → cập nhật cây thư mục & thêm 1 dòng Change Log trong `docs/PROJECT_MAP.md`.
   - **Commit atomic** (chỉ `git add` đúng file liên quan; không add `*.db`, `bin/`, `obj/`):
     `git commit -m "feat(mobile): [Task 24.XX] <imperative subject>"` (loại `fix`/`test`/`docs`/`chore`/`perf` khi phù hợp, theo `git-commit-convention.md`).
6. Quay lại bước 1 của vòng kế tiếp.

### 2. Quy tắc kỹ thuật cố định
- **Desktop-lock, KHÔNG mobile-first**: giữ nguyên khối PC; phần mobile/tablet thêm bằng `@media (max-width: 1023.98px)` / `(max-width: 767.98px)` ở **cuối file**. Breakpoint layout = **1024px** (chỉ đổi con số trong `MainLayout.razor.css`/`NavMenu.razor.css`). Chi tiết: `.agents/rules/mobile-desktop-lock.md`.
- Điều hướng mobile = **bottom-nav** (không drawer), ẩn trong 4 trang phiên học.
- CSS: ưu tiên `.razor.css` (dùng `::deep` cho component con); rule chung mới vào `app.css`/`study.css`. `!important` chỉ cho input 16px và kích thước modal mobile.
- Không thêm thư viện nặng khi CSS/JS thuần làm được. Package test (Playwright, bUnit) chỉ trong `Tests/`.
- Project test: `Tests/LearnNN_WebBlazor.Tests/`, add vào `LearnNN_WebBlazor.slnx`; cập nhật `PROJECT_MAP.md`.
- Test không dùng DB thật: SQLite file tạm + seed riêng. TUYỆT ĐỐI không sửa/commit `Source/LearnNN_VocabDB.db`.
- **Điều kiện chặn**: so ảnh baseline PC (1366×768, 1920×1080) phải khớp sau MỖI sub-task. Không được cập nhật baseline để "cho pass" trừ ngoại lệ đã duyệt (breakpoint 768→1024 ở vùng 768–1023px; guard double-submit không đổi UI).
- Không đổi schema DB trừ khi bắt buộc (ghi rõ trong log). Không hard-code secret/API key; không đụng cơ chế nạp key AI.
- Comment code theo `.agents/rules/code-commenting.md`.

### 3. Chuẩn "Tester" tối thiểu cho mỗi sub-task
Ma trận engine/viewport: xem README Task 24 (Chromium desktop, WebKit iPhone SE/14, Chromium Pixel 7, biên 360/320/768/landscape).

| Kiểm tra | Cách đo |
|---|---|
| Hồi quy PC | `toHaveScreenshot` (≤0.1%) mọi route ở 1366×768 & 1920×1080 so với baseline |
| Không tràn ngang | Script overflow trong `mobile-desktop-lock.md` mục 5 (`isOverflowing=false`, `offenders` rỗng) ở 360/375/390/412/768 (320 & landscape: chỉ không vỡ) |
| Touch target | Mọi `a, button, input, select, [role=button]` hiển thị trên mobile có `boundingBox` ≥ 44×44, gap ≥ 8px (loại link inline trong đoạn văn) |
| Bottom-nav | Hiện <1024px, ẩn ≥1024px; ẩn ở `/study/flashcards,match,scramble,quiz`; không che nội dung |
| Modal | Lưu/Đóng nằm trong viewport & bấm được ở 375×667 (kể cả khi mở bàn phím giả lập) |
| Double-submit | Bấm đúp nhanh nút lưu/xóa chỉ gọi service 1 lần |
| Console sạch | Không `console.error`/`pageerror`/request 4xx-5xx |
| Blazor sẵn sàng | Chờ circuit kết nối trước khi thao tác (không sleep cứng) |
| Lighthouse (24.09, 24.11, 24.12) | `npx lighthouse <url> --form-factor=mobile` hoặc chrome-devtools MCP; lưu tóm tắt vào `evidence/` |

Routes cần phủ: `/`, `/words`, `/topics`, `/study`, `/study/flashcards`, `/study/match`, `/study/scramble`, `/study/quiz`, `/ai/inbox`, `/ai/settings`.


### 4. Quy tắc làm việc của sub-agents
- Mỗi sub-agent nhận prompt tự chứa: mục tiêu, file liên quan, Acceptance Criteria của sub-task, định dạng kết quả trả về (danh sách file đổi / lệnh đã chạy / kết quả / lỗi còn lại).
- Developer và Tester là **hai agent khác nhau**; Tester không được "sửa test cho pass" mà không giải thích; test chỉ được nới lỏng khi tiêu chí sai thực sự, và phải ghi lý do vào `PROGRESS.md`.
- Sub-agent không commit; chỉ Orchestrator commit.

### 5. Ghi nhận trạng thái (chống mất ngữ cảnh)
Cuối MỖI vòng, cập nhật `PROGRESS.md`: sub-task hiện tại, số vòng, kết quả test (pass/fail + số lượng), lỗi còn mở, hash commit. Nếu ngữ cảnh dài, dùng `/compact` rồi tiếp tục từ `PROGRESS.md`.

### 6. Điều kiện dừng
- **Thành công**: tất cả sub-task `✅` (hoặc `🚫` có lý do) → chạy 24.13 lần cuối, cập nhật `docs/task/README.md` (thêm dòng Task 24 với trạng thái), báo cáo tổng kết (bảng sub-task, số test, điểm Lighthouse, danh sách `🚫`).
- **Dừng khẩn & báo người dùng**: cần quyết định có rủi ro (xóa dữ liệu, đổi schema phá dữ liệu, thêm dịch vụ trả phí), thiếu quyền/công cụ không thể cài (vd. không tải được trình duyệt Playwright sau 2 lần thử), hoặc ≥3 sub-task liên tiếp `🚫`.
- Không dừng chỉ vì một lỗi đơn lẻ — phân tích nguyên nhân gốc, sửa và chạy lại.

### 7. Báo cáo cuối
Tóm tắt ngắn: sub-task ✅/🚫, danh sách commit, kết quả test cuối, điểm Lighthouse mobile, việc còn tồn đọng, hướng dẫn kiểm tra thủ công trên điện thoại (`start-mobile.bat`).

---

## Gợi ý chạy
```powershell
git checkout web_mobile
agy   # rồi dán khối PROMPT, hoặc dùng /goal kèm nội dung PROMPT
```
