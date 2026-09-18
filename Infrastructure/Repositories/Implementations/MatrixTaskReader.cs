using Application.Interfaces;
using Domain.Entities.QuestionBank;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implement;

public sealed class MatrixTaskReader(ApplicationDbContext db) : IMatrixTaskReader
{
    public Task<WorkTask?> GetAsync(
        ulong taskId,
        CancellationToken cancellationToken)
    {
        return db.WorkTasks
            .AsNoTracking()
            .SingleOrDefaultAsync(task => task.Id == taskId, cancellationToken);
    }
}
