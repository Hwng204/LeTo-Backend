using Domain.Entities.Academic;
using Domain.Entities.Identity;

namespace Domain.Entities.Examination;

public sealed class ExamSubject
{
    public ulong Id { get; set; }
    public ulong ExamId { get; set; }
    public ulong SubjectId { get; set; }
    public uint DurationMinutes { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? ResultPublishedAt { get; set; }
    public ulong? ResultPublishedByUserId { get; set; }

    public Exam Exam { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
    public User? ResultPublishedByUser { get; set; }
    public ICollection<ExamSubjectGradeLevel> GradeLevels { get; set; } = new List<ExamSubjectGradeLevel>();
}
