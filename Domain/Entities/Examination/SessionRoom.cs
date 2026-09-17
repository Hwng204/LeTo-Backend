namespace Domain.Entities.Examination;

public sealed class SessionRoom
{
    public ulong Id { get; set; }
    public ulong ExamSessionId { get; set; }
    public ulong ExamRoomId { get; set; }
    public string Status { get; set; } = string.Empty;

    public ExamSession ExamSession { get; set; } = null!;
    public ExamRoom ExamRoom { get; set; } = null!;
    public ICollection<ExamRegistration> Registrations { get; set; } = new List<ExamRegistration>();
    public ICollection<ProctorAssignment> ProctorAssignments { get; set; } = new List<ProctorAssignment>();
}
