namespace Domain.Entities.QuestionBank;

public sealed class ExamVariantQuestion
{
    public ulong Id { get; set; }
    public ulong ExamVariantId { get; set; }
    public ulong ExamSetQuestionId { get; set; }
    public uint PositionNo { get; set; }
    public string? OptionOrderJson { get; set; }

    public ExamVariant ExamVariant { get; set; } = null!;
    public ExamSetQuestion ExamSetQuestion { get; set; } = null!;
}
