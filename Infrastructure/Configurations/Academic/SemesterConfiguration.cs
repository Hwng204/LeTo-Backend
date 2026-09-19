using Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Academic;

public sealed class SemesterConfiguration : IEntityTypeConfiguration<Semester>
{
    public void Configure(EntityTypeBuilder<Semester> builder)
    {
        builder.ToTable("semesters", table =>
        {
            table.HasCheckConstraint(
                "ck_semesters_dates",
                "(start_date IS NULL AND end_date IS NULL) OR end_date > start_date");
            table.HasCheckConstraint("ck_semesters_order", "semester_order IN (1, 2)");
            table.HasCheckConstraint("ck_semesters_status", "status IN ('PLANNED', 'ACTIVE', 'CLOSED')");
        });
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.AcademicYearId).HasColumnName("academic_year_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.Order).HasColumnName("semester_order").HasColumnType("tinyint unsigned").IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(x => x.StartDate).HasColumnName("start_date").HasColumnType("date");
        builder.Property(x => x.EndDate).HasColumnName("end_date").HasColumnType("date");
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).HasDefaultValue("PLANNED").IsRequired();
        builder.Property(x => x.Version).HasColumnName("version").HasDefaultValue(1u).IsConcurrencyToken();
        builder.HasIndex(x => new { x.AcademicYearId, x.Order })
            .IsUnique().HasDatabaseName("uq_semesters_year_order");
        builder.HasOne(x => x.AcademicYear).WithMany(x => x.Semesters)
            .HasForeignKey(x => x.AcademicYearId).HasConstraintName("fk_semesters_academic_year")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
