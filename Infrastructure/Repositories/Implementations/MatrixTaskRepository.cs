using Application.DTOs;
using Application.Interfaces;
using Domain.Entities.QuestionBank;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implement;

public sealed class MatrixTaskRepository(ApplicationDbContext db) : IMatrixTaskRepository
{
    private const int MaxPageSize = 100;

    public async Task<MatrixTaskPage> ListAsync(
        MatrixTaskQuery query,
        ulong? assignedToUserId,
        CancellationToken cancellationToken)
    {
        ValidatePage(query);

        var tasks = db.WorkTasks
            .AsNoTracking()
            .Where(task => task.TaskType == "MATRIX");

        if (assignedToUserId is not null)
        {
            tasks = tasks.Where(task => task.AssignedToUserId == assignedToUserId.Value);
        }

        if (query.BranchId is not null)
        {
            var branchId = query.BranchId.Value;
            tasks = tasks.Where(task => db.AcademicContexts.Any(context =>
                context.Id == task.AcademicContextId &&
                context.SchoolBranchId == branchId));
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            tasks = tasks.Where(task => task.Status == query.Status.Trim().ToUpperInvariant());
        }

        if (query.AssignedToUserId is not null && assignedToUserId is null)
        {
            tasks = tasks.Where(task => task.AssignedToUserId == query.AssignedToUserId.Value);
        }

        if (query.DueBefore is not null)
        {
            tasks = tasks.Where(task => task.DueAt != null && task.DueAt <= query.DueBefore);
        }

        var totalCount = await tasks.CountAsync(cancellationToken);
        var rows = await tasks
            .OrderByDescending(task => task.Id)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(task => new
            {
                task.Id,
                task.CreatedByUserId,
                task.AssignedToUserId,
                task.DueAt,
                task.Status,
                task.TaskType,
                task.Description,
                task.AcademicContextId,
                task.SemesterId,
                MatrixId = db.ExamMatrices
                    .Where(matrix => matrix.TaskId == task.Id)
                    .Select(matrix => (ulong?)matrix.Id)
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        var items = rows
            .Select(task => new MatrixTaskListItem(
                task.Id,
                task.CreatedByUserId,
                task.AssignedToUserId,
                task.DueAt,
                task.Status,
                task.TaskType,
                task.Description,
                task.AcademicContextId,
                task.SemesterId,
                task.MatrixId))
            .ToArray();

        return new MatrixTaskPage(items, query.Page, query.PageSize, totalCount);
    }

    public Task<WorkTask?> GetAsync(
        ulong taskId,
        CancellationToken cancellationToken)
    {
        return db.WorkTasks
            .AsNoTracking()
            .SingleOrDefaultAsync(
                task => task.Id == taskId && task.TaskType == "MATRIX",
                cancellationToken);
    }

    public Task<ulong?> GetLinkedMatrixIdAsync(
        ulong taskId,
        CancellationToken cancellationToken)
    {
        return db.ExamMatrices
            .AsNoTracking()
            .Where(matrix => matrix.TaskId == taskId)
            .Select(matrix => (ulong?)matrix.Id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<ulong?> GetContextBranchIdAsync(
        ulong academicContextId,
        CancellationToken cancellationToken)
    {
        return db.AcademicContexts
            .AsNoTracking()
            .Where(context => context.Id == academicContextId)
            .Select(context => (ulong?)context.SchoolBranchId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(WorkTask task, CancellationToken cancellationToken)
    {
        await db.WorkTasks.AddAsync(task, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return db.SaveChangesAsync(cancellationToken);
    }

    private static void ValidatePage(MatrixTaskQuery query)
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
}
