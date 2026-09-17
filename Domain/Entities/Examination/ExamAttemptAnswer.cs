using Domain.Entities.QuestionBank;

namespace Domain.Entities.Examination;

public sealed class ExamAttemptAnswer
{
    public ulong Id { get; set; }
    public ulong ExamAttemptId { get; set; }
    public ulong ExamVariantQuestionId { get; set; }
    public ulong? SelectedOptionId { get; set; }
    public decimal? ScoreAwarded { get; set; }
    public bool? IsCorrect { get; set; }

    public ExamAttempt ExamAttempt { get; set; } = null!;
    public ExamVariantQuestion ExamVariantQuestion { get; set; } = null!;
    public QuestionOption? SelectedOption { get; set; }
}
