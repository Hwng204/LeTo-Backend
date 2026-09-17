using Domain.Entities.Academic;

namespace Domain.Entities.QuestionBank;

public sealed class ExamMatrix
{
    public ulong Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public ulong TaskId { get; set; }
    public ulong? SemesterId { get; set; }
    public ulong AcademicContextId { get; set; }

    public WorkTask Task { get; set; } = null!;
    public Semester? Semester { get; set; }
    public AcademicContext AcademicContext { get; set; } = null!;
    public ICollection<MatrixDetail> Details { get; set; } = new List<MatrixDetail>();
}
