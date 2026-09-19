using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTermLifecycleFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_semesters_academic_year_id_migration",
                table: "semesters",
                column: "academic_year_id");

            migrationBuilder.DropIndex(
                name: "uq_semesters_year_name",
                table: "semesters");

            migrationBuilder.DropCheckConstraint(
                name: "ck_semesters_dates",
                table: "semesters");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "start_date",
                table: "semesters",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "end_date",
                table: "semesters",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<byte>(
                name: "semester_order",
                table: "semesters",
                type: "tinyint unsigned",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "semesters",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "PLANNED",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<uint>(
                name: "version",
                table: "semesters",
                type: "int unsigned",
                nullable: false,
                defaultValue: 1u);

            migrationBuilder.Sql("""
                UPDATE semesters AS target
                INNER JOIN (
                    SELECT id,
                           ROW_NUMBER() OVER (
                               PARTITION BY academic_year_id
                               ORDER BY start_date, id
                           ) AS calculated_order
                    FROM semesters
                ) AS ranked ON ranked.id = target.id
                SET target.semester_order = ranked.calculated_order;
                """);

            migrationBuilder.AlterColumn<byte>(
                name: "semester_order",
                table: "semesters",
                type: "tinyint unsigned",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "uq_semesters_year_order",
                table: "semesters",
                columns: new[] { "academic_year_id", "semester_order" },
                unique: true);

            migrationBuilder.DropIndex(
                name: "ix_semesters_academic_year_id_migration",
                table: "semesters");

            migrationBuilder.AddCheckConstraint(
                name: "ck_semesters_dates",
                table: "semesters",
                sql: "(start_date IS NULL AND end_date IS NULL) OR end_date > start_date");

            migrationBuilder.AddCheckConstraint(
                name: "ck_semesters_order",
                table: "semesters",
                sql: "semester_order IN (1, 2)");

            migrationBuilder.AddCheckConstraint(
                name: "ck_semesters_status",
                table: "semesters",
                sql: "status IN ('PLANNED', 'ACTIVE', 'CLOSED')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_semesters_academic_year_id_migration",
                table: "semesters",
                column: "academic_year_id");

            migrationBuilder.DropIndex(
                name: "uq_semesters_year_order",
                table: "semesters");

            migrationBuilder.DropCheckConstraint(
                name: "ck_semesters_dates",
                table: "semesters");

            migrationBuilder.DropCheckConstraint(
                name: "ck_semesters_order",
                table: "semesters");

            migrationBuilder.DropCheckConstraint(
                name: "ck_semesters_status",
                table: "semesters");

            migrationBuilder.DropColumn(
                name: "semester_order",
                table: "semesters");

            migrationBuilder.DropColumn(
                name: "status",
                table: "semesters");

            migrationBuilder.DropColumn(
                name: "version",
                table: "semesters");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "start_date",
                table: "semesters",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "end_date",
                table: "semesters",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "uq_semesters_year_name",
                table: "semesters",
                columns: new[] { "academic_year_id", "name" },
                unique: true);

            migrationBuilder.DropIndex(
                name: "ix_semesters_academic_year_id_migration",
                table: "semesters");

            migrationBuilder.AddCheckConstraint(
                name: "ck_semesters_dates",
                table: "semesters",
                sql: "end_date >= start_date");
        }
    }
}
