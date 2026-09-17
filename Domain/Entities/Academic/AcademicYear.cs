namespace Domain.Entities.Academic;

public sealed class AcademicYear
{
    public ulong Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Status { get; set; } = "ACTIVE";

    public ICollection<Semester> Semesters { get; set; } = new List<Semester>();
}
