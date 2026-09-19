Bản dưới đây giữ database `sep`, dùng user riêng, đồng nhất `localhost`, bổ sung `dotnet tool restore` và tránh danh sách migration bị lỗi thời.

````markdown
# TeLo Backend - chạy với MySQL local

Backend sử dụng .NET 8, EF Core 8 và MySQL 8. Docker không bắt buộc.

Toàn đội sử dụng EF Migration làm nguồn chuẩn của schema. Không tự tạo hoặc sửa bảng thủ công trong MySQL Workbench.

## 1. Yêu cầu

- .NET SDK 8 trở lên.
- MySQL Server 8.0.16 trở lên.
- MySQL Workbench để tạo database/user, xem và truy vấn dữ liệu.

Kiểm tra phiên bản .NET:

```powershell
dotnet --version
```

Kiểm tra MySQL trên Windows:

```powershell
Get-Service MySQL80
```

Nếu service chưa chạy, mở PowerShell bằng quyền Administrator:

```powershell
Start-Service MySQL80
```

## 2. Tạo database phát triển

Mở MySQL Workbench bằng tài khoản quản trị và chạy:

```sql
CREATE DATABASE IF NOT EXISTS sep
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_0900_ai_ci;

CREATE USER IF NOT EXISTS 'telo_app'@'localhost'
  IDENTIFIED BY 'THAY_BANG_MAT_KHAU_LOCAL_CUA_BAN';

ALTER USER 'telo_app'@'localhost'
  IDENTIFIED BY 'THAY_BANG_MAT_KHAU_LOCAL_CUA_BAN';

GRANT ALL PRIVILEGES ON sep.* TO 'telo_app'@'localhost';

FLUSH PRIVILEGES;
```

Thay `THAY_BANG_MAT_KHAU_LOCAL_CUA_BAN` bằng mật khẩu riêng trên máy của bạn.

Không đưa mật khẩu thật vào source code, ảnh chụp, chat, issue hoặc Git.

## 3. Lưu connection string an toàn

Chạy tại thư mục `LeTo-Backend`:

```powershell
dotnet user-secrets set `
  "ConnectionStrings:DefaultConnection" `
  "Server=localhost;Port=3306;Database=sep;User=telo_app;Password=MAT_KHAU_LOCAL;Allow User Variables=true;" `
  --project WebAPI
```

Thay `MAT_KHAU_LOCAL` bằng mật khẩu đã tạo ở bước 2.

Kiểm tra User Secrets:

```powershell
dotnet user-secrets list --project WebAPI
```

Lưu ý: kết quả có chứa connection string và mật khẩu. Chỉ kiểm tra trên máy cá nhân, không sao chép hoặc chia sẻ kết quả.

## 4. Khôi phục công cụ và dependencies

```powershell
dotnet tool restore
dotnet restore TeLoSchoolManagement.sln
```

Kiểm tra danh sách migration hiện có:

```powershell
dotnet tool run dotnet-ef migrations list `
  --project Infrastructure `
  --startup-project WebAPI
```

## 5. Tạo hoặc cập nhật schema bằng EF Migration

```powershell
dotnet tool run dotnet-ef database update `
  --project Infrastructure `
  --startup-project WebAPI
```

EF lưu các migration đã áp dụng trong bảng `__EFMigrationsHistory` và chỉ chạy những migration còn thiếu.

Không chạy file `CREATE TABLE` thủ công để tránh schema local khác với schema trong source code.

## 6. Build và test

Trong một số môi trường Windows, shared compiler có thể bị chặn. Các lệnh sau phù hợp để kiểm tra dự án:

```powershell
dotnet build TeLoSchoolManagement.sln `
  --disable-build-servers `
  -m:1 `
  -p:UseSharedCompilation=false

dotnet test TeLoSchoolManagement.sln `
  --disable-build-servers `
  -m:1 `
  -p:UseSharedCompilation=false
```

## 7. Chạy API

```powershell
dotnet run --project WebAPI --launch-profile https
```

Swagger:

- `https://localhost:7033/swagger`
- `http://localhost:5035/swagger`

Nếu máy chưa tin cậy HTTPS development certificate:

```powershell
dotnet dev-certs https --trust
```

## 8. Module Ma trận đề thi

API ma trận, đăng nhập JWT và quy trình nghiệp vụ: xem [MATRIX.md](MATRIX.md).

JWT cần `Jwt:SigningKey` tối thiểu 32 ký tự. Cấu hình bằng user secrets hoặc biến môi trường, không commit khóa thật.

## 9. Quy tắc đồng bộ database của đội

1. Thay đổi entity và EF configuration.
2. Tạo migration có tên rõ nghĩa.
3. Review cả phương thức `Up` và `Down`.
4. Commit migration cùng feature liên quan.
5. Thành viên khác pull code và chạy:

```powershell
dotnet tool restore
dotnet tool run dotnet-ef database update `
  --project Infrastructure `
  --startup-project WebAPI
```

6. Không tự sửa schema bằng MySQL Workbench.
7. Không chia sẻ database local; chỉ chia sẻ migration và seed data không nhạy cảm.
8. Dùng lệnh `migrations list` để xem danh sách migration hiện tại, tránh ghi cố định danh sách trong README vì có thể nhanh chóng lỗi thời.
````
