using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatrixFeatureConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_matrix_details_score",
                table: "matrix_details");

            migrationBuilder.AddColumn<ulong>(
                name: "academic_context_id",
                table: "tasks",
                type: "bigint unsigned",
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "semester_id",
                table: "tasks",
                type: "bigint unsigned",
                nullable: true);

            migrationBuilder.AlterColumn<ulong>(
                name: "task_id",
                table: "exam_matrices",
                type: "bigint unsigned",
                nullable: true,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned");

            migrationBuilder.CreateIndex(
                name: "idx_tasks_context",
                table: "tasks",
                column: "academic_context_id");

            migrationBuilder.CreateIndex(
                name: "idx_tasks_semester",
                table: "tasks",
                column: "semester_id");

            migrationBuilder.AddCheckConstraint(
                name: "ck_matrix_details_score",
                table: "matrix_details",
                sql: "allocated_score > 0");

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_context",
                table: "tasks",
                column: "academic_context_id",
                principalTable: "academic_contexts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_semester",
                table: "tasks",
                column: "semester_id",
                principalTable: "semesters",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tasks_context",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_tasks_semester",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "idx_tasks_context",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "idx_tasks_semester",
                table: "tasks");

            migrationBuilder.DropCheckConstraint(
                name: "ck_matrix_details_score",
                table: "matrix_details");

            migrationBuilder.DropColumn(
                name: "academic_context_id",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "semester_id",
                table: "tasks");

            migrationBuilder.AlterColumn<ulong>(
                name: "task_id",
                table: "exam_matrices",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_matrix_details_score",
                table: "matrix_details",
                sql: "allocated_score >= 0");
        }
    }
}
