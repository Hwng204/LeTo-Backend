using Domain.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Notification;

public sealed class NotificationTargetConfiguration : IEntityTypeConfiguration<NotificationTarget>
{
    public void Configure(EntityTypeBuilder<NotificationTarget> builder)
    {
        builder.ToTable("notification_targets", table =>
            table.HasCheckConstraint("ck_notification_targets_action", "action IN ('INCLUDE', 'EXCLUDE')"));

        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(entity => entity.ConfigId).HasColumnName("config_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(entity => entity.RoleId).HasColumnName("role_id").HasColumnType("bigint unsigned");
        builder.Property(entity => entity.UserId).HasColumnName("user_id").HasColumnType("bigint unsigned");
        builder.Property(entity => entity.Action).HasColumnName("action").HasMaxLength(10).IsRequired();

        builder.HasIndex(entity => new { entity.ConfigId, entity.RoleId })
            .IsUnique().HasDatabaseName("uq_notification_targets_role");
        builder.HasIndex(entity => new { entity.ConfigId, entity.UserId })
            .IsUnique().HasDatabaseName("uq_notification_targets_user");
        builder.HasIndex(entity => entity.RoleId).HasDatabaseName("idx_notification_targets_role");
        builder.HasIndex(entity => entity.UserId).HasDatabaseName("idx_notification_targets_user");

        builder.HasOne(entity => entity.Config)
            .WithMany(config => config.Targets)
            .HasForeignKey(entity => entity.ConfigId)
            .HasConstraintName("fk_notification_targets_config")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(entity => entity.Role)
            .WithMany()
            .HasForeignKey(entity => entity.RoleId)
            .HasConstraintName("fk_notification_targets_role")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(entity => entity.User)
            .WithMany()
            .HasForeignKey(entity => entity.UserId)
            .HasConstraintName("fk_notification_targets_user")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
