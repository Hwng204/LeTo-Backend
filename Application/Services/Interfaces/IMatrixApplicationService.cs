using Application.DTOs;


namespace Application.Services.Interface;

public interface IMatrixApplicationService
{
    Task<MatrixPage> ListAsync(MatrixListQuery query, CancellationToken cancellationToken);
    Task<MatrixResponse> GetAsync(ulong matrixId, CancellationToken cancellationToken);
    Task<MatrixResponse> CreateAsync(SaveMatrixRequest request, CancellationToken cancellationToken);
    Task<MatrixResponse> UpdateAsync(ulong matrixId, SaveMatrixRequest request, CancellationToken cancellationToken);
    Task DeleteDraftAsync(ulong matrixId, CancellationToken cancellationToken);
    Task<MatrixResponse> SubmitAsync(ulong matrixId, CancellationToken cancellationToken);
    Task<MatrixResponse> RejectAsync(ulong matrixId, RejectMatrixRequest? request, CancellationToken cancellationToken);
    Task<MatrixResponse> ApproveAsync(ulong matrixId, CancellationToken cancellationToken);
    Task<MatrixResponse> ConfirmDirectAsync(ulong matrixId, CancellationToken cancellationToken);
    Task<MatrixResponse> ArchiveAsync(ulong matrixId, CancellationToken cancellationToken);
    Task<MatrixResponse> CloneAsync(ulong matrixId, CancellationToken cancellationToken);
    Task<MatrixExportFile> ExportAsync(ulong matrixId, CancellationToken cancellationToken);
}
