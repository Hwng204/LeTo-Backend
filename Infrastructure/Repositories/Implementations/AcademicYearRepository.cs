using Application.AcademicYears;
using Domain.Entities.Academic;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Infrastructure.Repositories.Implementations;

public sealed class AcademicYearRepository(ApplicationDbContext context) : IAcademicYearRepository
{
    public Task<bool> ProvinceExistsAsync(
        string provinceCode,
        CancellationToken cancellationToken) =>
        context.Provinces.AsNoTracking().AnyAsync(
            province => province.Code == provinceCode && province.IsActive,
            cancellationToken);

    public Task<bool> HasConflictAsync(
        string provinceCode,
        string name,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken) =>
        context.AcademicYears.AsNoTracking().AnyAsync(
            year => year.ProvinceCode == provinceCode &&
                    (year.Name == name ||
                     (year.StartDate <= endDate && startDate <= year.EndDate)),
            cancellationToken);

    public async Task<bool> TryAddAsync(
        AcademicYear academicYear,
        CancellationToken cancellationToken)
    {
        context.AcademicYears.Add(academicYear);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException exception)
            when (exception.InnerException is MySqlException { Number: 1062 })
        {
            return false;
        }
    }

    public async Task<(IReadOnlyList<AcademicYear> Items, int TotalCount)> ListAsync(
        string provinceCode,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = context.AcademicYears
            .AsNoTracking()
            .Where(year => year.ProvinceCode == provinceCode);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(year => year.StartDate)
            .ThenByDescending(year => year.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return (items, totalCount);
    }
}
