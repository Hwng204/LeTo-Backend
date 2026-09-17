namespace Domain.Entities.Examination;

public sealed class ExamSession
{
    public ulong Id { get; set; }
    public ulong ExamSubjectGradeLevelId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public string? Status { get; set; }
    public string? AccessCode { get; set; }

    public ExamSubjectGradeLevel ExamSubjectGradeLevel { get; set; } = null!;
    public ICollection<SessionRoom> Rooms { get; set; } = new List<SessionRoom>();
}
