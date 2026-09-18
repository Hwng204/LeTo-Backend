using Domain.Entities.Academic;

namespace Application.AcademicYears;

public enum AcademicYearCreateOutcome
{
    Created,
    ProvinceNotFound,
    Conflict
}

public interface IAcademicYearRepository
{
    Task<AcademicYearCreateOutcome> TryAddAsync(
        AcademicYear academicYear,
        CancellationToken cancellationToken);

    Task<(IReadOnlyList<AcademicYear> Items, int TotalCount)> ListAsync(
        string provinceCode,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
