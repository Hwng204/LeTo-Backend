using Application.DTOs;
using Domain.Entities.QuestionBank;

namespace Application.Interfaces;

public interface IMatrixTaskRepository
{
    Task<MatrixTaskPage> ListAsync(
        MatrixTaskQuery query,
        ulong? assignedToUserId,
        CancellationToken cancellationToken);

    Task<WorkTask?> GetAsync(ulong taskId, CancellationToken cancellationToken);
    Task<ulong?> GetLinkedMatrixIdAsync(ulong taskId, CancellationToken cancellationToken);
    Task<ulong?> GetContextBranchIdAsync(ulong academicContextId, CancellationToken cancellationToken);
    Task AddAsync(WorkTask task, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
