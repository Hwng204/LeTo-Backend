using Domain.Entities.Examination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Examination;

public sealed class ExamSubjectGradeLevelConfiguration : IEntityTypeConfiguration<ExamSubjectGradeLevel>
{
    public void Configure(EntityTypeBuilder<ExamSubjectGradeLevel> builder)
    {
        builder.ToTable("exam_subject_grade_levels");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamSubjectId).HasColumnName("exam_subject_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.GradeLevelId).HasColumnName("grade_level_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.PrimaryExamSetId).HasColumnName("primary_exam_set_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.BackupExamSetId).HasColumnName("backup_exam_set_id").HasColumnType("bigint unsigned");
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).IsRequired();
        builder.HasIndex(x => new { x.ExamSubjectId, x.GradeLevelId }).IsUnique().HasDatabaseName("uq_exam_subject_grade");
        builder.HasIndex(x => x.PrimaryExamSetId).IsUnique().HasDatabaseName("uq_exam_subject_grade_primary_set");
        builder.HasIndex(x => x.BackupExamSetId).IsUnique().HasDatabaseName("uq_exam_subject_grade_backup_set");
        builder.HasIndex(x => x.GradeLevelId).HasDatabaseName("idx_exam_subject_grade_level");
        builder.HasOne(x => x.ExamSubject).WithMany(x => x.GradeLevels)
            .HasForeignKey(x => x.ExamSubjectId).HasConstraintName("fk_exam_subject_grade_subject")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.GradeLevel).WithMany()
            .HasForeignKey(x => x.GradeLevelId).HasConstraintName("fk_exam_subject_grade_grade")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.PrimaryExamSet).WithMany()
            .HasForeignKey(x => x.PrimaryExamSetId).HasConstraintName("fk_exam_subject_grade_primary_set")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.BackupExamSet).WithMany()
            .HasForeignKey(x => x.BackupExamSetId).HasConstraintName("fk_exam_subject_grade_backup_set")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
