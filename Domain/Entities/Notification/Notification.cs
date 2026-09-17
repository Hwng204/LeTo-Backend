using Domain.Entities.Organization;

namespace Domain.Entities.Notification;

public sealed class Notification
{
    public ulong Id { get; set; }
    public ulong? ConfigId { get; set; }
    public ulong SchoolId { get; set; }
    public ulong? SchoolBranchId { get; set; }
    public DateTime? ScheduledFor { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ActionUrl { get; set; }
    public DateTime CreatedAt { get; set; }

    public NotificationConfig? Config { get; set; }
    public School School { get; set; } = null!;
    public SchoolBranch? SchoolBranch { get; set; }
    public ICollection<NotificationRecipient> Recipients { get; set; } = new List<NotificationRecipient>();
}
