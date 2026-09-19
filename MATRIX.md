# Module Ma trận đề thi

Các bước cài đặt chung (MySQL, chuỗi kết nối, restore, migration) xem [README.md](README.md). Tài liệu này chỉ nói riêng phần ma trận.

## 1. Đăng nhập và chạy thử

Backend dùng access token JWT. Cần khóa ký (tối thiểu 32 ký tự) đặt qua biến môi trường, **không** ghi vào file được commit:

```powershell
$env:Jwt__SigningKey = "chuoi-bi-mat-it-nhat-32-ky-tu-cua-ban-1234"
dotnet run --project WebAPI --launch-profile http
```

Các biến `$env:` chỉ có hiệu lực trong cửa sổ PowerShell đang mở. Thiếu khóa thì backend không khởi động. Các thông số còn lại (`Jwt:Issuer`, `Jwt:Audience`, `Jwt:AccessTokenMinutes`, mặc định 60 phút) nằm trong `appsettings.json`.

**Lấy access token:** `POST /api/auth/login`
```json
{ "username": "pht_a", "password": "mat-khau" }
```
Trả về `{ "accessToken": "...", "tokenType": "Bearer", "expiresAtUtc": "..." }`. Sai tên đăng nhập hoặc mật khẩu trả 401. Mọi user có trạng thái `ACTIVE` đều đăng nhập được, token mang đủ danh sách vai trò của user.

**Dùng token:** gửi header `Authorization: Bearer <accessToken>`. Trong Swagger bấm **Authorize** rồi dán token (không gõ chữ `Bearer`).

Mật khẩu trong cột `users.password_hash` phải được băm bằng `PasswordHasher` của ASP.NET Identity (PBKDF2). Cách băm khác (bcrypt...) sẽ không đăng nhập được.

Token chứa: `NameIdentifier` (id user), `Name` (tên đăng nhập), `Role` (mỗi vai trò một claim) và `branch_id` (chi nhánh của user, nếu có). API ma trận cần vai trò `HIEU_TRUONG`, `PHT` hoặc `TEAM_LEAD`; user có vai trò khác đăng nhập được nhưng gọi API ma trận bị 403.

API cần sẵn dữ liệu: năm học, học kỳ, sách giáo khoa (chương, bài học), ngữ cảnh học thuật, và user Tổ trưởng cùng chi nhánh để giao việc. Repo không kèm dữ liệu mẫu.

## 2. Quy ước API

- Thành công: trả object trực tiếp (không bọc `ApiResponse`).
- Lỗi: Problem Details, có `code` (tiếng Anh, để xử lý) và `detail` (tiếng Việt, để hiển thị).
- Mã HTTP: `401` chưa xác thực, `403` sai quyền hoặc sai chi nhánh, `404` không thấy, `409` xung đột trạng thái, `422` dữ liệu không hợp lệ.
- `statusLabel` là nhãn tiếng Việt của `status`. `allowedActions` cho biết thao tác nào được phép với người dùng và trạng thái hiện tại.
- Tổng số câu và tổng điểm do server tính, không gửi lên.
- Mức nhận thức: `NHAN_BIET`, `THONG_HIEU`, `VAN_DUNG`. Loại câu hỏi luôn là trắc nghiệm, không cần gửi.

## 3. Luồng nghiệp vụ

Ma trận: `Nháp → Đã nộp → Đã duyệt → Đã lưu trữ`. PHT có thể **từ chối** ma trận đã nộp để đưa về `Nháp` (kèm nhận xét) cho Tổ trưởng làm lại. Muốn sửa ma trận đã duyệt thì **sao chép** thành bản Nháp mới.

- **PHT tự tạo:** tạo Nháp, bấm Xác nhận là thành Đã duyệt.
- **PHT giao Tổ trưởng:** PHT giao nhiệm vụ, Tổ trưởng tạo đúng một ma trận cho nhiệm vụ đó, nộp, PHT duyệt.
- Nhiệm vụ: `Đã giao → Đã nộp → Hoàn thành`. PHT từ chối ma trận thì nhiệm vụ về `Đã giao` để Tổ trưởng làm lại.
- **Từ chối:** chỉ PHT/Hiệu trưởng, chỉ với ma trận `Đã nộp`. Ma trận về `Nháp`, response có `rejectComment`, `rejectedAt`, `rejectedByUserId`. Tổ trưởng sửa rồi nộp lại (nhận xét cũ bị xóa khi nộp lại). Tổ trưởng **không** thu hồi được ma trận đã nộp; PHT vẫn sửa trực tiếp được trước khi duyệt hoặc từ chối.
- Ma trận không có dòng chi tiết thì không nộp hoặc xác nhận được.

## 4. Danh sách API

| API | Tác dụng |
|---|---|
| `GET /api/matrices` | Danh sách (phân trang; lọc từ khóa, ngữ cảnh, học kỳ, trạng thái) |
| `GET /api/matrices/{id}` | Chi tiết, tổng, `allowedActions` |
| `POST /api/matrices` | Tạo Nháp (`taskId: null` là tạo trực tiếp) |
| `PUT /api/matrices/{id}` | Sửa tên và chi tiết |
| `DELETE /api/matrices/{id}` | Xóa (chỉ khi Nháp) |
| `POST /api/matrices/{id}/submit` | Nộp |
| `POST /api/matrices/{id}/reject` | Từ chối (PHT): về Nháp, kèm nhận xét tùy chọn (`{"comment": "..."}`, tối đa 1000 ký tự) |
| `POST /api/matrices/{id}/approve` | Duyệt (PHT) |
| `POST /api/matrices/{id}/confirm` | Xác nhận nhanh ma trận tự tạo (PHT) |
| `POST /api/matrices/{id}/archive` | Lưu trữ (PHT) |
| `POST /api/matrices/{id}/clone` | Sao chép thành Nháp mới (PHT) |
| `GET /api/matrices/{id}/export.xlsx` | Xuất Excel (đã duyệt hoặc đã lưu trữ) |
| `POST /api/matrix-tasks` | Giao nhiệm vụ cho Tổ trưởng |
| `GET /api/matrix-tasks` | Nhiệm vụ đã giao (PHT, Hiệu trưởng) |
| `GET /api/my/matrix-tasks` | Nhiệm vụ của tôi (Tổ trưởng) |
| `GET /api/matrix-tasks/{id}` | Chi tiết nhiệm vụ |
| `GET /api/matrix-reference-data` | Dữ liệu dựng form: ngữ cảnh, học kỳ, bài học, Tổ trưởng, mức nhận thức |

Body tạo ma trận mẫu:

```json
{
  "name": "Ma trận Toán 5 - HK1",
  "academicContextId": 1,
  "semesterId": 1,
  "taskId": null,
  "details": [
    { "lessonId": 1, "cognitiveLevel": "NHAN_BIET", "questionCount": 3, "allocatedScore": 1.5 }
  ]
}
```

## 5. Cấu hình

| Mục | Ở đâu | Ghi chú |
|---|---|---|
| Origin frontend | `Cors:AllowedOrigins` trong `appsettings.json` | Mặc định `http://localhost:5173` |
| Mã vai trò | `MatrixAuth:PrincipalRoleCodes`, `PhtRoleCodes`, `TeamLeadRoleCodes` | Mặc định `HIEU_TRUONG`, `PHT`, `TEAM_LEAD` / `TO_TRUONG` |
| Khóa ký token | biến `Jwt__SigningKey` | Tối thiểu 32 ký tự, không commit |

Frontend đặt `VITE_API_BASE_URL` trỏ đúng cổng backend (ví dụ `http://localhost:5035/api`).
