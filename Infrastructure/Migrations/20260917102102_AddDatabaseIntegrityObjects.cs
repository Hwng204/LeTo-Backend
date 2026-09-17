using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseIntegrityObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "base_scope_code_key",
                table: "notification_configs",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                computedColumnSql: "CASE WHEN school_branch_id IS NULL AND base_config_id IS NULL THEN code ELSE NULL END",
                stored: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<ulong>(
                name: "base_scope_school_key",
                table: "notification_configs",
                type: "bigint unsigned",
                nullable: true,
                computedColumnSql: "CASE WHEN school_branch_id IS NULL AND base_config_id IS NULL THEN COALESCE(school_id, 0) ELSE NULL END",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "uq_notification_configs_base_scope",
                table: "notification_configs",
                columns: new[] { "base_scope_school_key", "base_scope_code_key" },
                unique: true);

            migrationBuilder.Sql("""
                CREATE TRIGGER trg_exam_sets_bi
                BEFORE INSERT ON exam_sets
                FOR EACH ROW
                BEGIN
                  IF (NEW.approved_at IS NULL) <> (NEW.approved_by_user_id IS NULL) THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'approved_at and approved_by_user_id must be set together';
                  END IF;

                  IF NEW.source_exam_set_id IS NOT NULL
                     AND NEW.id IS NOT NULL
                     AND NEW.id <> 0
                     AND NEW.source_exam_set_id = NEW.id THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'an exam set cannot be its own source';
                  END IF;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER trg_exam_sets_ai
                AFTER INSERT ON exam_sets
                FOR EACH ROW
                BEGIN
                  IF NEW.source_exam_set_id IS NOT NULL AND NEW.source_exam_set_id = NEW.id THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'an exam set cannot be its own source';
                  END IF;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER trg_exam_sets_bu
                BEFORE UPDATE ON exam_sets
                FOR EACH ROW
                BEGIN
                  IF (NEW.approved_at IS NULL) <> (NEW.approved_by_user_id IS NULL) THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'approved_at and approved_by_user_id must be set together';
                  END IF;

                  IF NEW.source_exam_set_id IS NOT NULL AND NEW.source_exam_set_id = NEW.id THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'an exam set cannot be its own source';
                  END IF;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER trg_exam_subjects_bi
                BEFORE INSERT ON exam_subjects
                FOR EACH ROW
                BEGIN
                  IF (NEW.result_published_at IS NULL)
                     <> (NEW.result_published_by_user_id IS NULL) THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'result publication time and publisher must be set together';
                  END IF;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER trg_exam_subjects_bu
                BEFORE UPDATE ON exam_subjects
                FOR EACH ROW
                BEGIN
                  IF (NEW.result_published_at IS NULL)
                     <> (NEW.result_published_by_user_id IS NULL) THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'result publication time and publisher must be set together';
                  END IF;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER trg_exam_subject_grade_levels_bi
                BEFORE INSERT ON exam_subject_grade_levels
                FOR EACH ROW
                BEGIN
                  IF NEW.backup_exam_set_id IS NOT NULL
                     AND NEW.backup_exam_set_id = NEW.primary_exam_set_id THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'primary and backup exam sets must be different';
                  END IF;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER trg_exam_subject_grade_levels_bu
                BEFORE UPDATE ON exam_subject_grade_levels
                FOR EACH ROW
                BEGIN
                  IF NEW.backup_exam_set_id IS NOT NULL
                     AND NEW.backup_exam_set_id = NEW.primary_exam_set_id THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'primary and backup exam sets must be different';
                  END IF;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER trg_notification_targets_bi
                BEFORE INSERT ON notification_targets
                FOR EACH ROW
                BEGIN
                  IF NOT (
                    (NEW.role_id IS NOT NULL AND NEW.user_id IS NULL)
                    OR (NEW.role_id IS NULL AND NEW.user_id IS NOT NULL)
                  ) THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'exactly one of role_id or user_id is required';
                  END IF;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER trg_notification_targets_bu
                BEFORE UPDATE ON notification_targets
                FOR EACH ROW
                BEGIN
                  IF NOT (
                    (NEW.role_id IS NOT NULL AND NEW.user_id IS NULL)
                    OR (NEW.role_id IS NULL AND NEW.user_id IS NOT NULL)
                  ) THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'exactly one of role_id or user_id is required';
                  END IF;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER trg_notification_configs_bi
                BEFORE INSERT ON notification_configs
                FOR EACH ROW
                BEGIN
                  DECLARE v_parent_branch_id BIGINT UNSIGNED;
                  DECLARE v_parent_base_id BIGINT UNSIGNED;
                  DECLARE v_parent_event_code VARCHAR(100);

                  IF NOT (
                    (NEW.school_id IS NULL AND NEW.school_branch_id IS NULL AND NEW.base_config_id IS NULL)
                    OR
                    (NEW.school_id IS NOT NULL AND NEW.school_branch_id IS NULL AND NEW.base_config_id IS NULL)
                    OR
                    (NEW.school_id IS NOT NULL AND NEW.school_branch_id IS NOT NULL AND NEW.base_config_id IS NOT NULL)
                  ) THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'invalid system/school/branch notification config shape';
                  END IF;

                  IF NEW.base_config_id IS NOT NULL
                     AND NEW.id IS NOT NULL
                     AND NEW.id <> 0
                     AND NEW.base_config_id = NEW.id THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'a notification config cannot inherit from itself';
                  END IF;

                  IF NEW.base_config_id IS NOT NULL THEN
                    SET v_parent_branch_id = NULL;
                    SET v_parent_base_id = NULL;
                    SET v_parent_event_code = NULL;

                    SELECT school_branch_id, base_config_id, event_code
                      INTO v_parent_branch_id, v_parent_base_id, v_parent_event_code
                    FROM notification_configs
                    WHERE id = NEW.base_config_id
                    LIMIT 1;

                    IF v_parent_branch_id IS NOT NULL OR v_parent_base_id IS NOT NULL THEN
                      SIGNAL SQLSTATE '45000'
                        SET MESSAGE_TEXT = 'base_config_id must reference a school-level base config';
                    END IF;

                    IF NOT (v_parent_event_code <=> NEW.event_code) THEN
                      SIGNAL SQLSTATE '45000'
                        SET MESSAGE_TEXT = 'branch override must keep the base event_code';
                    END IF;
                  END IF;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER trg_notification_configs_bu
                BEFORE UPDATE ON notification_configs
                FOR EACH ROW
                BEGIN
                  DECLARE v_parent_branch_id BIGINT UNSIGNED;
                  DECLARE v_parent_base_id BIGINT UNSIGNED;
                  DECLARE v_parent_event_code VARCHAR(100);

                  IF NOT (
                    (NEW.school_id IS NULL AND NEW.school_branch_id IS NULL AND NEW.base_config_id IS NULL)
                    OR
                    (NEW.school_id IS NOT NULL AND NEW.school_branch_id IS NULL AND NEW.base_config_id IS NULL)
                    OR
                    (NEW.school_id IS NOT NULL AND NEW.school_branch_id IS NOT NULL AND NEW.base_config_id IS NOT NULL)
                  ) THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'invalid system/school/branch notification config shape';
                  END IF;

                  IF NEW.base_config_id IS NOT NULL AND NEW.base_config_id = NEW.id THEN
                    SIGNAL SQLSTATE '45000'
                      SET MESSAGE_TEXT = 'a notification config cannot inherit from itself';
                  END IF;

                  IF NEW.base_config_id IS NOT NULL THEN
                    SET v_parent_branch_id = NULL;
                    SET v_parent_base_id = NULL;
                    SET v_parent_event_code = NULL;

                    SELECT school_branch_id, base_config_id, event_code
                      INTO v_parent_branch_id, v_parent_base_id, v_parent_event_code
                    FROM notification_configs
                    WHERE id = NEW.base_config_id
                    LIMIT 1;

                    IF v_parent_branch_id IS NOT NULL OR v_parent_base_id IS NOT NULL THEN
                      SIGNAL SQLSTATE '45000'
                        SET MESSAGE_TEXT = 'base_config_id must reference a school-level base config';
                    END IF;

                    IF NOT (v_parent_event_code <=> NEW.event_code) THEN
                      SIGNAL SQLSTATE '45000'
                        SET MESSAGE_TEXT = 'branch override must keep the base event_code';
                    END IF;
                  END IF;
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_notification_configs_bu;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_notification_configs_bi;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_notification_targets_bu;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_notification_targets_bi;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_exam_subject_grade_levels_bu;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_exam_subject_grade_levels_bi;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_exam_subjects_bu;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_exam_subjects_bi;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_exam_sets_bu;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_exam_sets_ai;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_exam_sets_bi;");

            migrationBuilder.DropIndex(
                name: "uq_notification_configs_base_scope",
                table: "notification_configs");

            migrationBuilder.DropColumn(
                name: "base_scope_code_key",
                table: "notification_configs");

            migrationBuilder.DropColumn(
                name: "base_scope_school_key",
                table: "notification_configs");
        }
    }
}
