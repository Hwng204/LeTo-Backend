using Domain.Entities.Organization;

namespace Domain.Entities.Identity;

public sealed class Student
{
    public ulong Id { get; set; }
    public ulong? UserId { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public ulong ClassId { get; set; }

    public User? User { get; set; }
    public SchoolClass SchoolClass { get; set; } = null!;
}
