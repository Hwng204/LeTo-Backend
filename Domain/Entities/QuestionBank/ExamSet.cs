using Domain.Entities.Identity;

namespace Domain.Entities.QuestionBank;

public sealed class ExamSet
{
    public ulong Id { get; set; }
    public ulong? ExamMatrixId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public ulong? TaskId { get; set; }
    public ulong CreatedByUserId { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public ulong? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public ulong? SourceExamSetId { get; set; }

    public ExamMatrix? ExamMatrix { get; set; }
    public WorkTask? Task { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public User? ApprovedByUser { get; set; }
    public ExamSet? SourceExamSet { get; set; }
    public ICollection<ExamSet> DerivedExamSets { get; set; } = new List<ExamSet>();
    public ICollection<ExamSetQuestion> Questions { get; set; } = new List<ExamSetQuestion>();
    public ICollection<ExamVariant> Variants { get; set; } = new List<ExamVariant>();
}
