using Domain.Entities.Identity;

namespace Domain.Entities.Examination;

public sealed class Violation
{
    public ulong Id { get; set; }
    public ulong ExamAttemptId { get; set; }
    public string ViolationType { get; set; } = string.Empty;
    public string Decision { get; set; } = string.Empty;
    public ulong? HandledByUserId { get; set; }
    public string? Description { get; set; }
    public DateTime OccurredAt { get; set; }

    public ExamAttempt ExamAttempt { get; set; } = null!;
    public User? HandledByUser { get; set; }
}
