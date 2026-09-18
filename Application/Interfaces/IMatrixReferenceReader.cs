using Application.DTOs;


namespace Application.Interfaces;

public interface IMatrixReferenceReader
{
    Task EnsureValidAsync(
        ulong academicContextId,
        ulong? semesterId,
        IReadOnlyCollection<MatrixDetailRequest> details,
        CancellationToken cancellationToken,
        ulong? requiredBranchId = null);

    Task<MatrixExportInfo> GetExportInfoAsync(
        ulong academicContextId,
        ulong? semesterId,
        IReadOnlyCollection<ulong> lessonIds,
        CancellationToken cancellationToken);
}

