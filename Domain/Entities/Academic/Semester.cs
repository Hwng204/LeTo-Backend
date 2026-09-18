namespace Domain.Entities.Academic;

public sealed class Semester
{
    public ulong Id { get; set; }
    public ulong AcademicYearId { get; set; }
    public byte Order { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Status { get; set; } = "PLANNED";
    public uint Version { get; set; } = 1;

    public AcademicYear AcademicYear { get; set; } = null!;
}
