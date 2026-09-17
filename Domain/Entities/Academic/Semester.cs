namespace Domain.Entities.Academic;

public sealed class Semester
{
    public ulong Id { get; set; }
    public ulong AcademicYearId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public AcademicYear AcademicYear { get; set; } = null!;
}
