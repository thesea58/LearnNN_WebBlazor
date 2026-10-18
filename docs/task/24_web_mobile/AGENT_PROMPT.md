# 🤖 AGENT_PROMPT – Vòng lặp Sửa & Test liên tục cho Task 24 (Web Mobile)

> **Cách dùng (Agy CLI)**: chạy ở thư mục gốc repo, trên nhánh `web_mobile`, rồi dán toàn bộ khối **PROMPT** bên dưới
> (hoặc: `agy "Đọc và thực thi docs/task/24_web_mobile/AGENT_PROMPT.md, phần PROMPT"`).
> Khuyến nghị dùng cùng `/goal` để agent không dừng sớm.

---

## PROMPT

Bạn là **Orchestrator** của dự án LearnNN_WebBlazor (Blazor Server .NET 10, SQLite). Nhiệm vụ: hoàn thành **toàn bộ** sub-task trong `docs/task/24_web_mobile/README.md` bằng vòng lặp **Sửa → Test → Ghi log → Commit** liên tục, KHÔNG dừng để hỏi người dùng trừ khi gặp điều kiện dừng (mục 6).

### 0. Khởi động (chỉ 1 lần)
1. `git branch --show-current` phải là `web_mobile`; nếu không, `git checkout web_mobile` (tạo mới nếu chưa có).
2. Đọc BẮT BUỘC: `.agents/rules/*.md`, `docs/PROJECT_MAP.md`, `docs/task/24_web_mobile/README.md`, `docs/task/24_web_mobile/PROGRESS.md`.
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
- Mobile-first: viết CSS cho ≤430px trước, dùng breakpoint Bootstrap (`<768px` = mobile). Ưu tiên scoped CSS `.razor.css`; CSS dùng chung vào `Source/wwwroot/app.css` / `study.css`.
- Không thêm thư viện nặng khi CSS/JS thuần làm được. Package mới (Playwright, bUnit) chỉ đặt trong project `Tests/`, không vào project web.
- Project test mới: `Tests/LearnNN_WebBlazor.Tests/`, add vào `LearnNN_WebBlazor.slnx`; phải cập nhật `PROJECT_MAP.md`.
- Test không được phụ thuộc DB thật của người dùng: dùng SQLite file tạm/in-memory và seed riêng. TUYỆT ĐỐI không sửa/commit `Source/LearnNN_VocabDB.db`.
- Không phá tính năng desktop: mỗi sub-task phải chạy lại smoke test ở 1280×800.
- Không làm thay đổi schema DB trừ khi bắt buộc; nếu có, tạo migration và ghi rõ trong log.
- Không hard-code secret/API key. Không đụng cơ chế nạp key AI hiện có.
- Comment code theo `.agents/rules/code-commenting.md`.

### 3. Chuẩn "Tester" tối thiểu cho mỗi sub-task
| Kiểm tra | Cách đo |
|---|---|
| Không scroll ngang | `document.documentElement.scrollWidth <= window.innerWidth` ở từng route × viewport |
| Touch target | Mọi `a, button, input, select, [role=button]` hiển thị có `boundingBox` ≥ 44×44 (loại trừ link inline trong đoạn văn) |
| Console sạch | Không có `console.error` / `pageerror` / request 4xx-5xx |
| Blazor hoạt động | Chờ `blazor` circuit kết nối trước khi thao tác (không dùng sleep cứng) |
| Hồi quy desktop | Smoke: `/`, `/words`, `/topics`, `/study`, `/ai/inbox` tải OK ở 1280×800 |
| Lighthouse (24.07, 24.09, 24.10) | `npx lighthouse <url> --form-factor=mobile --only-categories=...` hoặc chrome-devtools MCP; lưu JSON/tóm tắt vào `evidence/` |

Routes cần phủ: `/`, `/words`, `/topics`, `/study`, `/study/flashcards`, `/study/match`, `/study/scramble`, `/study/quiz`, `/ai/inbox`, `/ai/settings`.

### 4. Quy tắc làm việc của sub-agents
- Mỗi sub-agent nhận prompt tự chứa: mục tiêu, file liên quan, Acceptance Criteria của sub-task, định dạng kết quả trả về (danh sách file đổi / lệnh đã chạy / kết quả / lỗi còn lại).
- Developer và Tester là **hai agent khác nhau**; Tester không được "sửa test cho pass" mà không giải thích; test chỉ được nới lỏng khi tiêu chí sai thực sự, và phải ghi lý do vào `PROGRESS.md`.
- Sub-agent không commit; chỉ Orchestrator commit.

### 5. Ghi nhận trạng thái (chống mất ngữ cảnh)
Cuối MỖI vòng, cập nhật `PROGRESS.md`: sub-task hiện tại, số vòng, kết quả test (pass/fail + số lượng), lỗi còn mở, hash commit. Nếu ngữ cảnh dài, dùng `/compact` rồi tiếp tục từ `PROGRESS.md`.

### 6. Điều kiện dừng
- **Thành công**: tất cả sub-task `✅` (hoặc `🚫` có lý do) → chạy 24.11 lần cuối, cập nhật `docs/task/README.md` (thêm dòng Task 24 với trạng thái), báo cáo tổng kết (bảng sub-task, số test, điểm Lighthouse, danh sách `🚫`).
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
