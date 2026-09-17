using Domain.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationEntity = Domain.Entities.Notification.Notification;

namespace Infrastructure.Configurations.Notification;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
{
    public void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        builder.ToTable("notifications");

        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(entity => entity.ConfigId).HasColumnName("config_id").HasColumnType("bigint unsigned");
        builder.Property(entity => entity.SchoolId).HasColumnName("school_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(entity => entity.SchoolBranchId).HasColumnName("school_branch_id").HasColumnType("bigint unsigned");
        builder.Property(entity => entity.ScheduledFor).HasColumnName("scheduled_for").HasColumnType("datetime(6)");
        builder.Property(entity => entity.Title).HasColumnName("title").HasMaxLength(500).IsRequired();
        builder.Property(entity => entity.Content).HasColumnName("content").HasColumnType("longtext").IsRequired();
        builder.Property(entity => entity.ActionUrl).HasColumnName("action_url").HasMaxLength(2048);
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at").HasColumnType("datetime(6)")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)").IsRequired();

        builder.HasIndex(entity => new { entity.ScheduledFor, entity.CreatedAt })
            .HasDatabaseName("idx_notifications_due");
        builder.HasIndex(entity => new { entity.SchoolId, entity.CreatedAt })
            .HasDatabaseName("idx_notifications_school_created");
        builder.HasIndex(entity => new { entity.SchoolBranchId, entity.CreatedAt })
            .HasDatabaseName("idx_notifications_branch_created");
        builder.HasIndex(entity => entity.ConfigId).HasDatabaseName("idx_notifications_config");

        builder.HasOne(entity => entity.Config)
            .WithMany(config => config.Notifications)
            .HasForeignKey(entity => entity.ConfigId)
            .HasConstraintName("fk_notifications_config")
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(entity => entity.School)
            .WithMany()
            .HasForeignKey(entity => entity.SchoolId)
            .HasConstraintName("fk_notifications_school")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(entity => entity.SchoolBranch)
            .WithMany()
            .HasForeignKey(entity => new { entity.SchoolBranchId, entity.SchoolId })
            .HasPrincipalKey(branch => new { branch.Id, branch.SchoolId })
            .HasConstraintName("fk_notifications_branch_school")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
