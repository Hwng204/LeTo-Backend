using Domain.Entities.Identity;

namespace Domain.Entities.Examination;

public sealed class TechnicalIncident
{
    public ulong Id { get; set; }
    public ulong ExamAttemptId { get; set; }
    public string IncidentType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public ulong ReportedByUserId { get; set; }
    public ulong? HandledByUserId { get; set; }

    public ExamAttempt ExamAttempt { get; set; } = null!;
    public User ReportedByUser { get; set; } = null!;
    public User? HandledByUser { get; set; }
}
