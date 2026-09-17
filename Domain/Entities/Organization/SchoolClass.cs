using Domain.Entities.Academic;

namespace Domain.Entities.Organization;

public sealed class SchoolClass
{
    public ulong Id { get; set; }
    public ulong SchoolBranchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "ACTIVE";
    public ulong AcademicYearId { get; set; }
    public ulong GradeLevelId { get; set; }

    public SchoolBranch SchoolBranch { get; set; } = null!;
    public AcademicYear AcademicYear { get; set; } = null!;
    public GradeLevel GradeLevel { get; set; } = null!;
}
