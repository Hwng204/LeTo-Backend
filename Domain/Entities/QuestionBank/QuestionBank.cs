using Domain.Entities.Academic;
using Domain.Entities.Organization;

namespace Domain.Entities.QuestionBank;

public sealed class QuestionBank
{
    public ulong Id { get; set; }
    public ulong SchoolBranchId { get; set; }
    public ulong SubjectId { get; set; }
    public ulong GradeLevelId { get; set; }
    public string BankType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public SchoolBranch SchoolBranch { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
    public GradeLevel GradeLevel { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
