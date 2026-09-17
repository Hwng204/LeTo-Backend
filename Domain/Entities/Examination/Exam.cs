using Domain.Entities.Academic;
using Domain.Entities.Organization;

namespace Domain.Entities.Examination;

public sealed class Exam
{
    public ulong Id { get; set; }
    public ulong SemesterId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public ulong SchoolBranchId { get; set; }

    public Semester Semester { get; set; } = null!;
    public SchoolBranch SchoolBranch { get; set; } = null!;
    public ICollection<ExamSubject> Subjects { get; set; } = new List<ExamSubject>();
    public ICollection<ExamRoom> Rooms { get; set; } = new List<ExamRoom>();
    public ICollection<ExamProctor> Proctors { get; set; } = new List<ExamProctor>();
}
