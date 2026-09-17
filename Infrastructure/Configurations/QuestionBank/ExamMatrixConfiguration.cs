using Domain.Entities.QuestionBank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.QuestionBank;

public sealed class ExamMatrixConfiguration : IEntityTypeConfiguration<ExamMatrix>
{
    public void Configure(EntityTypeBuilder<ExamMatrix> builder)
    {
        builder.ToTable("exam_matrices");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).IsRequired();
        builder.Property(x => x.TaskId).HasColumnName("task_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.SemesterId).HasColumnName("semester_id").HasColumnType("bigint unsigned");
        builder.Property(x => x.AcademicContextId).HasColumnName("academic_context_id").HasColumnType("bigint unsigned").IsRequired();
        builder.HasIndex(x => x.TaskId).IsUnique().HasDatabaseName("uq_exam_matrices_task");
        builder.HasIndex(x => x.AcademicContextId).HasDatabaseName("idx_exam_matrices_context");
        builder.HasIndex(x => x.SemesterId).HasDatabaseName("idx_exam_matrices_semester");
        builder.HasOne(x => x.Task).WithMany()
            .HasForeignKey(x => x.TaskId).HasConstraintName("fk_exam_matrices_task")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Semester).WithMany()
            .HasForeignKey(x => x.SemesterId).HasConstraintName("fk_exam_matrices_semester")
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(x => x.AcademicContext).WithMany()
            .HasForeignKey(x => x.AcademicContextId).HasConstraintName("fk_exam_matrices_context")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
