using Domain.Entities.QuestionBank;

namespace Domain.Entities.Examination;

public sealed class ExamAttempt
{
    public ulong Id { get; set; }
    public ulong ExamRegistrationId { get; set; }
    public ulong ExamVariantId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public decimal? TotalScore { get; set; }
    public string? Note { get; set; }

    public ExamRegistration ExamRegistration { get; set; } = null!;
    public ExamVariant ExamVariant { get; set; } = null!;
    public ICollection<ExamAttemptAnswer> Answers { get; set; } = new List<ExamAttemptAnswer>();
    public ICollection<TechnicalIncident> TechnicalIncidents { get; set; } = new List<TechnicalIncident>();
    public ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
