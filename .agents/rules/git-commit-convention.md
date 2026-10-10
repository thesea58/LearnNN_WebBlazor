---
trigger: always_on
---

# GIT COMMIT CONVENTION RULE

> Quy ước chuẩn hóa thông điệp Git Commit (Conventional Commits) cho dự án LearnNN_WebBlazor.
> Mọi commit được tạo bởi AI hoặc Developer đều phải tuân thủ nghiêm ngặt định dạng này.

---

## 1. Cấu trúc Message (Commit Structure)

Cú pháp chuẩn:
```text
<type>(<scope>): <subject>
# hoặc khi gắn liền với một Task cụ thể trong docs/task/:
<type>: [<Task XX>] <subject>
```

- **`<type>`**: Loại thay đổi (bắt buộc, chữ thường, xem danh sách bên dưới).
- **`(<scope>)`**: Phạm vi/khu vực thay đổi (tùy chọn, vd: `study`, `ai`, `vocab`, `layout`, `db`).
- **`[<Task XX>]`**: Mã số task tương ứng trong `docs/task/README.md` (khuyến nghị khi giải quyết task).
- **`<subject>`**: Mô tả ngắn gọn (tiếng Anh, thể mệnh lệnh/imperative: "add", "implement", "fix", không dùng "added", "fixing").

---

## 2. Phân loại Commit (Types)

| Type | Mục đích | Ví dụ |
|------|----------|-------|
| `feat` | Thêm tính năng mới cho người dùng hoặc hệ thống | `feat: [Task 18] implement AI quiz explainer and trap detector` |
| `fix` | Sửa lỗi / bug | `fix(quiz): prevent duplicate answer logging on double click` |
| `docs` | Thêm hoặc cập nhật tài liệu (`docs/`, `PROJECT_MAP.md`, `README.md`) | `docs: update project map and architecture change log for task 18` |
| `refactor` | Tái cấu trúc code (không sửa bug, không thêm tính năng mới) | `refactor(services): extract common SRS review calculation logic` |
| `perf` | Cải thiện hiệu năng, tối ưu tốc độ hoặc bộ nhớ | `perf(words): optimize search query with EF Core compiled queries` |
| `style` | Thay đổi format code, khoảng trắng, dấu chấm phẩy, không đổi logic | `style(css): adjust flashcard 3D flip perspective and spacing` |
| `test` | Thêm hoặc sửa đổi bài test (Unit Test, BUnit, Integration Test) | `test(srs): add unit tests for SM-2 interval calculations` |
| `chore` | Cập nhật cấu hình build, dependencies, tooling, gitignore, dọn dẹp | `chore: add gitignore entries for temporary database cache files` |
| `ci` | Thay đổi cấu hình CI/CD pipelines (GitHub Actions, workflows) | `ci: optimize dotnet.yml publish step for blazor web app` |

---

## 3. Nguyên tắc vàng (Best Practices)

1. **Atomic Commits**: Mỗi commit chỉ nên giải quyết một đơn vị công việc rõ ràng. Không gộp nhiều thay đổi không liên quan vào cùng một commit.
2. **Imperative Mood**: Viết subject ở thể mệnh lệnh hiện tại:
   - ✅ `feat: implement flashcard study mode`
   - ❌ `feat: implemented flashcard study mode`
   - ❌ `feat: implementing flashcard study mode`
3. **Clean Working Tree**:
   - Luôn kiểm tra `git status` và `git diff` trước khi commit.
   - Tuyệt đối không commit các file tạm, file nhị phân phát sinh khi build (`bin/`, `obj/`, `.vs/`, `.idea/`, `*.db-shm`, `*.db-wal`).
4. **Task Reference**: Khi hoàn thành hoặc đóng góp vào một task trong dự án, luôn đặt mã task vào subject:
   - `feat: [Task 17] add AI manual bridge modal and lenient json parser`
