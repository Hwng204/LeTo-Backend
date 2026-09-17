using Domain.Entities.Examination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Examination;

public sealed class ExamProctorConfiguration : IEntityTypeConfiguration<ExamProctor>
{
    public void Configure(EntityTypeBuilder<ExamProctor> builder)
    {
        builder.ToTable("exam_proctors");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamId).HasColumnName("exam_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.TeacherId).HasColumnName("teacher_id").HasColumnType("bigint unsigned").IsRequired();
        builder.HasIndex(x => new { x.ExamId, x.TeacherId })
            .IsUnique().HasDatabaseName("uq_exam_proctors_exam_teacher");
        builder.HasIndex(x => x.TeacherId).HasDatabaseName("idx_exam_proctors_teacher");
        builder.HasOne(x => x.Exam).WithMany(x => x.Proctors)
            .HasForeignKey(x => x.ExamId).HasConstraintName("fk_exam_proctors_exam")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Teacher).WithMany()
            .HasForeignKey(x => x.TeacherId).HasConstraintName("fk_exam_proctors_teacher")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
