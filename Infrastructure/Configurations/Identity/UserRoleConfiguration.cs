using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Identity;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.RoleId).HasColumnName("role_id").HasColumnType("bigint unsigned").IsRequired();
        builder.HasIndex(x => new { x.UserId, x.RoleId }).IsUnique().HasDatabaseName("uq_user_roles_user_role");
        builder.HasIndex(x => new { x.RoleId, x.UserId }).HasDatabaseName("idx_user_roles_role_user");
        builder.HasOne(x => x.User).WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.UserId).HasConstraintName("fk_user_roles_user")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Role).WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.RoleId).HasConstraintName("fk_user_roles_role")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
