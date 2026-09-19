using Infrastructure.Repositories.Interface;

namespace Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IAcademicYearRepository AcademicYears { get; }
    IProvinceRepository Provinces { get; }

    Task<int> CompleteAsync(CancellationToken cancellationToken = default);

    Task<T> ExecuteInTransactionAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default);
}
