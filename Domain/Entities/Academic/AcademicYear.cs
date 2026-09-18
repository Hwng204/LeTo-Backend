using Domain.Entities.Organization;

namespace Domain.Entities.Academic;

public sealed class AcademicYear
{
    public ulong Id { get; set; }
    public string? Code { get; private set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Status { get; set; } = "DRAFT";
    public string? ProvinceCode { get; set; }
    public string? ActiveProvinceCode { get; private set; }
    public uint Version { get; set; } = 1;

    public Province? Province { get; set; }
    public ICollection<Semester> Semesters { get; set; } = new List<Semester>();

    public void AssignCode(string code)
    {
        if (!string.IsNullOrEmpty(Code))
        {
            throw new InvalidOperationException("Academic year code is immutable once assigned.");
        }

        if (string.IsNullOrWhiteSpace(code) || code.Length > 64)
        {
            throw new ArgumentException("Academic year code must contain 1 to 64 characters.", nameof(code));
        }

        Code = code;
    }
}
