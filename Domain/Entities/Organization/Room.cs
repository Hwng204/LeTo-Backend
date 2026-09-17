namespace Domain.Entities.Organization;

public sealed class Room
{
    public ulong Id { get; set; }
    public ulong SchoolBranchId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public SchoolBranch SchoolBranch { get; set; } = null!;
}
