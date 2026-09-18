using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities.QuestionBank;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Infrastructure.Repositories.Implement;

public sealed class ExamMatrixRepository(ApplicationDbContext db) : IMatrixRepository
{
    private const int MaxPageSize = 100;

    public async Task<MatrixPage> ListAsync(
        MatrixListQuery query,
        CancellationToken cancellationToken)
    {
        ValidatePage(query);

        var matrices = db.ExamMatrices.AsNoTracking();

        if (string.IsNullOrWhiteSpace(query.Status))
        {
            matrices = matrices.Where(matrix => matrix.Status != MatrixStatusCodes.Archived);
        }
        else
        {
            var status = query.Status.Trim().ToUpperInvariant();
            if (!MatrixStatusCodes.IsKnown(status))
            {
                throw new ArgumentException("Trạng thái ma trận không hợp lệ.", nameof(query));
            }

            matrices = matrices.Where(matrix => matrix.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var keyword = query.Keyword.Trim();
            matrices = matrices.Where(matrix =>
                EF.Functions.Like(matrix.Name, $"%{keyword}%"));
        }

        if (query.AcademicContextId is not null)
        {
            matrices = matrices.Where(matrix =>
                matrix.AcademicContextId == query.AcademicContextId.Value);
        }

        if (query.SemesterId is not null)
        {
            matrices = matrices.Where(matrix =>
                matrix.SemesterId == query.SemesterId.Value);
        }

        if (query.BranchId is not null)
        {
            matrices = matrices.Where(matrix =>
                matrix.AcademicContext.SchoolBranchId == query.BranchId.Value);
        }

        if (query.AssignedToUserId is not null)
        {
            matrices = matrices.Where(matrix =>
                matrix.Task != null &&
                matrix.Task.AssignedToUserId == query.AssignedToUserId.Value);
        }

        var totalCount = await matrices.CountAsync(cancellationToken);
        var rows = await matrices
            .OrderByDescending(matrix => matrix.Id)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(matrix => new
            {
                matrix.Id,
                matrix.Name,
                matrix.Status,
                matrix.TaskId,
                matrix.AcademicContextId,
                matrix.SemesterId,
                TotalQuestions = matrix.Details
                    .Select(detail => (long?)detail.QuestionCount)
                    .Sum() ?? 0,
                TotalScore = matrix.Details
                    .Select(detail => (decimal?)detail.AllocatedScore)
                    .Sum() ?? 0m
            })
            .ToListAsync(cancellationToken);

        var items = rows.Select(row => new MatrixListItem(
            row.Id,
            row.Name,
            row.Status,
            row.TaskId,
            row.AcademicContextId,
            row.SemesterId,
            checked((uint)row.TotalQuestions),
            row.TotalScore)).ToArray();

        return new MatrixPage(items, query.Page, query.PageSize, totalCount);
    }

    public async Task<ExamMatrix?> GetAsync(
        ulong id,
        CancellationToken cancellationToken)
    {
        var matrix = await db.ExamMatrices
            .Include(item => item.Task)
            .Include(item => item.AcademicContext)
            .Include(item => item.Details)
                .ThenInclude(detail => detail.Lesson)
                    .ThenInclude(lesson => lesson.Chapter)
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (matrix is not null)
        {
            matrix.Details = matrix.Details
                .OrderBy(detail => detail.Lesson.Chapter.SortOrder)
                .ThenBy(detail => detail.Lesson.SortOrder)
                .ThenBy(detail => detail.CognitiveLevel)
                .ThenBy(detail => detail.QuestionType)
                .ToList();
        }

        return matrix;
    }

    public Task<bool> ExistsForTaskAsync(
        ulong taskId,
        CancellationToken cancellationToken)
    {
        return db.ExamMatrices.AnyAsync(
            matrix => matrix.TaskId == taskId,
            cancellationToken);
    }

    public async Task AddAsync(
        ExamMatrix matrix,
        CancellationToken cancellationToken)
    {
        if (matrix.Task is not null &&
            db.Entry(matrix.Task).State == EntityState.Detached)
        {
            db.Attach(matrix.Task);
        }

        await db.ExamMatrices.AddAsync(matrix, cancellationToken);
    }

    public Task RemoveAsync(
        ExamMatrix matrix,
        CancellationToken cancellationToken)
    {
        db.ExamMatrices.Remove(matrix);
        return Task.CompletedTask;
    }

    public async Task<bool> TryUpdateStatusAsync(
        ExamMatrix matrix,
        string expectedStatus,
        CancellationToken cancellationToken)
    {
        var affected = await db.ExamMatrices
            .Where(item => item.Id == matrix.Id && item.Status == expectedStatus)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(item => item.Status, matrix.Status),
                cancellationToken);

        if (affected != 1)
        {
            return false;
        }

        var status = db.Entry(matrix).Property(item => item.Status);
        status.OriginalValue = matrix.Status;
        status.IsModified = false;
        return true;
    }

    // Row lock (SELECT ... FOR UPDATE) that also proves the status is still the one the caller read.
    public async Task<bool> LockWithStatusAsync(
        ulong matrixId,
        string expectedStatus,
        CancellationToken cancellationToken)
    {
        var rows = await db.Database
            .SqlQuery<ulong>($"SELECT id AS Value FROM exam_matrices WHERE id = {matrixId} AND status = {expectedStatus} FOR UPDATE")
            .ToListAsync(cancellationToken);
        return rows.Count == 1;
    }

    public async Task SetTaskStatusAsync(
        ulong taskId,
        string status,
        ulong updatedByUserId,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        await db.WorkTasks
            .Where(task => task.Id == taskId)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(task => task.Status, status)
                    .SetProperty(task => task.UpdatedAt, now)
                    .SetProperty(task => task.UpdatedByUserId, updatedByUserId),
                cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (IsDuplicateMatrixConstraint(exception))
        {
            throw new MatrixApplicationException(
                GetDuplicateCode(exception),
                "Ma trận xung đột với dữ liệu đã có (nhiệm vụ đã có ma trận hoặc dòng chi tiết bị trùng).");
        }
    }

    private static void ValidatePage(MatrixListQuery query)
    {
        if (query.Page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(query.Page));
        }

        if (query.PageSize is < 1 or > MaxPageSize)
        {
            throw new ArgumentOutOfRangeException(nameof(query.PageSize));
        }
    }

    private static bool IsDuplicateMatrixConstraint(DbUpdateException exception)
    {
        var baseException = exception.GetBaseException();
        return baseException is MySqlException mysqlException &&
            mysqlException.Number == 1062;
    }

    private static string GetDuplicateCode(DbUpdateException exception)
    {
        var message = exception.GetBaseException().Message;
        return message.Contains("uq_exam_matrices_task", StringComparison.OrdinalIgnoreCase)
            ? "TaskAlreadyHasMatrix"
            : message.Contains("uq_matrix_details_cell", StringComparison.OrdinalIgnoreCase)
                ? "DuplicateDetail"
                : "PersistenceConflict";
    }
}
