using Application.DTOs;
using Domain.Entities.QuestionBank;

namespace Application.Interfaces;

public interface IMatrixRepository
{
    Task<MatrixPage> ListAsync(MatrixListQuery query, CancellationToken cancellationToken);
    Task<ExamMatrix?> GetAsync(ulong id, CancellationToken cancellationToken);
    Task<bool> ExistsForTaskAsync(ulong taskId, CancellationToken cancellationToken);
    Task AddAsync(ExamMatrix matrix, CancellationToken cancellationToken);
    Task RemoveAsync(ExamMatrix matrix, CancellationToken cancellationToken);
    Task<bool> TryUpdateStatusAsync(
        ExamMatrix matrix,
        string expectedStatus,
        CancellationToken cancellationToken);
    Task<bool> LockWithStatusAsync(
        ulong matrixId,
        string expectedStatus,
        CancellationToken cancellationToken);
    Task SetTaskStatusAsync(
        ulong taskId,
        string status,
        ulong updatedByUserId,
        CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
