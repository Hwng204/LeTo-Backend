namespace Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> CompleteAsync();
}
