using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProvinceAcademicCalendarScope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "uq_academic_years_name",
                table: "academic_years");

            migrationBuilder.DropCheckConstraint(
                name: "ck_academic_years_dates",
                table: "academic_years");

            migrationBuilder.AddColumn<string>(
                name: "province_code",
                table: "schools",
                type: "varchar(2)",
                maxLength: 2,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "academic_years",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "DRAFT",
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32,
                oldDefaultValue: "ACTIVE")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AddColumn<string>(
                name: "province_code",
                table: "academic_years",
                type: "varchar(2)",
                maxLength: 2,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<uint>(
                name: "version",
                table: "academic_years",
                type: "int unsigned",
                nullable: false,
                defaultValue: 1u);

            migrationBuilder.CreateTable(
                name: "provinces",
                columns: table => new
                {
                    code = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    division_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    last_synced_at = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provinces", x => x.code);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "ix_schools_province_code",
                table: "schools",
                column: "province_code");

            migrationBuilder.CreateIndex(
                name: "uq_academic_years_province_name",
                table: "academic_years",
                columns: new[] { "province_code", "name" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_academic_years_dates",
                table: "academic_years",
                sql: "end_date > start_date");

            migrationBuilder.AddCheckConstraint(
                name: "ck_academic_years_status",
                table: "academic_years",
                sql: "status IN ('DRAFT', 'ACTIVE', 'CLOSED')");

            migrationBuilder.CreateIndex(
                name: "ix_provinces_name",
                table: "provinces",
                column: "name");

            migrationBuilder.AddForeignKey(
                name: "fk_academic_years_province",
                table: "academic_years",
                column: "province_code",
                principalTable: "provinces",
                principalColumn: "code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_schools_province",
                table: "schools",
                column: "province_code",
                principalTable: "provinces",
                principalColumn: "code",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_academic_years_province",
                table: "academic_years");

            migrationBuilder.DropForeignKey(
                name: "fk_schools_province",
                table: "schools");

            migrationBuilder.DropTable(
                name: "provinces");

            migrationBuilder.DropIndex(
                name: "ix_schools_province_code",
                table: "schools");

            migrationBuilder.DropIndex(
                name: "uq_academic_years_province_name",
                table: "academic_years");

            migrationBuilder.DropCheckConstraint(
                name: "ck_academic_years_dates",
                table: "academic_years");

            migrationBuilder.DropCheckConstraint(
                name: "ck_academic_years_status",
                table: "academic_years");

            migrationBuilder.DropColumn(
                name: "province_code",
                table: "schools");

            migrationBuilder.DropColumn(
                name: "province_code",
                table: "academic_years");

            migrationBuilder.DropColumn(
                name: "version",
                table: "academic_years");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "academic_years",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "ACTIVE",
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32,
                oldDefaultValue: "DRAFT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "uq_academic_years_name",
                table: "academic_years",
                column: "name",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_academic_years_dates",
                table: "academic_years",
                sql: "end_date >= start_date");
        }
    }
}
