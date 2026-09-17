using Domain.Entities.Organization;

namespace Domain.Entities.Identity;

public sealed class Teacher
{
    public ulong Id { get; set; }
    public ulong UserId { get; set; }
    public string? Specialization { get; set; }
    public string? Position { get; set; }
    public bool? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public ulong? ClassId { get; set; }

    public User User { get; set; } = null!;
    public SchoolClass? SchoolClass { get; set; }
}
