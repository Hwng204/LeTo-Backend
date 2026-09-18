# TeLo Backend - chạy với MySQL local

Backend dùng .NET 8, EF Core 8 và MySQL 8. Docker không bắt buộc. Toàn đội phải
dùng EF Migration làm nguồn chuẩn của schema; không tạo/sửa bảng thủ công trong
MySQL Workbench.

## 1. Yêu cầu

- .NET SDK 8 trở lên.
- MySQL Server 8.0.16 trở lên.
- MySQL Workbench chỉ dùng để tạo database/user, xem và truy vấn dữ liệu.

Kiểm tra MySQL trên Windows:

```powershell
Get-Service MySQL80
```

Nếu service chưa chạy, mở PowerShell bằng quyền Administrator:

```powershell
Start-Service MySQL80
```

## 2. Tạo database phát triển

Mở MySQL Workbench bằng tài khoản quản trị và chạy một lần:

```sql
CREATE DATABASE IF NOT EXISTS telo_dev
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_0900_ai_ci;

CREATE USER IF NOT EXISTS 'telo_app'@'localhost'
  IDENTIFIED BY 'THAY_BANG_MAT_KHAU_LOCAL_CUA_BAN';

GRANT ALL PRIVILEGES ON telo_dev.* TO 'telo_app'@'localhost';
FLUSH PRIVILEGES;
```

`THAY_BANG_MAT_KHAU_LOCAL_CUA_BAN` chỉ tồn tại trên máy cá nhân. Không đưa mật
khẩu thật vào source, ảnh chụp, chat, issue hoặc Git.

## 3. Lưu connection string an toàn

Chạy tại thư mục `LeTo-Backend` và thay mật khẩu bằng giá trị vừa tạo:

```powershell
dotnet user-secrets set `
  "ConnectionStrings:DefaultConnection" `
  "Server=127.0.0.1;Port=3306;Database=telo_dev;User=telo_app;Password=MAT_KHAU_LOCAL;Allow User Variables=true;" `
  --project WebAPI
```

Kiểm tra secret đã có tên đúng mà không cần mở file cấu hình:

```powershell
dotnet user-secrets list --project WebAPI
```

## 4. Tạo schema thật bằng EF Migration

```powershell
dotnet restore TeLoSchoolManagement.sln
dotnet ef migrations list --project Infrastructure --startup-project WebAPI
dotnet ef database update --project Infrastructure --startup-project WebAPI
```

Không chạy lại file `CREATE TABLE` thủ công. EF lưu migration đã áp dụng trong
bảng `__EFMigrationsHistory` và chỉ chạy phần còn thiếu.

## 5. Build, test và chạy API

Trong một số môi trường Windows, shared compiler có thể bị chặn. Lệnh dưới đây
ổn định cho cả CI và máy phát triển:

```powershell
dotnet build TeLoSchoolManagement.sln --disable-build-servers -m:1 -p:UseSharedCompilation=false
dotnet test TeLoSchoolManagement.sln --disable-build-servers -m:1 -p:UseSharedCompilation=false
dotnet run --project WebAPI --launch-profile https
```

Swagger:

- `https://localhost:7033/swagger`
- `http://localhost:5035/swagger`

## 6. Quy tắc đồng bộ database của đội

1. Thay đổi entity và EF configuration.
2. Tạo migration có tên rõ nghĩa.
3. Review cả migration `Up` và `Down`.
4. Commit migration cùng feature.
5. Thành viên khác pull code rồi chạy `dotnet ef database update`.
6. Không chia sẻ database local; chỉ chia sẻ migration và seed data không nhạy cảm.

Database hiện có các migration nền:

- `InitialCreate`
- `AddDatabaseIntegrityObjects`
