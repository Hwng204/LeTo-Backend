using Domain.Entities.Identity;

namespace Domain.Entities.Examination;

public sealed class ExamRegistration
{
    public ulong Id { get; set; }
    public ulong ExamSubjectGradeLevelId { get; set; }
    public ulong StudentId { get; set; }
    public ulong? SessionRoomId { get; set; }
    public string Status { get; set; } = string.Empty;

    public ExamSubjectGradeLevel ExamSubjectGradeLevel { get; set; } = null!;
    public Student Student { get; set; } = null!;
    public SessionRoom? SessionRoom { get; set; }
    public ExamAttempt? Attempt { get; set; }
}
