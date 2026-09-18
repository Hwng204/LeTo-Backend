using Domain.Entities.Academic;

namespace Application.AcademicYears;

public interface IAcademicYearRepository
{
    Task<bool> ProvinceExistsAsync(string provinceCode, CancellationToken cancellationToken);

    Task<bool> HasConflictAsync(
        string provinceCode,
        string name,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken);

    Task<bool> TryAddAsync(AcademicYear academicYear, CancellationToken cancellationToken);

    Task<(IReadOnlyList<AcademicYear> Items, int TotalCount)> ListAsync(
        string provinceCode,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
