namespace Domain.Entities.Academic;

public sealed class Subject
{
    public ulong Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "ACTIVE";
}
