using Domain.Entities.Identity;

namespace Domain.Entities.Examination;

public sealed class ExamProctor
{
    public ulong Id { get; set; }
    public ulong ExamId { get; set; }
    public ulong TeacherId { get; set; }

    public Exam Exam { get; set; } = null!;
    public Teacher Teacher { get; set; } = null!;
    public ICollection<ProctorAssignment> Assignments { get; set; } = new List<ProctorAssignment>();
}
