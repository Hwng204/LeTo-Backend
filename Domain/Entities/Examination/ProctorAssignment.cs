namespace Domain.Entities.Examination;

public sealed class ProctorAssignment
{
    public ulong Id { get; set; }
    public ulong SessionRoomId { get; set; }
    public ulong ExamProctorId { get; set; }
    public string ProctorRole { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }

    public SessionRoom SessionRoom { get; set; } = null!;
    public ExamProctor ExamProctor { get; set; } = null!;
}
