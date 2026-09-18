namespace Application.Interfaces;

public interface IMatrixTransaction
{
    Task<T> ExecuteAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken);
}

