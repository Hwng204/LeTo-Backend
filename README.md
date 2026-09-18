# Backend - Chạy sau khi pull

Chạy các lệnh tại thư mục chứa TeLoSchoolManagement.sln.

## 1. Vào đúng thư mục repo

~~~powershell
cd "location..."
~~~


## 2. Chuẩn bị MySQL 8.0

Cần cài .NET SDK 8.x và MySQL Server 8.0.16 trở lên. Docker không bắt buộc.

~~~powershell
dotnet --version
mysql --version
Get-Service *mysql*
Start-Service MySQL80
~~~

Tạo database trống:

~~~powershell
mysql -u root -p -e "CREATE DATABASE sep CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;"
~~~


Đặt connection string trong terminal hiện tại:

~~~powershell
$env:ConnectionStrings__DefaultConnection = "Server=127.0.0.1;Port=3306;Database=sep;User=root;Password=MAT_KHAU_LOCAL;"
~~~

Không ghi mật khẩu vào source hoặc Git.

## 3. Restore và build

~~~powershell
dotnet tool restore
dotnet restore TeLoSchoolManagement.sln
dotnet build TeLoSchoolManagement.sln
~~~


## 4. Tạo database bằng migration

~~~powershell
dotnet tool run dotnet-ef migrations list --project Infrastructure --startup-project WebAPI
dotnet tool run dotnet-ef database update --project Infrastructure --startup-project WebAPI
~~~

Danh sách migration phải có:

~~~text
InitialCreate
AddDatabaseIntegrityObjects
AddMatrixFeatureConstraints
~~~


## 5. Chạy API

~~~powershell
dotnet run --project WebAPI --launch-profile https
~~~

Mở Swagger tại:

~~~text
https://localhost:7033/swagger
http://localhost:5035/swagger
~~~

Sau migration thành công, database có 49 bảng, 2 generated column, 1 unique index,
11 trigger và không có view.


## 6. Module Ma trận đề thi

Các API ma trận và cách chạy thử (người dùng giả khi chưa có đăng nhập): xem [MATRIX.md](MATRIX.md).
