using Domain.Entities.Academic;
using Infrastructure.Context;
using Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Infrastructure.Repositories.Implement;

public sealed class AcademicYearRepository(ApplicationDbContext context) : IAcademicYearRepository
{
    public async Task<AcademicYearCreateOutcome> TryAddAsync(
        AcademicYear academicYear,
        CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var lockedProvinces = await context.Provinces
            .FromSqlInterpolated($"""
                SELECT *
                FROM provinces
                WHERE code = {academicYear.ProvinceCode} AND is_active = TRUE
                FOR UPDATE
                """)
            .ToListAsync(cancellationToken);
        var province = lockedProvinces.SingleOrDefault();

        if (province is null)
        {
            return AcademicYearCreateOutcome.ProvinceNotFound;
        }

        var hasConflict = await context.AcademicYears.AsNoTracking().AnyAsync(
            year => year.ProvinceCode == academicYear.ProvinceCode &&
                    (year.Name == academicYear.Name ||
                     (year.StartDate <= academicYear.EndDate &&
                      academicYear.StartDate <= year.EndDate)),
            cancellationToken);
        if (hasConflict)
        {
            return AcademicYearCreateOutcome.Conflict;
        }

        context.AcademicYears.Add(academicYear);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return AcademicYearCreateOutcome.Created;
        }
        catch (DbUpdateException exception)
            when (exception.InnerException is MySqlException { Number: 1062 })
        {
            return AcademicYearCreateOutcome.Conflict;
        }
    }

    public async Task<(IReadOnlyList<AcademicYear> Items, int TotalCount)> ListAsync(
        AcademicYearListFilter filter,
        CancellationToken cancellationToken)
    {
        var query = context.AcademicYears
            .AsNoTracking()
            .Where(year => year.ProvinceCode == filter.ProvinceCode);

        if (filter.Status is not null)
        {
            query = query.Where(year => year.Status == filter.Status);
        }

        if (filter.Search is not null)
        {
            query = query.Where(year => year.Name.Contains(filter.Search));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(year => year.StartDate)
            .ThenByDescending(year => year.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToArrayAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<AcademicYear?> GetByIdWithSemestersAsync(
        ulong id,
        CancellationToken cancellationToken) =>
        context.AcademicYears
            .Include(year => year.Semesters)
            .FirstOrDefaultAsync(year => year.Id == id, cancellationToken);

    public Task<bool> HasConflictExceptCurrentAsync(
        string provinceCode,
        ulong currentYearId,
        string name,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken) =>
        context.AcademicYears.AsNoTracking().AnyAsync(
            year => year.ProvinceCode == provinceCode &&
                    year.Id != currentYearId &&
                    (year.Name == name ||
                     (year.StartDate <= endDate && startDate <= year.EndDate)),
            cancellationToken);

    public Task<bool> HasActiveYearInProvinceAsync(
        string provinceCode,
        ulong exceptYearId,
        CancellationToken cancellationToken) =>
        context.AcademicYears.AsNoTracking().AnyAsync(
            year => year.ProvinceCode == provinceCode &&
                    year.Id != exceptYearId &&
                    year.Status == "ACTIVE",
            cancellationToken);

    public async Task<bool> UpdateAsync(
        AcademicYear academicYear,
        CancellationToken cancellationToken)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
        catch (DbUpdateException exception)
            when (exception.InnerException is MySqlException { Number: 1062 })
        {
            return false;
        }
    }

}
