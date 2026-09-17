using Domain.Entities.QuestionBank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.QuestionBank;

public sealed class ExamSetConfiguration : IEntityTypeConfiguration<ExamSet>
{
    public void Configure(EntityTypeBuilder<ExamSet> builder)
    {
        builder.ToTable("exam_sets");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamMatrixId).HasColumnName("exam_matrix_id").HasColumnType("bigint unsigned");
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).IsRequired();
        builder.Property(x => x.TaskId).HasColumnName("task_id").HasColumnType("bigint unsigned");
        builder.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.Purpose).HasColumnName("purpose").HasMaxLength(100).IsRequired();
        builder.Property(x => x.ApprovedByUserId).HasColumnName("approved_by_user_id").HasColumnType("bigint unsigned");
        builder.Property(x => x.ApprovedAt).HasColumnName("approved_at").HasColumnType("datetime(6)");
        builder.Property(x => x.SourceExamSetId).HasColumnName("source_exam_set_id").HasColumnType("bigint unsigned");
        builder.HasIndex(x => x.TaskId).IsUnique().HasDatabaseName("uq_exam_sets_task");
        builder.HasIndex(x => x.ExamMatrixId).HasDatabaseName("idx_exam_sets_matrix");
        builder.HasIndex(x => x.CreatedByUserId).HasDatabaseName("idx_exam_sets_creator");
        builder.HasIndex(x => x.ApprovedByUserId).HasDatabaseName("idx_exam_sets_approver");
        builder.HasIndex(x => x.SourceExamSetId).HasDatabaseName("idx_exam_sets_source");
        builder.HasOne(x => x.ExamMatrix).WithMany()
            .HasForeignKey(x => x.ExamMatrixId).HasConstraintName("fk_exam_sets_matrix")
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(x => x.Task).WithMany()
            .HasForeignKey(x => x.TaskId).HasConstraintName("fk_exam_sets_task")
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(x => x.CreatedByUser).WithMany()
            .HasForeignKey(x => x.CreatedByUserId).HasConstraintName("fk_exam_sets_creator")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ApprovedByUser).WithMany()
            .HasForeignKey(x => x.ApprovedByUserId).HasConstraintName("fk_exam_sets_approver")
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(x => x.SourceExamSet).WithMany(x => x.DerivedExamSets)
            .HasForeignKey(x => x.SourceExamSetId).HasConstraintName("fk_exam_sets_source")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
