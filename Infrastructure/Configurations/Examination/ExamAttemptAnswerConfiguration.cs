using Domain.Entities.Examination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Examination;

public sealed class ExamAttemptAnswerConfiguration : IEntityTypeConfiguration<ExamAttemptAnswer>
{
    public void Configure(EntityTypeBuilder<ExamAttemptAnswer> builder)
    {
        builder.ToTable("exam_attempt_answers", table => table.HasCheckConstraint("ck_exam_attempt_answers_score", "score_awarded IS NULL OR score_awarded >= 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamAttemptId).HasColumnName("exam_attempt_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.ExamVariantQuestionId).HasColumnName("exam_variant_question_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.SelectedOptionId).HasColumnName("selected_option_id").HasColumnType("bigint unsigned");
        builder.Property(x => x.ScoreAwarded).HasColumnName("score_awarded").HasPrecision(5, 2);
        builder.Property(x => x.IsCorrect).HasColumnName("is_correct").HasColumnType("tinyint(1)");
        builder.HasIndex(x => new { x.ExamAttemptId, x.ExamVariantQuestionId })
            .IsUnique().HasDatabaseName("uq_exam_attempt_answers_question");
        builder.HasIndex(x => x.SelectedOptionId).HasDatabaseName("idx_exam_attempt_answers_selected_option");
        builder.HasOne(x => x.ExamAttempt).WithMany(x => x.Answers)
            .HasForeignKey(x => x.ExamAttemptId).HasConstraintName("fk_exam_attempt_answers_attempt")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ExamVariantQuestion).WithMany()
            .HasForeignKey(x => x.ExamVariantQuestionId).HasConstraintName("fk_exam_attempt_answers_variant_question")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.SelectedOption).WithMany()
            .HasForeignKey(x => x.SelectedOptionId).HasConstraintName("fk_exam_attempt_answers_option")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
