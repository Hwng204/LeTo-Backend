using Domain.Entities.Identity;

namespace Domain.Entities.Notification;

public sealed class NotificationTarget
{
    public ulong Id { get; set; }
    public ulong ConfigId { get; set; }
    public ulong? RoleId { get; set; }
    public ulong? UserId { get; set; }
    public string Action { get; set; } = string.Empty;

    public NotificationConfig Config { get; set; } = null!;
    public Role? Role { get; set; }
    public User? User { get; set; }
}
