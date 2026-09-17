namespace Domain.Entities.Academic;

public sealed class GradeLevel
{
    public ulong Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "ACTIVE";
}
