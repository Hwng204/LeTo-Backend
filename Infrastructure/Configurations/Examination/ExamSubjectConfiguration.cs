using Domain.Entities.Examination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Examination;

public sealed class ExamSubjectConfiguration : IEntityTypeConfiguration<ExamSubject>
{
    public void Configure(EntityTypeBuilder<ExamSubject> builder)
    {
        builder.ToTable("exam_subjects", table => table.HasCheckConstraint("ck_exam_subjects_duration", "duration_minutes > 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamId).HasColumnName("exam_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.SubjectId).HasColumnName("subject_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.DurationMinutes).HasColumnName("duration_minutes").HasColumnType("int unsigned").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).IsRequired();
        builder.Property(x => x.ResultPublishedAt).HasColumnName("result_published_at").HasColumnType("datetime(6)");
        builder.Property(x => x.ResultPublishedByUserId).HasColumnName("result_published_by_user_id").HasColumnType("bigint unsigned");
        builder.HasIndex(x => new { x.ExamId, x.SubjectId }).IsUnique().HasDatabaseName("uq_exam_subjects_exam_subject");
        builder.HasIndex(x => x.SubjectId).HasDatabaseName("idx_exam_subjects_subject");
        builder.HasIndex(x => x.ResultPublishedByUserId).HasDatabaseName("idx_exam_subjects_publisher");
        builder.HasOne(x => x.Exam).WithMany(x => x.Subjects)
            .HasForeignKey(x => x.ExamId).HasConstraintName("fk_exam_subjects_exam")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Subject).WithMany()
            .HasForeignKey(x => x.SubjectId).HasConstraintName("fk_exam_subjects_subject")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ResultPublishedByUser).WithMany()
            .HasForeignKey(x => x.ResultPublishedByUserId).HasConstraintName("fk_exam_subjects_publisher")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
