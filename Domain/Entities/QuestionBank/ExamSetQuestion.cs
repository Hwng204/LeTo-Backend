namespace Domain.Entities.QuestionBank;

public sealed class ExamSetQuestion
{
    public ulong Id { get; set; }
    public ulong ExamSetId { get; set; }
    public ulong QuestionId { get; set; }
    public ulong? MatrixDetailId { get; set; }

    public ExamSet ExamSet { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public MatrixDetail? MatrixDetail { get; set; }
    public ICollection<ExamVariantQuestion> VariantQuestions { get; set; } = new List<ExamVariantQuestion>();
}
