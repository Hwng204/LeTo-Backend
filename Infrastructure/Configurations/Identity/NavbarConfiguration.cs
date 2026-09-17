using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Identity;

public sealed class NavbarConfiguration : IEntityTypeConfiguration<Navbar>
{
    public void Configure(EntityTypeBuilder<Navbar> builder)
    {
        builder.ToTable("navbars");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ModuleId).HasColumnName("module_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.ParentId).HasColumnName("parent_id").HasColumnType("bigint unsigned");
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(x => x.DisplayOrder).HasColumnName("display_order").HasColumnType("int unsigned").HasDefaultValue(0u).IsRequired();
        builder.Property(x => x.UrlPath).HasColumnName("url_path").HasMaxLength(512);
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).HasDefaultValue("ACTIVE").IsRequired();
        builder.HasIndex(x => new { x.ModuleId, x.ParentId, x.DisplayOrder })
            .HasDatabaseName("idx_navbars_module_parent_order");
        builder.HasOne(x => x.Module).WithMany(x => x.Navbars)
            .HasForeignKey(x => x.ModuleId).HasConstraintName("fk_navbars_module")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Parent).WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId).HasConstraintName("fk_navbars_parent")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
