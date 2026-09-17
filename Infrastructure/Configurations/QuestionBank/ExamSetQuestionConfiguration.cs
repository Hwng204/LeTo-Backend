using Domain.Entities.QuestionBank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.QuestionBank;

public sealed class ExamSetQuestionConfiguration : IEntityTypeConfiguration<ExamSetQuestion>
{
    public void Configure(EntityTypeBuilder<ExamSetQuestion> builder)
    {
        builder.ToTable("exam_set_questions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamSetId).HasColumnName("exam_set_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.QuestionId).HasColumnName("question_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.MatrixDetailId).HasColumnName("matrix_detail_id").HasColumnType("bigint unsigned");
        builder.HasIndex(x => new { x.ExamSetId, x.QuestionId })
            .IsUnique().HasDatabaseName("uq_exam_set_questions_question");
        builder.HasIndex(x => x.MatrixDetailId).HasDatabaseName("idx_exam_set_questions_matrix_detail");
        builder.HasOne(x => x.ExamSet).WithMany(x => x.Questions)
            .HasForeignKey(x => x.ExamSetId).HasConstraintName("fk_exam_set_questions_set")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Question).WithMany()
            .HasForeignKey(x => x.QuestionId).HasConstraintName("fk_exam_set_questions_question")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.MatrixDetail).WithMany()
            .HasForeignKey(x => x.MatrixDetailId).HasConstraintName("fk_exam_set_questions_matrix_detail")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
