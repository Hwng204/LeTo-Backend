namespace Domain.Entities.Identity;

public sealed class UserRole
{
    public ulong Id { get; set; }
    public ulong UserId { get; set; }
    public ulong RoleId { get; set; }

    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}
