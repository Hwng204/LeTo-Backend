using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImmutableAcademicYearCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "academic_years",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.Sql("""
                UPDATE academic_years
                SET code = CONCAT(province_code, '-', name)
                WHERE province_code IS NOT NULL;
                """);

            migrationBuilder.CreateIndex(
                name: "uq_academic_years_code",
                table: "academic_years",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "uq_academic_years_code",
                table: "academic_years");

            migrationBuilder.DropColumn(
                name: "code",
                table: "academic_years");
        }
    }
}
