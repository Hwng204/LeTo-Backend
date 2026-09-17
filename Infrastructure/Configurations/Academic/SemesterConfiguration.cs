using Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Academic;

public sealed class SemesterConfiguration : IEntityTypeConfiguration<Semester>
{
    public void Configure(EntityTypeBuilder<Semester> builder)
    {
        builder.ToTable("semesters", table => table.HasCheckConstraint("ck_semesters_dates", "end_date >= start_date"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.AcademicYearId).HasColumnName("academic_year_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(x => x.StartDate).HasColumnName("start_date").HasColumnType("date").IsRequired();
        builder.Property(x => x.EndDate).HasColumnName("end_date").HasColumnType("date").IsRequired();
        builder.HasIndex(x => new { x.AcademicYearId, x.Name })
            .IsUnique().HasDatabaseName("uq_semesters_year_name");
        builder.HasOne(x => x.AcademicYear).WithMany(x => x.Semesters)
            .HasForeignKey(x => x.AcademicYearId).HasConstraintName("fk_semesters_academic_year")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
