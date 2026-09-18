using Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Organization;

public sealed class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.ToTable("schools");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).HasDefaultValue("ACTIVE").IsRequired();
        builder.Property(x => x.ProvinceCode).HasColumnName("province_code").HasMaxLength(2);
        builder.HasIndex(x => x.Code).IsUnique().HasDatabaseName("uq_schools_code");
        builder.HasIndex(x => x.ProvinceCode).HasDatabaseName("ix_schools_province_code");
        builder.HasOne(x => x.Province).WithMany(x => x.Schools)
            .HasForeignKey(x => x.ProvinceCode).HasConstraintName("fk_schools_province")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
