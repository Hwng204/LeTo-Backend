using Domain.Entities.Examination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Examination;

public sealed class ExamAttemptConfiguration : IEntityTypeConfiguration<ExamAttempt>
{
    public void Configure(EntityTypeBuilder<ExamAttempt> builder)
    {
        builder.ToTable("exam_attempts", table =>
        {
            table.HasCheckConstraint("ck_exam_attempts_time", "submitted_at IS NULL OR submitted_at >= started_at");
            table.HasCheckConstraint("ck_exam_attempts_score", "total_score IS NULL OR total_score >= 0");
        });
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamRegistrationId).HasColumnName("exam_registration_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.ExamVariantId).HasColumnName("exam_variant_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).IsRequired();
        builder.Property(x => x.StartedAt).HasColumnName("started_at").HasColumnType("datetime(6)").IsRequired();
        builder.Property(x => x.SubmittedAt).HasColumnName("submitted_at").HasColumnType("datetime(6)");
        builder.Property(x => x.TotalScore).HasColumnName("total_score").HasPrecision(5, 2);
        builder.Property(x => x.Note).HasColumnName("note").HasColumnType("text");
        builder.HasIndex(x => x.ExamRegistrationId).IsUnique().HasDatabaseName("uq_exam_attempts_registration");
        builder.HasIndex(x => x.ExamVariantId).HasDatabaseName("idx_exam_attempts_variant");
        builder.HasIndex(x => x.Status).HasDatabaseName("idx_exam_attempts_status");
        builder.HasOne(x => x.ExamRegistration).WithOne(x => x.Attempt)
            .HasForeignKey<ExamAttempt>(x => x.ExamRegistrationId).HasConstraintName("fk_exam_attempts_registration")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ExamVariant).WithMany()
            .HasForeignKey(x => x.ExamVariantId).HasConstraintName("fk_exam_attempts_variant")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
