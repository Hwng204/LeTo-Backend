namespace Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public async Task<int> CompleteAsync()
    {
        await Task.CompletedTask;
        return 1;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
