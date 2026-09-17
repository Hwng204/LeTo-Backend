using Domain.Entities.Examination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Examination;

public sealed class ExamRegistrationConfiguration : IEntityTypeConfiguration<ExamRegistration>
{
    public void Configure(EntityTypeBuilder<ExamRegistration> builder)
    {
        builder.ToTable("exam_registrations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamSubjectGradeLevelId).HasColumnName("exam_subject_grade_level_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.StudentId).HasColumnName("student_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.SessionRoomId).HasColumnName("session_room_id").HasColumnType("bigint unsigned");
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).IsRequired();
        builder.HasIndex(x => new { x.ExamSubjectGradeLevelId, x.StudentId })
            .IsUnique().HasDatabaseName("uq_exam_registrations_scope_student");
        builder.HasIndex(x => x.StudentId).HasDatabaseName("idx_exam_registrations_student");
        builder.HasIndex(x => x.SessionRoomId).HasDatabaseName("idx_exam_registrations_session_room");
        builder.HasOne(x => x.ExamSubjectGradeLevel).WithMany(x => x.Registrations)
            .HasForeignKey(x => x.ExamSubjectGradeLevelId).HasConstraintName("fk_exam_registrations_subject_grade")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Student).WithMany()
            .HasForeignKey(x => x.StudentId).HasConstraintName("fk_exam_registrations_student")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.SessionRoom).WithMany(x => x.Registrations)
            .HasForeignKey(x => x.SessionRoomId).HasConstraintName("fk_exam_registrations_session_room")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
