using Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Organization;

public sealed class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("rooms");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.SchoolBranchId).HasColumnName("school_branch_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(x => x.RoomType).HasColumnName("room_type").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).IsRequired();
        builder.HasIndex(x => new { x.SchoolBranchId, x.Code })
            .IsUnique().HasDatabaseName("uq_rooms_branch_code");
        builder.HasOne(x => x.SchoolBranch).WithMany(x => x.Rooms)
            .HasForeignKey(x => x.SchoolBranchId).HasConstraintName("fk_rooms_branch")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
