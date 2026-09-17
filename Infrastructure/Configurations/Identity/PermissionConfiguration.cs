using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Identity;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(x => new { x.RoleId, x.NavbarId });
        builder.Property(x => x.RoleId).HasColumnName("role_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.NavbarId).HasColumnName("navbar_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.PermissionMask).HasColumnName("permission").HasColumnType("int unsigned").HasDefaultValue(0u).IsRequired();
        builder.HasIndex(x => x.NavbarId).HasDatabaseName("idx_permissions_navbar");
        builder.HasOne(x => x.Role).WithMany(x => x.Permissions)
            .HasForeignKey(x => x.RoleId).HasConstraintName("fk_permissions_role")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Navbar).WithMany(x => x.Permissions)
            .HasForeignKey(x => x.NavbarId).HasConstraintName("fk_permissions_navbar")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
