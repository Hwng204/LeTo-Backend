using Domain.Entities.Organization;

namespace Domain.Entities.Academic;

public sealed class AcademicYear
{
    public ulong Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Status { get; set; } = "DRAFT";
    public string? ProvinceCode { get; set; }
    public string? ActiveProvinceCode { get; private set; }
    public uint Version { get; set; } = 1;

    public Province? Province { get; set; }
    public ICollection<Semester> Semesters { get; set; } = new List<Semester>();
}
