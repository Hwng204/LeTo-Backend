using Domain.Entities.Examination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Examination;

public sealed class ExamSessionConfiguration : IEntityTypeConfiguration<ExamSession>
{
    public void Configure(EntityTypeBuilder<ExamSession> builder)
    {
        builder.ToTable("exam_sessions", table => table.HasCheckConstraint("ck_exam_sessions_time", "end_at > start_at"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamSubjectGradeLevelId).HasColumnName("exam_subject_grade_level_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(x => x.StartAt).HasColumnName("start_at").HasColumnType("datetime(6)").IsRequired();
        builder.Property(x => x.EndAt).HasColumnName("end_at").HasColumnType("datetime(6)").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32);
        builder.Property(x => x.AccessCode).HasColumnName("access_code").HasMaxLength(255);
        builder.HasIndex(x => new { x.ExamSubjectGradeLevelId, x.Code })
            .IsUnique().HasDatabaseName("uq_exam_sessions_scope_code");
        builder.HasIndex(x => new { x.StartAt, x.EndAt }).HasDatabaseName("idx_exam_sessions_time");
        builder.HasOne(x => x.ExamSubjectGradeLevel).WithMany(x => x.Sessions)
            .HasForeignKey(x => x.ExamSubjectGradeLevelId).HasConstraintName("fk_exam_sessions_subject_grade")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
