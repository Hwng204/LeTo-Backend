using Application.Common.Security;
using Application.DTOs;
using Domain.Entities.QuestionBank;

namespace Application.Interfaces;

public interface IMatrixTaskReferenceReader
{
    Task EnsureAssignmentValidAsync(
        MatrixActor actor,
        ulong assignedToUserId,
        ulong academicContextId,
        ulong? semesterId,
        CancellationToken cancellationToken);

    Task<MatrixReferenceData> GetAsync(
        MatrixActor actor,
        ulong? academicContextId,
        CancellationToken cancellationToken);
}
