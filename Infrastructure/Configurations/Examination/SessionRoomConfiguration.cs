using Domain.Entities.Examination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Examination;

public sealed class SessionRoomConfiguration : IEntityTypeConfiguration<SessionRoom>
{
    public void Configure(EntityTypeBuilder<SessionRoom> builder)
    {
        builder.ToTable("session_rooms");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamSessionId).HasColumnName("exam_session_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.ExamRoomId).HasColumnName("exam_room_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).IsRequired();
        builder.HasIndex(x => new { x.ExamSessionId, x.ExamRoomId })
            .IsUnique().HasDatabaseName("uq_session_rooms_session_room");
        builder.HasIndex(x => x.ExamRoomId).HasDatabaseName("idx_session_rooms_exam_room");
        builder.HasOne(x => x.ExamSession).WithMany(x => x.Rooms)
            .HasForeignKey(x => x.ExamSessionId).HasConstraintName("fk_session_rooms_session")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ExamRoom).WithMany(x => x.Sessions)
            .HasForeignKey(x => x.ExamRoomId).HasConstraintName("fk_session_rooms_exam_room")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
