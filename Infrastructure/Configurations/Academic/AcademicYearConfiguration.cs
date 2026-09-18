using Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Academic;

public sealed class AcademicYearConfiguration : IEntityTypeConfiguration<AcademicYear>
{
    public void Configure(EntityTypeBuilder<AcademicYear> builder)
    {
        builder.ToTable("academic_years", table =>
        {
            table.HasCheckConstraint("ck_academic_years_dates", "end_date > start_date");
            table.HasCheckConstraint("ck_academic_years_status", "status IN ('DRAFT', 'ACTIVE', 'CLOSED')");
        });
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(x => x.StartDate).HasColumnName("start_date").HasColumnType("date").IsRequired();
        builder.Property(x => x.EndDate).HasColumnName("end_date").HasColumnType("date").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).HasDefaultValue("DRAFT").IsRequired();
        builder.Property(x => x.ProvinceCode).HasColumnName("province_code").HasMaxLength(2);
        builder.Property(x => x.Version).HasColumnName("version").HasDefaultValue(1u).IsConcurrencyToken();
        builder.HasIndex(x => new { x.ProvinceCode, x.Name })
            .IsUnique().HasDatabaseName("uq_academic_years_province_name");
        builder.HasOne(x => x.Province).WithMany(x => x.AcademicYears)
            .HasForeignKey(x => x.ProvinceCode).HasConstraintName("fk_academic_years_province")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
