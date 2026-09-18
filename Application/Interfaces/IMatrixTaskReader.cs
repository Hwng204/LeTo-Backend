using Domain.Entities.QuestionBank;

namespace Application.Interfaces;

public interface IMatrixTaskReader
{
    Task<WorkTask?> GetAsync(ulong taskId, CancellationToken cancellationToken);
}

