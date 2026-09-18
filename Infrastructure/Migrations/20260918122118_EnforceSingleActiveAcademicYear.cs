using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnforceSingleActiveAcademicYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "active_province_code",
                table: "academic_years",
                type: "varchar(2)",
                maxLength: 2,
                nullable: true,
                computedColumnSql: "CASE WHEN status = 'ACTIVE' THEN province_code ELSE NULL END",
                stored: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "uq_academic_years_active_province",
                table: "academic_years",
                column: "active_province_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "uq_academic_years_active_province",
                table: "academic_years");

            migrationBuilder.DropColumn(
                name: "active_province_code",
                table: "academic_years");
        }
    }
}
