using Domain.Entities.Examination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Examination;

public sealed class TechnicalIncidentConfiguration : IEntityTypeConfiguration<TechnicalIncident>
{
    public void Configure(EntityTypeBuilder<TechnicalIncident> builder)
    {
        builder.ToTable("technical_incidents");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamAttemptId).HasColumnName("exam_attempt_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.IncidentType).HasColumnName("incident_type").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasColumnName("description").HasColumnType("text").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).IsRequired();
        builder.Property(x => x.OccurredAt).HasColumnName("occurred_at").HasColumnType("datetime(6)").IsRequired();
        builder.Property(x => x.ReportedByUserId).HasColumnName("reported_by_user_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.HandledByUserId).HasColumnName("handled_by_user_id").HasColumnType("bigint unsigned");
        builder.HasIndex(x => x.ExamAttemptId).HasDatabaseName("idx_technical_incidents_attempt");
        builder.HasIndex(x => new { x.Status, x.OccurredAt }).HasDatabaseName("idx_technical_incidents_status_time");
        builder.HasIndex(x => x.ReportedByUserId).HasDatabaseName("idx_technical_incidents_reporter");
        builder.HasIndex(x => x.HandledByUserId).HasDatabaseName("idx_technical_incidents_handler");
        builder.HasOne(x => x.ExamAttempt).WithMany(x => x.TechnicalIncidents)
            .HasForeignKey(x => x.ExamAttemptId).HasConstraintName("fk_technical_incidents_attempt")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ReportedByUser).WithMany()
            .HasForeignKey(x => x.ReportedByUserId).HasConstraintName("fk_technical_incidents_reporter")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.HandledByUser).WithMany()
            .HasForeignKey(x => x.HandledByUserId).HasConstraintName("fk_technical_incidents_handler")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
