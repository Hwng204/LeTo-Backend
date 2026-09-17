using Domain.Entities.Examination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Examination;

public sealed class ProctorAssignmentConfiguration : IEntityTypeConfiguration<ProctorAssignment>
{
    public void Configure(EntityTypeBuilder<ProctorAssignment> builder)
    {
        builder.ToTable("proctor_assignments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.SessionRoomId).HasColumnName("session_room_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.ExamProctorId).HasColumnName("exam_proctor_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.ProctorRole).HasColumnName("proctor_role").HasMaxLength(50).IsRequired();
        builder.Property(x => x.AssignedAt).HasColumnName("assigned_at").HasColumnType("datetime(6)")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)").IsRequired();
        builder.HasIndex(x => new { x.SessionRoomId, x.ExamProctorId })
            .IsUnique().HasDatabaseName("uq_proctor_assignments_room_proctor");
        builder.HasIndex(x => x.ExamProctorId).HasDatabaseName("idx_proctor_assignments_proctor");
        builder.HasOne(x => x.SessionRoom).WithMany(x => x.ProctorAssignments)
            .HasForeignKey(x => x.SessionRoomId).HasConstraintName("fk_proctor_assignments_session_room")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ExamProctor).WithMany(x => x.Assignments)
            .HasForeignKey(x => x.ExamProctorId).HasConstraintName("fk_proctor_assignments_exam_proctor")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
