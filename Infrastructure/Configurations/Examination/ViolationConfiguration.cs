using Domain.Entities.Examination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Examination;

public sealed class ViolationConfiguration : IEntityTypeConfiguration<Violation>
{
    public void Configure(EntityTypeBuilder<Violation> builder)
    {
        builder.ToTable("violations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamAttemptId).HasColumnName("exam_attempt_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.ViolationType).HasColumnName("violation_type").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Decision).HasColumnName("decision").HasMaxLength(100).IsRequired();
        builder.Property(x => x.HandledByUserId).HasColumnName("handled_by_user_id").HasColumnType("bigint unsigned");
        builder.Property(x => x.Description).HasColumnName("description").HasColumnType("text");
        builder.Property(x => x.OccurredAt).HasColumnName("occurred_at").HasColumnType("datetime(6)").IsRequired();
        builder.HasIndex(x => x.ExamAttemptId).HasDatabaseName("idx_violations_attempt");
        builder.HasIndex(x => x.HandledByUserId).HasDatabaseName("idx_violations_handler");
        builder.HasOne(x => x.ExamAttempt).WithMany(x => x.Violations)
            .HasForeignKey(x => x.ExamAttemptId).HasConstraintName("fk_violations_attempt")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.HandledByUser).WithMany()
            .HasForeignKey(x => x.HandledByUserId).HasConstraintName("fk_violations_handler")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
