# Module Ma trận đề thi

Các bước cài đặt chung (MySQL, chuỗi kết nối, restore, migration) xem [README.md](README.md). Tài liệu này chỉ nói riêng phần ma trận.

## 1. Chạy khi chưa có đăng nhập

Phần đăng nhập do người khác đảm nhận. Ở môi trường **Development**, backend dùng "người dùng giả" thay cho token:

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:Dev__UserId = "2"        # id user có thật trong DB
$env:Dev__Role = "PHT"        # HIEU_TRUONG | PHT | TEAM_LEAD
$env:Dev__BranchId = "1"      # id chi nhánh; để trống với HIEU_TRUONG
dotnet run --project WebAPI --launch-profile http
```

Các biến `$env:` chỉ có hiệu lực trong cửa sổ PowerShell đang mở. Không đặt `Dev__UserId` thì API ma trận không dùng được (không có cách xác thực).

**Đổi vai khi thử:** trong Swagger (`http://localhost:5035/swagger`) bấm **Authorize**, nhập các header `X-Dev-UserId`, `X-Dev-Role`, `X-Dev-BranchId` (ghi đè giá trị `Dev__*`).

| Vai | X-Dev-Role | X-Dev-BranchId |
|---|---|---|
| Hiệu trưởng (mọi chi nhánh) | `HIEU_TRUONG` | để trống |
| PHT (chỉ chi nhánh mình) | `PHT` | id chi nhánh |
| Tổ trưởng | `TEAM_LEAD` | id chi nhánh |

API cần sẵn dữ liệu: năm học, học kỳ, sách giáo khoa (chương, bài học), ngữ cảnh học thuật, và user Tổ trưởng cùng chi nhánh để giao việc. Repo không kèm dữ liệu mẫu.

## 2. Quy ước API

- Thành công: trả object trực tiếp (không bọc `ApiResponse`).
- Lỗi: Problem Details, có `code` (tiếng Anh, để xử lý) và `detail` (tiếng Việt, để hiển thị).
- Mã HTTP: `401` chưa xác thực, `403` sai quyền hoặc sai chi nhánh, `404` không thấy, `409` xung đột trạng thái, `422` dữ liệu không hợp lệ.
- `statusLabel` là nhãn tiếng Việt của `status`. `allowedActions` cho biết thao tác nào được phép với người dùng và trạng thái hiện tại.
- Tổng số câu và tổng điểm do server tính, không gửi lên.
- Mức nhận thức: `NHAN_BIET`, `THONG_HIEU`, `VAN_DUNG`. Loại câu hỏi luôn là trắc nghiệm, không cần gửi.

## 3. Luồng nghiệp vụ

Ma trận: `Nháp ⇄ Đã nộp → Đã duyệt → Đã lưu trữ`. Không có "Từ chối". Muốn sửa ma trận đã duyệt thì **sao chép** thành bản Nháp mới.

- **PHT tự tạo:** tạo Nháp, bấm Xác nhận là thành Đã duyệt.
- **PHT giao Tổ trưởng:** PHT giao nhiệm vụ, Tổ trưởng tạo đúng một ma trận cho nhiệm vụ đó, nộp, PHT duyệt.
- Nhiệm vụ: `Đã giao → Đã nộp → Hoàn thành`. Thu hồi ma trận thì nhiệm vụ về `Đã giao`.
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
| `POST /api/matrices/{id}/withdraw` | Thu hồi (Tổ trưởng) |
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

Frontend đặt `VITE_API_BASE_URL` trỏ đúng cổng backend (ví dụ `http://localhost:5035/api`).

## 6. Khi phần đăng nhập được thêm vào

- Người làm đăng nhập thêm xác thực của họ vào `WebAPI/Program.cs`. Controller và code ma trận không phải sửa.
- Token phải có: `NameIdentifier` (hoặc `sub`), `Role` (`HIEU_TRUONG` / `PHT` / `TEAM_LEAD`) và `branch_id` (bắt buộc với PHT, thiếu thì bị 403; tên hằng số ở `MatrixClaims.BranchId`).
- Sau khi tích hợp, bỏ các biến `Dev__*`.
