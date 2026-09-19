using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatrixRejection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "reject_comment",
                table: "exam_matrices",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "rejected_at",
                table: "exam_matrices",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "rejected_by_user_id",
                table: "exam_matrices",
                type: "bigint unsigned",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "idx_exam_matrices_rejecter",
                table: "exam_matrices",
                column: "rejected_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_exam_matrices_rejecter",
                table: "exam_matrices",
                column: "rejected_by_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_exam_matrices_rejecter",
                table: "exam_matrices");

            migrationBuilder.DropIndex(
                name: "idx_exam_matrices_rejecter",
                table: "exam_matrices");

            migrationBuilder.DropColumn(
                name: "reject_comment",
                table: "exam_matrices");

            migrationBuilder.DropColumn(
                name: "rejected_at",
                table: "exam_matrices");

            migrationBuilder.DropColumn(
                name: "rejected_by_user_id",
                table: "exam_matrices");
        }
    }
}
