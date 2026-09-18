using Application.AcademicYears;
using Domain.Entities.Academic;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Infrastructure.Repositories.Implementations;

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
