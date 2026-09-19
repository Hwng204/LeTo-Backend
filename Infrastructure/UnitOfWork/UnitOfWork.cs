using Infrastructure.Context;
using Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitOfWork;

public class UnitOfWork(
    ApplicationDbContext db,
    IAcademicYearRepository academicYears,
    IProvinceRepository provinces) : IUnitOfWork
{
    public IAcademicYearRepository AcademicYears { get; } = academicYears;
    public IProvinceRepository Provinces { get; } = provinces;

    public Task<int> CompleteAsync(CancellationToken cancellationToken = default) =>
        db.SaveChangesAsync(cancellationToken);

    public async Task<T> ExecuteInTransactionAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await operation(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await RollbackAsync(transaction);
            throw;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    private static async Task RollbackAsync(IDbContextTransaction transaction)
    {
        try
        {
            await transaction.RollbackAsync(CancellationToken.None);
        }
        catch
        {
            // Preserve the original operation exception.
        }
    }
}
