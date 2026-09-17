using Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Academic;

public sealed class GradeLevelConfiguration : IEntityTypeConfiguration<GradeLevel>
{
    public void Configure(EntityTypeBuilder<GradeLevel> builder)
    {
        builder.ToTable("grade_levels");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).HasDefaultValue("ACTIVE").IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("uq_grade_levels_name");
    }
}
