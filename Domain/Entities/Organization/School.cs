namespace Domain.Entities.Organization;

public sealed class School
{
    public ulong Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "ACTIVE";

    public ICollection<SchoolBranch> Branches { get; set; } = new List<SchoolBranch>();
}
