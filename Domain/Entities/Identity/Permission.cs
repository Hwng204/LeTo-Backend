namespace Domain.Entities.Identity;

public sealed class Permission
{
    public ulong RoleId { get; set; }
    public ulong NavbarId { get; set; }
    public uint PermissionMask { get; set; }

    public Role Role { get; set; } = null!;
    public Navbar Navbar { get; set; } = null!;
}
