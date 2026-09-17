using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "academic_years",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, defaultValue: "ACTIVE", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_academic_years", x => x.id);
                    table.CheckConstraint("ck_academic_years_dates", "end_date >= start_date");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "grade_levels",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, defaultValue: "ACTIVE", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grade_levels", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "modules",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, defaultValue: "ACTIVE", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modules", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "schools",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, defaultValue: "ACTIVE", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schools", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, defaultValue: "ACTIVE", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "textbooks",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    book_set = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_textbooks", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "semesters",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    academic_year_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_semesters", x => x.id);
                    table.CheckConstraint("ck_semesters_dates", "end_date >= start_date");
                    table.ForeignKey(
                        name: "fk_semesters_academic_year",
                        column: x => x.academic_year_id,
                        principalTable: "academic_years",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "navbars",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    module_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    parent_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    display_order = table.Column<uint>(type: "int unsigned", nullable: false, defaultValue: 0u),
                    url_path = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, defaultValue: "ACTIVE", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_navbars", x => x.id);
                    table.ForeignKey(
                        name: "fk_navbars_module",
                        column: x => x.module_id,
                        principalTable: "modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_navbars_parent",
                        column: x => x.parent_id,
                        principalTable: "navbars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "school_branches",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    school_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, defaultValue: "ACTIVE", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_school_branches", x => x.id);
                    table.UniqueConstraint("uq_school_branches_id_school", x => new { x.id, x.school_id });
                    table.ForeignKey(
                        name: "fk_school_branches_school",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "textbook_chapters",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    textbook_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sort_order = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_textbook_chapters", x => x.id);
                    table.ForeignKey(
                        name: "fk_textbook_chapters_textbook",
                        column: x => x.textbook_id,
                        principalTable: "textbooks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    role_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    navbar_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    permission = table.Column<uint>(type: "int unsigned", nullable: false, defaultValue: 0u)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => new { x.role_id, x.navbar_id });
                    table.ForeignKey(
                        name: "fk_permissions_navbar",
                        column: x => x.navbar_id,
                        principalTable: "navbars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_permissions_role",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "academic_contexts",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    academic_year_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    school_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    textbook_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    subject_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    grade_level_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    school_branch_id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_academic_contexts", x => x.id);
                    table.ForeignKey(
                        name: "fk_academic_contexts_branch_school",
                        columns: x => new { x.school_branch_id, x.school_id },
                        principalTable: "school_branches",
                        principalColumns: new[] { "id", "school_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_academic_contexts_grade",
                        column: x => x.grade_level_id,
                        principalTable: "grade_levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_academic_contexts_school",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_academic_contexts_subject",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_academic_contexts_textbook",
                        column: x => x.textbook_id,
                        principalTable: "textbooks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_academic_contexts_year",
                        column: x => x.academic_year_id,
                        principalTable: "academic_years",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    school_branch_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, defaultValue: "ACTIVE", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    academic_year_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    grade_level_id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.id);
                    table.ForeignKey(
                        name: "fk_classes_academic_year",
                        column: x => x.academic_year_id,
                        principalTable: "academic_years",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_classes_branch",
                        column: x => x.school_branch_id,
                        principalTable: "school_branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_classes_grade_level",
                        column: x => x.grade_level_id,
                        principalTable: "grade_levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exams",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    semester_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    school_branch_id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exams", x => x.id);
                    table.CheckConstraint("ck_exams_dates", "end_date >= start_date");
                    table.ForeignKey(
                        name: "fk_exams_branch",
                        column: x => x.school_branch_id,
                        principalTable: "school_branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exams_semester",
                        column: x => x.semester_id,
                        principalTable: "semesters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "notification_configs",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    school_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    school_branch_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    base_config_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    event_code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    timing = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    recipient_scope = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title_template = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content_template = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    action_url_template = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_configs", x => x.id);
                    table.CheckConstraint("ck_notification_configs_recipient_scope", "recipient_scope IN ('NONE', 'ALL_SCHOOL', 'SOURCE_ACTOR', 'SOURCE_ASSIGNEE', 'SOURCE_PARTICIPANTS')");
                    table.CheckConstraint("ck_notification_configs_timing", "timing IN ('IMMEDIATE', 'BEFORE_DEADLINE')");
                    table.ForeignKey(
                        name: "fk_notification_configs_branch_school",
                        columns: x => new { x.school_branch_id, x.school_id },
                        principalTable: "school_branches",
                        principalColumns: new[] { "id", "school_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_notification_configs_school",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "question_banks",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    school_branch_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    subject_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    grade_level_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    bank_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_banks", x => x.id);
                    table.CheckConstraint("ck_question_banks_type", "bank_type IN ('exam', 'practice')");
                    table.ForeignKey(
                        name: "fk_question_banks_branch",
                        column: x => x.school_branch_id,
                        principalTable: "school_branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_question_banks_grade",
                        column: x => x.grade_level_id,
                        principalTable: "grade_levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_question_banks_subject",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    school_branch_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    room_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.id);
                    table.ForeignKey(
                        name: "fk_rooms_branch",
                        column: x => x.school_branch_id,
                        principalTable: "school_branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(254)", maxLength: 254, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    school_branch_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    full_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    moet_identifier = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, defaultValue: "ACTIVE", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_school_branch",
                        column: x => x.school_branch_id,
                        principalTable: "school_branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "textbook_lessons",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    chapter_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    content = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sort_order = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_textbook_lessons", x => x.id);
                    table.ForeignKey(
                        name: "fk_textbook_lessons_chapter",
                        column: x => x.chapter_id,
                        principalTable: "textbook_chapters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    config_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    school_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    school_branch_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    scheduled_for = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    title = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    action_url = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_branch_school",
                        columns: x => new { x.school_branch_id, x.school_id },
                        principalTable: "school_branches",
                        principalColumns: new[] { "id", "school_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_notifications_config",
                        column: x => x.config_id,
                        principalTable: "notification_configs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_notifications_school",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_rooms",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    room_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    candidate_limit = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_rooms", x => x.id);
                    table.CheckConstraint("ck_exam_rooms_limit", "candidate_limit > 0");
                    table.ForeignKey(
                        name: "fk_exam_rooms_exam",
                        column: x => x.exam_id,
                        principalTable: "exams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exam_rooms_room",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_subjects",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    subject_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    duration_minutes = table.Column<uint>(type: "int unsigned", nullable: false),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    result_published_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    result_published_by_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_subjects", x => x.id);
                    table.CheckConstraint("ck_exam_subjects_duration", "duration_minutes > 0");
                    table.ForeignKey(
                        name: "fk_exam_subjects_exam",
                        column: x => x.exam_id,
                        principalTable: "exams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exam_subjects_publisher",
                        column: x => x.result_published_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_exam_subjects_subject",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "notification_targets",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    config_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    role_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    user_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    action = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_targets", x => x.id);
                    table.CheckConstraint("ck_notification_targets_action", "action IN ('INCLUDE', 'EXCLUDE')");
                    table.ForeignKey(
                        name: "fk_notification_targets_config",
                        column: x => x.config_id,
                        principalTable: "notification_configs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notification_targets_role",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notification_targets_user",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    class_id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.id);
                    table.ForeignKey(
                        name: "fk_students_class",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_students_user",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_by_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    assigned_to_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    due_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    updated_by_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    task_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.id);
                    table.ForeignKey(
                        name: "fk_tasks_assignee",
                        column: x => x.assigned_to_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tasks_creator",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tasks_updater",
                        column: x => x.updated_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    specialization = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    position = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    phone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(254)", maxLength: 254, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    class_id = table.Column<ulong>(type: "bigint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teachers", x => x.id);
                    table.ForeignKey(
                        name: "fk_teachers_class",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_teachers_user",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    role_id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_roles_role",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_user",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "notification_recipients",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    notification_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    email_status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "PENDING", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email_sent_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    read_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_recipients", x => x.id);
                    table.CheckConstraint("ck_notification_recipients_email_status", "email_status IN ('PENDING', 'SENDING', 'SENT', 'ERROR', 'CANCELLED')");
                    table.CheckConstraint("ck_notification_recipients_sent_at", "email_status <> 'SENT' OR email_sent_at IS NOT NULL");
                    table.ForeignKey(
                        name: "fk_notification_recipients_notification",
                        column: x => x.notification_id,
                        principalTable: "notifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notification_recipients_user",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_matrices",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    task_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    semester_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    academic_context_id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_matrices", x => x.id);
                    table.ForeignKey(
                        name: "fk_exam_matrices_context",
                        column: x => x.academic_context_id,
                        principalTable: "academic_contexts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exam_matrices_semester",
                        column: x => x.semester_id,
                        principalTable: "semesters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_exam_matrices_task",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "question_tasks",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    task_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    lesson_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    assigned_question_count = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_tasks", x => x.id);
                    table.CheckConstraint("ck_question_tasks_count", "assigned_question_count > 0");
                    table.ForeignKey(
                        name: "fk_question_tasks_lesson",
                        column: x => x.lesson_id,
                        principalTable: "textbook_lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_question_tasks_task",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_proctors",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    teacher_id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_proctors", x => x.id);
                    table.ForeignKey(
                        name: "fk_exam_proctors_exam",
                        column: x => x.exam_id,
                        principalTable: "exams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exam_proctors_teacher",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_sets",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_matrix_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    task_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    created_by_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    purpose = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    approved_by_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    approved_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    source_exam_set_id = table.Column<ulong>(type: "bigint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_sets", x => x.id);
                    table.ForeignKey(
                        name: "fk_exam_sets_approver",
                        column: x => x.approved_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_exam_sets_creator",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exam_sets_matrix",
                        column: x => x.exam_matrix_id,
                        principalTable: "exam_matrices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_exam_sets_source",
                        column: x => x.source_exam_set_id,
                        principalTable: "exam_sets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_exam_sets_task",
                        column: x => x.task_id,
                        principalTable: "tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "matrix_details",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_matrix_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    lesson_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    cognitive_level = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    question_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    question_count = table.Column<uint>(type: "int unsigned", nullable: false),
                    allocated_score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_matrix_details", x => x.id);
                    table.CheckConstraint("ck_matrix_details_count", "question_count > 0");
                    table.CheckConstraint("ck_matrix_details_score", "allocated_score >= 0");
                    table.ForeignKey(
                        name: "fk_matrix_details_lesson",
                        column: x => x.lesson_id,
                        principalTable: "textbook_lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_matrix_details_matrix",
                        column: x => x.exam_matrix_id,
                        principalTable: "exam_matrices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "question_task_details",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    question_task_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    cognitive_level = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    question_count = table.Column<uint>(type: "int unsigned", nullable: false),
                    question_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_task_details", x => x.id);
                    table.CheckConstraint("ck_question_task_details_count", "question_count > 0");
                    table.ForeignKey(
                        name: "fk_question_task_details_task",
                        column: x => x.question_task_id,
                        principalTable: "question_tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_subject_grade_levels",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_subject_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    grade_level_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    primary_exam_set_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    backup_exam_set_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_subject_grade_levels", x => x.id);
                    table.ForeignKey(
                        name: "fk_exam_subject_grade_backup_set",
                        column: x => x.backup_exam_set_id,
                        principalTable: "exam_sets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_exam_subject_grade_grade",
                        column: x => x.grade_level_id,
                        principalTable: "grade_levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exam_subject_grade_primary_set",
                        column: x => x.primary_exam_set_id,
                        principalTable: "exam_sets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exam_subject_grade_subject",
                        column: x => x.exam_subject_id,
                        principalTable: "exam_subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_variants",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_set_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    variant_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_variants", x => x.id);
                    table.ForeignKey(
                        name: "fk_exam_variants_set",
                        column: x => x.exam_set_id,
                        principalTable: "exam_sets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    question_task_detail_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    question_bank_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    content = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    answer_explanation = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_by_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    submitted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    reviewed_by_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    review_comment = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    reviewed_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.id);
                    table.ForeignKey(
                        name: "fk_questions_bank",
                        column: x => x.question_bank_id,
                        principalTable: "question_banks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_questions_creator",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_questions_reviewer",
                        column: x => x.reviewed_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_questions_task_detail",
                        column: x => x.question_task_detail_id,
                        principalTable: "question_task_details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_sessions",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_subject_grade_level_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    end_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    access_code = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_sessions", x => x.id);
                    table.CheckConstraint("ck_exam_sessions_time", "end_at > start_at");
                    table.ForeignKey(
                        name: "fk_exam_sessions_subject_grade",
                        column: x => x.exam_subject_grade_level_id,
                        principalTable: "exam_subject_grade_levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_set_questions",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_set_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    question_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    matrix_detail_id = table.Column<ulong>(type: "bigint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_set_questions", x => x.id);
                    table.ForeignKey(
                        name: "fk_exam_set_questions_matrix_detail",
                        column: x => x.matrix_detail_id,
                        principalTable: "matrix_details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_exam_set_questions_question",
                        column: x => x.question_id,
                        principalTable: "questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exam_set_questions_set",
                        column: x => x.exam_set_id,
                        principalTable: "exam_sets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "question_options",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    question_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_correct = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    sort_order = table.Column<uint>(type: "int unsigned", nullable: false),
                    option_key = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_options", x => x.id);
                    table.ForeignKey(
                        name: "fk_question_options_question",
                        column: x => x.question_id,
                        principalTable: "questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "session_rooms",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_session_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    exam_room_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session_rooms", x => x.id);
                    table.ForeignKey(
                        name: "fk_session_rooms_exam_room",
                        column: x => x.exam_room_id,
                        principalTable: "exam_rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_session_rooms_session",
                        column: x => x.exam_session_id,
                        principalTable: "exam_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_variant_questions",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_variant_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    exam_set_question_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    position_no = table.Column<uint>(type: "int unsigned", nullable: false),
                    option_order_json = table.Column<string>(type: "json", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_variant_questions", x => x.id);
                    table.CheckConstraint("ck_exam_variant_questions_position", "position_no > 0");
                    table.ForeignKey(
                        name: "fk_exam_variant_questions_set_question",
                        column: x => x.exam_set_question_id,
                        principalTable: "exam_set_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exam_variant_questions_variant",
                        column: x => x.exam_variant_id,
                        principalTable: "exam_variants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_registrations",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_subject_grade_level_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    student_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    session_room_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_registrations", x => x.id);
                    table.ForeignKey(
                        name: "fk_exam_registrations_session_room",
                        column: x => x.session_room_id,
                        principalTable: "session_rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_exam_registrations_student",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exam_registrations_subject_grade",
                        column: x => x.exam_subject_grade_level_id,
                        principalTable: "exam_subject_grade_levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "proctor_assignments",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    session_room_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    exam_proctor_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    proctor_role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    assigned_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proctor_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_proctor_assignments_exam_proctor",
                        column: x => x.exam_proctor_id,
                        principalTable: "exam_proctors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_proctor_assignments_session_room",
                        column: x => x.session_room_id,
                        principalTable: "session_rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_attempts",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_registration_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    exam_variant_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    started_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    submitted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    total_score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    note = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_attempts", x => x.id);
                    table.CheckConstraint("ck_exam_attempts_score", "total_score IS NULL OR total_score >= 0");
                    table.CheckConstraint("ck_exam_attempts_time", "submitted_at IS NULL OR submitted_at >= started_at");
                    table.ForeignKey(
                        name: "fk_exam_attempts_registration",
                        column: x => x.exam_registration_id,
                        principalTable: "exam_registrations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exam_attempts_variant",
                        column: x => x.exam_variant_id,
                        principalTable: "exam_variants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "exam_attempt_answers",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_attempt_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    exam_variant_question_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    selected_option_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    score_awarded = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    is_correct = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_attempt_answers", x => x.id);
                    table.CheckConstraint("ck_exam_attempt_answers_score", "score_awarded IS NULL OR score_awarded >= 0");
                    table.ForeignKey(
                        name: "fk_exam_attempt_answers_attempt",
                        column: x => x.exam_attempt_id,
                        principalTable: "exam_attempts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_exam_attempt_answers_option",
                        column: x => x.selected_option_id,
                        principalTable: "question_options",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_exam_attempt_answers_variant_question",
                        column: x => x.exam_variant_question_id,
                        principalTable: "exam_variant_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "technical_incidents",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_attempt_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    incident_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    occurred_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    reported_by_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    handled_by_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_technical_incidents", x => x.id);
                    table.ForeignKey(
                        name: "fk_technical_incidents_attempt",
                        column: x => x.exam_attempt_id,
                        principalTable: "exam_attempts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_technical_incidents_handler",
                        column: x => x.handled_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_technical_incidents_reporter",
                        column: x => x.reported_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "violations",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    exam_attempt_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    violation_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    decision = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    handled_by_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    occurred_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_violations", x => x.id);
                    table.ForeignKey(
                        name: "fk_violations_attempt",
                        column: x => x.exam_attempt_id,
                        principalTable: "exam_attempts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_violations_handler",
                        column: x => x.handled_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "idx_academic_contexts_subject_grade",
                table: "academic_contexts",
                columns: new[] { "subject_id", "grade_level_id" });

            migrationBuilder.CreateIndex(
                name: "IX_academic_contexts_grade_level_id",
                table: "academic_contexts",
                column: "grade_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_academic_contexts_school_branch_id_school_id",
                table: "academic_contexts",
                columns: new[] { "school_branch_id", "school_id" });

            migrationBuilder.CreateIndex(
                name: "IX_academic_contexts_school_id",
                table: "academic_contexts",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "IX_academic_contexts_textbook_id",
                table: "academic_contexts",
                column: "textbook_id");

            migrationBuilder.CreateIndex(
                name: "uq_academic_contexts_scope",
                table: "academic_contexts",
                columns: new[] { "academic_year_id", "school_id", "school_branch_id", "textbook_id", "subject_id", "grade_level_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_academic_years_name",
                table: "academic_years",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_classes_grade",
                table: "classes",
                column: "grade_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_academic_year_id",
                table: "classes",
                column: "academic_year_id");

            migrationBuilder.CreateIndex(
                name: "uq_classes_branch_year_name",
                table: "classes",
                columns: new[] { "school_branch_id", "academic_year_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_exam_attempt_answers_selected_option",
                table: "exam_attempt_answers",
                column: "selected_option_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempt_answers_exam_variant_question_id",
                table: "exam_attempt_answers",
                column: "exam_variant_question_id");

            migrationBuilder.CreateIndex(
                name: "uq_exam_attempt_answers_question",
                table: "exam_attempt_answers",
                columns: new[] { "exam_attempt_id", "exam_variant_question_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_exam_attempts_status",
                table: "exam_attempts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_exam_attempts_variant",
                table: "exam_attempts",
                column: "exam_variant_id");

            migrationBuilder.CreateIndex(
                name: "uq_exam_attempts_registration",
                table: "exam_attempts",
                column: "exam_registration_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_exam_matrices_context",
                table: "exam_matrices",
                column: "academic_context_id");

            migrationBuilder.CreateIndex(
                name: "idx_exam_matrices_semester",
                table: "exam_matrices",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "uq_exam_matrices_task",
                table: "exam_matrices",
                column: "task_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_exam_proctors_teacher",
                table: "exam_proctors",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "uq_exam_proctors_exam_teacher",
                table: "exam_proctors",
                columns: new[] { "exam_id", "teacher_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_exam_registrations_session_room",
                table: "exam_registrations",
                column: "session_room_id");

            migrationBuilder.CreateIndex(
                name: "idx_exam_registrations_student",
                table: "exam_registrations",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "uq_exam_registrations_scope_student",
                table: "exam_registrations",
                columns: new[] { "exam_subject_grade_level_id", "student_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_exam_rooms_room",
                table: "exam_rooms",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "uq_exam_rooms_exam_room",
                table: "exam_rooms",
                columns: new[] { "exam_id", "room_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_exam_sessions_time",
                table: "exam_sessions",
                columns: new[] { "start_at", "end_at" });

            migrationBuilder.CreateIndex(
                name: "uq_exam_sessions_scope_code",
                table: "exam_sessions",
                columns: new[] { "exam_subject_grade_level_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_exam_set_questions_matrix_detail",
                table: "exam_set_questions",
                column: "matrix_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_set_questions_question_id",
                table: "exam_set_questions",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "uq_exam_set_questions_question",
                table: "exam_set_questions",
                columns: new[] { "exam_set_id", "question_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_exam_sets_approver",
                table: "exam_sets",
                column: "approved_by_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_exam_sets_creator",
                table: "exam_sets",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_exam_sets_matrix",
                table: "exam_sets",
                column: "exam_matrix_id");

            migrationBuilder.CreateIndex(
                name: "idx_exam_sets_source",
                table: "exam_sets",
                column: "source_exam_set_id");

            migrationBuilder.CreateIndex(
                name: "uq_exam_sets_task",
                table: "exam_sets",
                column: "task_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_exam_subject_grade_level",
                table: "exam_subject_grade_levels",
                column: "grade_level_id");

            migrationBuilder.CreateIndex(
                name: "uq_exam_subject_grade",
                table: "exam_subject_grade_levels",
                columns: new[] { "exam_subject_id", "grade_level_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_exam_subject_grade_backup_set",
                table: "exam_subject_grade_levels",
                column: "backup_exam_set_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_exam_subject_grade_primary_set",
                table: "exam_subject_grade_levels",
                column: "primary_exam_set_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_exam_subjects_publisher",
                table: "exam_subjects",
                column: "result_published_by_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_exam_subjects_subject",
                table: "exam_subjects",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "uq_exam_subjects_exam_subject",
                table: "exam_subjects",
                columns: new[] { "exam_id", "subject_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exam_variant_questions_exam_set_question_id",
                table: "exam_variant_questions",
                column: "exam_set_question_id");

            migrationBuilder.CreateIndex(
                name: "uq_exam_variant_questions_position",
                table: "exam_variant_questions",
                columns: new[] { "exam_variant_id", "position_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_exam_variant_questions_question",
                table: "exam_variant_questions",
                columns: new[] { "exam_variant_id", "exam_set_question_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_exam_variants_set_code",
                table: "exam_variants",
                columns: new[] { "exam_set_id", "variant_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_exams_branch_semester",
                table: "exams",
                columns: new[] { "school_branch_id", "semester_id" });

            migrationBuilder.CreateIndex(
                name: "IX_exams_semester_id",
                table: "exams",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "uq_grade_levels_name",
                table: "grade_levels",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_matrix_details_lesson",
                table: "matrix_details",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "uq_matrix_details_cell",
                table: "matrix_details",
                columns: new[] { "exam_matrix_id", "lesson_id", "cognitive_level", "question_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_modules_name",
                table: "modules",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_navbars_module_parent_order",
                table: "navbars",
                columns: new[] { "module_id", "parent_id", "display_order" });

            migrationBuilder.CreateIndex(
                name: "IX_navbars_parent_id",
                table: "navbars",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "idx_notification_configs_branch_active",
                table: "notification_configs",
                columns: new[] { "school_branch_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "idx_notification_configs_school_active",
                table: "notification_configs",
                columns: new[] { "school_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_configs_school_branch_id_school_id",
                table: "notification_configs",
                columns: new[] { "school_branch_id", "school_id" });

            migrationBuilder.CreateIndex(
                name: "uq_notification_configs_branch_override",
                table: "notification_configs",
                columns: new[] { "base_config_id", "school_branch_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_notification_configs_parent_match",
                table: "notification_configs",
                columns: new[] { "id", "school_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_notification_recipients_delivery",
                table: "notification_recipients",
                columns: new[] { "email_status", "notification_id" });

            migrationBuilder.CreateIndex(
                name: "idx_notification_recipients_inbox",
                table: "notification_recipients",
                columns: new[] { "user_id", "read_at", "notification_id" });

            migrationBuilder.CreateIndex(
                name: "uq_notification_recipients_notification_user",
                table: "notification_recipients",
                columns: new[] { "notification_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_notification_targets_role",
                table: "notification_targets",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "idx_notification_targets_user",
                table: "notification_targets",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uq_notification_targets_role",
                table: "notification_targets",
                columns: new[] { "config_id", "role_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_notification_targets_user",
                table: "notification_targets",
                columns: new[] { "config_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_notifications_branch_created",
                table: "notifications",
                columns: new[] { "school_branch_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "idx_notifications_config",
                table: "notifications",
                column: "config_id");

            migrationBuilder.CreateIndex(
                name: "idx_notifications_due",
                table: "notifications",
                columns: new[] { "scheduled_for", "created_at" });

            migrationBuilder.CreateIndex(
                name: "idx_notifications_school_created",
                table: "notifications",
                columns: new[] { "school_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_notifications_school_branch_id_school_id",
                table: "notifications",
                columns: new[] { "school_branch_id", "school_id" });

            migrationBuilder.CreateIndex(
                name: "idx_permissions_navbar",
                table: "permissions",
                column: "navbar_id");

            migrationBuilder.CreateIndex(
                name: "idx_proctor_assignments_proctor",
                table: "proctor_assignments",
                column: "exam_proctor_id");

            migrationBuilder.CreateIndex(
                name: "uq_proctor_assignments_room_proctor",
                table: "proctor_assignments",
                columns: new[] { "session_room_id", "exam_proctor_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_question_banks_subject_grade",
                table: "question_banks",
                columns: new[] { "subject_id", "grade_level_id" });

            migrationBuilder.CreateIndex(
                name: "IX_question_banks_grade_level_id",
                table: "question_banks",
                column: "grade_level_id");

            migrationBuilder.CreateIndex(
                name: "uq_question_banks_scope",
                table: "question_banks",
                columns: new[] { "school_branch_id", "subject_id", "grade_level_id", "bank_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_question_options_key",
                table: "question_options",
                columns: new[] { "question_id", "option_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_question_options_order",
                table: "question_options",
                columns: new[] { "question_id", "sort_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_question_task_details_target",
                table: "question_task_details",
                columns: new[] { "question_task_id", "cognitive_level", "question_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_question_tasks_lesson",
                table: "question_tasks",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "uq_question_tasks_task",
                table: "question_tasks",
                column: "task_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_questions_bank_status",
                table: "questions",
                columns: new[] { "question_bank_id", "status" });

            migrationBuilder.CreateIndex(
                name: "idx_questions_creator",
                table: "questions",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_questions_reviewer",
                table: "questions",
                column: "reviewed_by_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_questions_task_detail",
                table: "questions",
                column: "question_task_detail_id");

            migrationBuilder.CreateIndex(
                name: "uq_roles_code",
                table: "roles",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_rooms_branch_code",
                table: "rooms",
                columns: new[] { "school_branch_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_school_branches_school_code",
                table: "school_branches",
                columns: new[] { "school_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_schools_code",
                table: "schools",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_semesters_year_name",
                table: "semesters",
                columns: new[] { "academic_year_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_session_rooms_exam_room",
                table: "session_rooms",
                column: "exam_room_id");

            migrationBuilder.CreateIndex(
                name: "uq_session_rooms_session_room",
                table: "session_rooms",
                columns: new[] { "exam_session_id", "exam_room_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_students_class",
                table: "students",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "uq_students_user",
                table: "students",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_subjects_name",
                table: "subjects",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_tasks_assignee_status_due",
                table: "tasks",
                columns: new[] { "assigned_to_user_id", "status", "due_at" });

            migrationBuilder.CreateIndex(
                name: "idx_tasks_creator",
                table: "tasks",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_tasks_updater",
                table: "tasks",
                column: "updated_by_user_id");

            migrationBuilder.CreateIndex(
                name: "uq_teachers_homeroom_class",
                table: "teachers",
                column: "class_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_teachers_user",
                table: "teachers",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_technical_incidents_attempt",
                table: "technical_incidents",
                column: "exam_attempt_id");

            migrationBuilder.CreateIndex(
                name: "idx_technical_incidents_handler",
                table: "technical_incidents",
                column: "handled_by_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_technical_incidents_reporter",
                table: "technical_incidents",
                column: "reported_by_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_technical_incidents_status_time",
                table: "technical_incidents",
                columns: new[] { "status", "occurred_at" });

            migrationBuilder.CreateIndex(
                name: "uq_textbook_chapters_order",
                table: "textbook_chapters",
                columns: new[] { "textbook_id", "sort_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_textbook_lessons_order",
                table: "textbook_lessons",
                columns: new[] { "chapter_id", "sort_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_textbooks_title",
                table: "textbooks",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "idx_user_roles_role_user",
                table: "user_roles",
                columns: new[] { "role_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "uq_user_roles_user_role",
                table: "user_roles",
                columns: new[] { "user_id", "role_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_users_branch_status",
                table: "users",
                columns: new[] { "school_branch_id", "status" });

            migrationBuilder.CreateIndex(
                name: "uq_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_users_moet_identifier",
                table: "users",
                column: "moet_identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_violations_attempt",
                table: "violations",
                column: "exam_attempt_id");

            migrationBuilder.CreateIndex(
                name: "idx_violations_handler",
                table: "violations",
                column: "handled_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_notification_configs_base_match",
                table: "notification_configs",
                columns: new[] { "base_config_id", "school_id", "code" },
                principalTable: "notification_configs",
                principalColumns: new[] { "id", "school_id", "code" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_notification_configs_base_match",
                table: "notification_configs");

            migrationBuilder.DropTable(
                name: "exam_attempt_answers");

            migrationBuilder.DropTable(
                name: "notification_recipients");

            migrationBuilder.DropTable(
                name: "notification_targets");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "proctor_assignments");

            migrationBuilder.DropTable(
                name: "technical_incidents");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "violations");

            migrationBuilder.DropTable(
                name: "question_options");

            migrationBuilder.DropTable(
                name: "exam_variant_questions");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "navbars");

            migrationBuilder.DropTable(
                name: "exam_proctors");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "exam_attempts");

            migrationBuilder.DropTable(
                name: "exam_set_questions");

            migrationBuilder.DropTable(
                name: "notification_configs");

            migrationBuilder.DropTable(
                name: "modules");

            migrationBuilder.DropTable(
                name: "teachers");

            migrationBuilder.DropTable(
                name: "exam_registrations");

            migrationBuilder.DropTable(
                name: "exam_variants");

            migrationBuilder.DropTable(
                name: "matrix_details");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "session_rooms");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "question_banks");

            migrationBuilder.DropTable(
                name: "question_task_details");

            migrationBuilder.DropTable(
                name: "exam_rooms");

            migrationBuilder.DropTable(
                name: "exam_sessions");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "question_tasks");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "exam_subject_grade_levels");

            migrationBuilder.DropTable(
                name: "textbook_lessons");

            migrationBuilder.DropTable(
                name: "exam_sets");

            migrationBuilder.DropTable(
                name: "exam_subjects");

            migrationBuilder.DropTable(
                name: "textbook_chapters");

            migrationBuilder.DropTable(
                name: "exam_matrices");

            migrationBuilder.DropTable(
                name: "exams");

            migrationBuilder.DropTable(
                name: "academic_contexts");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "semesters");

            migrationBuilder.DropTable(
                name: "grade_levels");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "textbooks");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "academic_years");

            migrationBuilder.DropTable(
                name: "school_branches");

            migrationBuilder.DropTable(
                name: "schools");
        }
    }
}
