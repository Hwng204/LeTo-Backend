using Application.Common;
using Application.Common.Security;
using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Application.Services.Interface;
using Domain.Entities.QuestionBank;

namespace Application.Services.Implement;

public sealed class MatrixApplicationService(
    IMatrixRepository repository,
    IMatrixTaskReader taskReader,
    IMatrixReferenceReader referenceReader,
    IMatrixTransaction transaction,
    IMatrixCurrentUser currentUser,
    IMatrixWorkbookExporter exporter) : IMatrixApplicationService
{
    public Task<MatrixPage> ListAsync(
        MatrixListQuery query,
        CancellationToken cancellationToken)
    {
        var actor = currentUser.Actor;
        var scopedQuery = actor.Role == MatrixActorRole.TeamLead
            ? query with { AssignedToUserId = actor.UserId }
            : query with { BranchId = BranchScope(actor) };

        return repository.ListAsync(scopedQuery, cancellationToken);
    }

    public async Task<MatrixResponse> CreateAsync(
        SaveMatrixRequest request,
        CancellationToken cancellationToken)
    {
        ValidateRequest(request);
        var actor = currentUser.Actor;

        return await transaction.ExecuteAsync(async ct =>
        {
            var matrix = await BuildNewMatrixAsync(request, actor, ct);
            await repository.AddAsync(matrix, ct);
            await repository.SaveChangesAsync(ct);
            return matrix.ToResponse(actor);
        }, cancellationToken);
    }

    public async Task<MatrixResponse> GetAsync(
        ulong matrixId,
        CancellationToken cancellationToken)
    {
        var actor = currentUser.Actor;
        var matrix = await GetRequiredAsync(matrixId, actor, cancellationToken);
        EnsureViewAccess(matrix, actor);
        return matrix.ToResponse(actor);
    }

    public async Task<MatrixResponse> UpdateAsync(
        ulong matrixId,
        SaveMatrixRequest request,
        CancellationToken cancellationToken)
    {
        ValidateRequest(request);
        var actor = currentUser.Actor;

        return await transaction.ExecuteAsync(async ct =>
        {
            var matrix = await GetRequiredAsync(matrixId, actor, ct);
            if (request.TaskId != matrix.TaskId)
            {
                throw new MatrixApplicationException(
                    "TaskImmutable",
                    "Không thể chuyển ma trận giữa dạng trực tiếp và dạng giao nhiệm vụ.");
            }

            var academicContextId = matrix.AcademicContextId;
            var semesterId = matrix.SemesterId;

            if (matrix.TaskId is not null)
            {
                var task = await taskReader.GetAsync(matrix.TaskId.Value, ct);
                if (task is null)
                {
                    throw new MatrixApplicationException(
                        "TaskNotFound",
                        "Không tìm thấy nhiệm vụ ma trận.");
                }

                EnsureTaskAccess(task, actor);
                academicContextId = RequireTaskContext(task);
                semesterId = task.SemesterId;
            }
            else
            {
                academicContextId = request.AcademicContextId;
                semesterId = request.SemesterId;
            }

            await referenceReader.EnsureValidAsync(
                academicContextId,
                semesterId,
                request.Details,
                ct,
                BranchScope(actor));

            await EnsureUnchangedAsync(matrix, ct);
            matrix.Name = request.Name.Trim();
            matrix.AcademicContextId = academicContextId;
            matrix.SemesterId = semesterId;
            ReplaceDetails(matrix, request.Details, actor);
            await repository.SaveChangesAsync(ct);
            return matrix.ToResponse(actor);
        }, cancellationToken);
    }

    public async Task DeleteDraftAsync(
        ulong matrixId,
        CancellationToken cancellationToken)
    {
        var actor = currentUser.Actor;

        await transaction.ExecuteAsync(async ct =>
        {
            var matrix = await GetRequiredAsync(matrixId, actor, ct);
            if (matrix.Status != MatrixStatusCodes.Draft)
            {
                throw new MatrixApplicationException(
                    "InvalidTransition",
                    "Chỉ được xóa ma trận ở trạng thái Nháp.");
            }

            if (!matrix.CanHardDelete(actor))
            {
                throw new MatrixApplicationException(
                    "Forbidden",
                    "Bạn không có quyền xóa ma trận Nháp này.");
            }

            await EnsureUnchangedAsync(matrix, ct);
            await repository.RemoveAsync(matrix, ct);
            await repository.SaveChangesAsync(ct);
            return true;
        }, cancellationToken);
    }

    public Task<MatrixResponse> SubmitAsync(
        ulong matrixId,
        CancellationToken cancellationToken)
    {
        return TransitionAsync(
            matrixId,
            (matrix, actor) => matrix.Submit(actor),
            MatrixTaskStatusCodes.Submitted,
            cancellationToken);
    }

    public Task<MatrixResponse> WithdrawAsync(
        ulong matrixId,
        CancellationToken cancellationToken)
    {
        return TransitionAsync(
            matrixId,
            (matrix, actor) => matrix.Withdraw(actor),
            MatrixTaskStatusCodes.Assigned,
            cancellationToken);
    }

    public Task<MatrixResponse> ApproveAsync(
        ulong matrixId,
        CancellationToken cancellationToken)
    {
        return TransitionAsync(
            matrixId,
            (matrix, actor) => matrix.Approve(actor),
            MatrixTaskStatusCodes.Completed,
            cancellationToken);
    }

    public Task<MatrixResponse> ConfirmDirectAsync(
        ulong matrixId,
        CancellationToken cancellationToken)
    {
        return TransitionAsync(
            matrixId,
            (matrix, actor) => matrix.ConfirmDirect(actor),
            null,
            cancellationToken);
    }

    public Task<MatrixResponse> ArchiveAsync(
        ulong matrixId,
        CancellationToken cancellationToken)
    {
        return TransitionAsync(
            matrixId,
            (matrix, actor) => matrix.Archive(actor),
            null,
            cancellationToken);
    }

    public async Task<MatrixResponse> CloneAsync(
        ulong matrixId,
        CancellationToken cancellationToken)
    {
        var actor = currentUser.Actor;

        return await transaction.ExecuteAsync(async ct =>
        {
            var matrix = await GetRequiredAsync(matrixId, actor, ct);
            var clone = matrix.CloneAsDraft(actor);
            await repository.AddAsync(clone, ct);
            await repository.SaveChangesAsync(ct);
            return clone.ToResponse(actor);
        }, cancellationToken);
    }

    private async Task<MatrixResponse> TransitionAsync(
        ulong matrixId,
        Action<ExamMatrix, MatrixActor> transition,
        string? taskStatusAfter,
        CancellationToken cancellationToken)
    {
        var actor = currentUser.Actor;

        return await transaction.ExecuteAsync(async ct =>
        {
            var matrix = await GetRequiredAsync(matrixId, actor, ct);
            var expectedStatus = matrix.Status;
            try
            {
                transition(matrix, actor);
            }
            catch (MatrixDomainException exception)
            {
                throw new MatrixApplicationException(exception.Code, exception.Message);
            }

            if (!await repository.TryUpdateStatusAsync(matrix, expectedStatus, ct))
            {
                throw new MatrixApplicationException(
                    "ConcurrencyConflict",
                    "Trạng thái ma trận đã bị thay đổi bởi thao tác khác. Vui lòng tải lại và thử lại.");
            }

            if (taskStatusAfter is not null && matrix.TaskId is not null)
            {
                await repository.SetTaskStatusAsync(
                    matrix.TaskId.Value,
                    taskStatusAfter,
                    actor.UserId,
                    ct);
            }

            return matrix.ToResponse(actor);
        }, cancellationToken);
    }

    public async Task<MatrixExportFile> ExportAsync(
        ulong matrixId,
        CancellationToken cancellationToken)
    {
        var matrix = await GetAsync(matrixId, cancellationToken);
        if (!matrix.AllowedActions.Contains("Export"))
        {
            throw new MatrixApplicationException(
                "InvalidTransition",
                "Chỉ xuất được ma trận đã duyệt hoặc đã lưu trữ.");
        }

        var info = await referenceReader.GetExportInfoAsync(
            matrix.AcademicContextId,
            matrix.SemesterId,
            matrix.Details.Select(detail => detail.LessonId).Distinct().ToArray(),
            cancellationToken);

        return new MatrixExportFile(
            $"{SafeFileName(matrix.Name)}.xlsx",
            exporter.Create(matrix, info));
    }

    private static string SafeFileName(string value)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = new string(value
            .Trim()
            .Select(character => invalid.Contains(character) ? '_' : character)
            .ToArray())
            .Trim('.', ' ');

        return string.IsNullOrWhiteSpace(sanitized) ? "matrix" : sanitized;
    }

    // A PHT is limited to their own branch; the Principal and Team Leads are not branch-limited here.
    private static ulong? BranchScope(MatrixActor actor)
    {
        if (actor.Role != MatrixActorRole.Pht || actor.IsPrincipal)
        {
            return null;
        }

        return actor.BranchId ?? throw new MatrixApplicationException(
            "Forbidden",
            "Tài khoản PHT chưa được gán chi nhánh.");
    }

    private async Task EnsureUnchangedAsync(ExamMatrix matrix, CancellationToken cancellationToken)
    {
        if (!await repository.LockWithStatusAsync(matrix.Id, matrix.Status, cancellationToken))
        {
            throw new MatrixApplicationException(
                "ConcurrencyConflict",
                "Ma trận đã bị thay đổi bởi thao tác khác. Vui lòng tải lại và thử lại.");
        }
    }

    private async Task<ExamMatrix> BuildNewMatrixAsync(
        SaveMatrixRequest request,
        MatrixActor actor,
        CancellationToken cancellationToken)
    {
        if (request.TaskId is null)
        {
            if (actor.Role != MatrixActorRole.Pht)
            {
                throw new MatrixApplicationException(
                    "Forbidden",
                    "Chỉ PHT mới được tạo ma trận trực tiếp.");
            }

            await referenceReader.EnsureValidAsync(
                request.AcademicContextId,
                request.SemesterId,
                request.Details,
                cancellationToken,
                BranchScope(actor));

            var directMatrix = new ExamMatrix
            {
                Name = request.Name.Trim(),
                Status = MatrixStatusCodes.Draft,
                TaskId = null,
                SemesterId = request.SemesterId,
                AcademicContextId = request.AcademicContextId
            };

            ReplaceDetails(directMatrix, request.Details, actor);
            return directMatrix;
        }

        var task = await taskReader.GetAsync(request.TaskId.Value, cancellationToken);
        if (task is null)
        {
            throw new MatrixApplicationException(
                "TaskNotFound",
                "Không tìm thấy nhiệm vụ ma trận.");
        }

        EnsureTaskAccess(task, actor);

        if (!string.Equals(task.TaskType, "MATRIX", StringComparison.OrdinalIgnoreCase))
        {
            throw new MatrixApplicationException(
                "InvalidTaskType",
                "Nhiệm vụ được chọn không phải nhiệm vụ ma trận.");
        }

        if (await repository.ExistsForTaskAsync(task.Id, cancellationToken))
        {
            throw new MatrixApplicationException(
                "TaskAlreadyHasMatrix",
                "Nhiệm vụ này đã có ma trận.");
        }

        var taskContextId = RequireTaskContext(task);
        await referenceReader.EnsureValidAsync(
            taskContextId,
            task.SemesterId,
            request.Details,
            cancellationToken,
            BranchScope(actor));

        var delegatedMatrix = new ExamMatrix
        {
            Name = request.Name.Trim(),
            Status = MatrixStatusCodes.Draft,
            TaskId = task.Id,
            Task = task,
            SemesterId = task.SemesterId,
            AcademicContextId = taskContextId
        };

        ReplaceDetails(delegatedMatrix, request.Details, actor);
        return delegatedMatrix;
    }

    private async Task<ExamMatrix> GetRequiredAsync(
        ulong matrixId,
        MatrixActor actor,
        CancellationToken cancellationToken)
    {
        var matrix = await repository.GetAsync(matrixId, cancellationToken);
        if (matrix is null)
        {
            throw new MatrixApplicationException(
                "NotFound",
                "Không tìm thấy ma trận.");
        }

        var branchScope = BranchScope(actor);
        if (branchScope is not null &&
            matrix.AcademicContext?.SchoolBranchId != branchScope)
        {
            throw new MatrixApplicationException(
                "Forbidden",
                "Ma trận thuộc chi nhánh khác.");
        }

        return matrix;
    }

    private static void EnsureViewAccess(ExamMatrix matrix, MatrixActor actor)
    {
        if (actor.Role == MatrixActorRole.Pht)
        {
            return;
        }

        if (actor.Role == MatrixActorRole.TeamLead &&
            matrix.Task is not null &&
            matrix.Task.AssignedToUserId == actor.UserId)
        {
            return;
        }

        throw new MatrixApplicationException(
            "Forbidden",
            "Bạn không có quyền xem ma trận này.");
    }

    private static void ReplaceDetails(
        ExamMatrix matrix,
        IReadOnlyCollection<MatrixDetailRequest> details,
        MatrixActor actor)
    {
        try
        {
            matrix.ReplaceDetails(details.ToValues(), actor);
        }
        catch (MatrixDomainException exception)
        {
            throw new MatrixApplicationException(exception.Code, exception.Message);
        }
    }

    private static ulong RequireTaskContext(WorkTask task)
    {
        if (task.AcademicContextId is null)
        {
            throw new MatrixApplicationException(
                "TaskScopeRequired",
                "Nhiệm vụ ma trận phải có ngữ cảnh học thuật.");
        }

        return task.AcademicContextId.Value;
    }

    private static void EnsureTaskAccess(WorkTask task, MatrixActor actor)
    {
        if (actor.Role == MatrixActorRole.TeamLead &&
            task.AssignedToUserId != actor.UserId)
        {
            throw new MatrixApplicationException(
                "Forbidden",
                "Bạn không phải Tổ trưởng được giao nhiệm vụ này.");
        }
    }

    private static void ValidateRequest(SaveMatrixRequest request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Name))
        {
            throw new MatrixApplicationException(
                "InvalidRequest",
                "Tên ma trận là bắt buộc.");
        }

        if (request.Details is null)
        {
            throw new MatrixApplicationException(
                "InvalidRequest",
                "Chi tiết ma trận là bắt buộc.");
        }

        if (request.Name.Trim().Length > 255 ||
            request.Details.Any(detail =>
                detail is null ||
                detail.CognitiveLevel?.Trim().Length > 50 ||
                detail.AllocatedScore > 999.99m))
        {
            throw new MatrixApplicationException(
                "InvalidRequest",
                "Tên ma trận (tối đa 255 ký tự), mức nhận thức (tối đa 50 ký tự) hoặc điểm (tối đa 999,99) vượt quá giới hạn cho phép.");
        }
    }
}
