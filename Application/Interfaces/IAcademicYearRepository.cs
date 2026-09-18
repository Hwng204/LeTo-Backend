using Application.DTOs;
using Domain.Entities.Academic;

namespace Application.Interfaces;

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
        AcademicYearListQuery query,
        CancellationToken cancellationToken);

    Task<AcademicYear?> GetByIdWithSemestersAsync(
        ulong id,
        CancellationToken cancellationToken);

    Task<bool> HasConflictExceptCurrentAsync(
        string provinceCode,
        ulong currentYearId,
        string name,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken);

    Task<bool> HasActiveYearInProvinceAsync(
        string provinceCode,
        ulong exceptYearId,
        CancellationToken cancellationToken);

    Task<bool> UpdateAsync(
        AcademicYear academicYear,
        CancellationToken cancellationToken);

    /// <summary>Persists all pending changes tracked by this repository's unit of work.</summary>
    Task CommitAsync(CancellationToken cancellationToken);
}
