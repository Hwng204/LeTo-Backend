using System.Linq.Expressions;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

// Changes are only tracked here; IUnitOfWork.CompleteAsync writes them to the database.
public class GenericRepository<T>(ApplicationDbContext db) : IGenericRepository<T> where T : class
{
    protected ApplicationDbContext Db { get; } = db;

    public virtual async Task<T?> GetByIdAsync(
        ulong id,
        CancellationToken cancellationToken = default)
    {
        return await Db.Set<T>().FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await Db.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await Db.Set<T>().AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task AddAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        await Db.Set<T>().AddAsync(entity, cancellationToken);
    }

    public virtual Task UpdateAsync(T entity)
    {
        Db.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity)
    {
        Db.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
}
