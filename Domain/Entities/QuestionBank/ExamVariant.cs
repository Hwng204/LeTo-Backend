namespace Domain.Entities.QuestionBank;

public sealed class ExamVariant
{
    public ulong Id { get; set; }
    public ulong ExamSetId { get; set; }
    public string VariantCode { get; set; } = string.Empty;

    public ExamSet ExamSet { get; set; } = null!;
    public ICollection<ExamVariantQuestion> Questions { get; set; } = new List<ExamVariantQuestion>();
}
