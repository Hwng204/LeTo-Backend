using Application.DTOs;
using Domain.Entities.QuestionBank;
using Infrastructure.Exports;
using Infrastructure.Models;

namespace Application.Mappings;

public static class MatrixMappingExtensions
{
    public static MatrixListFilter ToFilter(this MatrixListQuery query)
    {
        return new MatrixListFilter(
            query.Page,
            query.PageSize,
            query.Keyword,
            query.AcademicContextId,
            query.SemesterId,
            query.Status,
            query.AssignedToUserId,
            query.BranchId);
    }

    public static MatrixPage ToDto(this PagedResult<MatrixListRow> page)
    {
        var items = page.Items
            .Select(row => new MatrixListItem(
                row.Id,
                row.Name,
                row.Status,
                row.TaskId,
                row.AcademicContextId,
                row.SemesterId,
                row.TotalQuestions,
                row.TotalScore))
            .ToArray();

        return new MatrixPage(items, page.Page, page.PageSize, page.TotalCount);
    }

    public static MatrixWorkbookModel ToWorkbookModel(
        this MatrixResponse matrix,
        MatrixExportInfo info)
    {
        var rows = matrix.Details
            .Select(detail => new MatrixWorkbookRow(
                info.LessonTitles.TryGetValue(detail.LessonId, out var title)
                    ? title
                    : detail.LessonId.ToString(),
                detail.CognitiveLevel,
                detail.QuestionType,
                detail.QuestionCount,
                detail.AllocatedScore))
            .ToArray();

        return new MatrixWorkbookModel(
            matrix.Name,
            matrix.Status,
            matrix.TaskId,
            info.ContextLabel,
            info.SemesterName ?? matrix.SemesterId?.ToString() ?? string.Empty,
            matrix.TotalQuestions,
            matrix.TotalScore,
            rows);
    }

    public static IReadOnlyList<MatrixDetailValue> ToValues(
        this IReadOnlyCollection<MatrixDetailRequest> details)
    {
        return details
            .Select(detail => new MatrixDetailValue(
                detail.LessonId,
                detail.CognitiveLevel,
                MatrixQuestionTypes.MultipleChoice,
                detail.QuestionCount,
                detail.AllocatedScore))
            .ToArray();
    }

    public static MatrixResponse ToResponse(this ExamMatrix matrix, MatrixActor actor)
    {
        var details = matrix.Details
            .Select(detail => new MatrixDetailResponse(
                detail.Id,
                detail.LessonId,
                detail.CognitiveLevel,
                detail.QuestionType,
                detail.QuestionCount,
                detail.AllocatedScore))
            .ToArray();

        return new MatrixResponse(
            matrix.Id,
            matrix.Name,
            matrix.Status,
            matrix.TaskId,
            matrix.AcademicContextId,
            matrix.SemesterId,
            details,
            matrix.TotalQuestions,
            matrix.TotalScore,
            AllowedActions(matrix, actor),
            matrix.RejectComment,
            matrix.RejectedAt,
            matrix.RejectedByUserId);
    }

    private static IReadOnlyList<string> AllowedActions(
        ExamMatrix matrix,
        MatrixActor actor)
    {
        var actions = new List<string> { "View" };

        if (matrix.CanEdit(actor))
        {
            actions.Add("Update");
        }

        if (matrix.CanHardDelete(actor))
        {
            actions.Add("Delete");
        }

        if (matrix.Status == MatrixStatusCodes.Draft)
        {
            if (matrix.Details.Count > 0)
            {
                if (actor.Role == MatrixActorRole.Pht && matrix.TaskId is null)
                {
                    actions.Add("Confirm");
                }

                if (actor.Role == MatrixActorRole.Pht || matrix.TaskId is not null)
                {
                    actions.Add("Submit");
                }
            }
        }
        else if (matrix.Status == MatrixStatusCodes.Submitted)
        {
            if (actor.Role == MatrixActorRole.Pht)
            {
                actions.Add("Approve");
                actions.Add("Reject");
            }
        }
        else if (matrix.Status == MatrixStatusCodes.Approved)
        {
            if (actor.Role == MatrixActorRole.Pht)
            {
                actions.Add("Archive");
                actions.Add("Clone");
            }

            actions.Add("Export");
        }
        else if (matrix.Status == MatrixStatusCodes.Archived)
        {
            if (actor.Role == MatrixActorRole.Pht)
            {
                actions.Add("Clone");
            }

            actions.Add("Export");
        }

        return actions;
    }
}

public static class MatrixTaskMappingExtensions
{
    public static MatrixTaskFilter ToFilter(this MatrixTaskQuery query)
    {
        return new MatrixTaskFilter(
            query.Page,
            query.PageSize,
            query.Status,
            query.AssignedToUserId,
            query.DueBefore,
            query.BranchId);
    }

    public static MatrixTaskPage ToDto(this PagedResult<MatrixTaskRow> page)
    {
        var items = page.Items
            .Select(row => new MatrixTaskListItem(
                row.Id,
                row.CreatedByUserId,
                row.AssignedToUserId,
                row.DueAt,
                row.Status,
                row.TaskType,
                row.Description,
                row.AcademicContextId,
                row.SemesterId,
                row.MatrixId))
            .ToArray();

        return new MatrixTaskPage(items, page.Page, page.PageSize, page.TotalCount);
    }

    public static MatrixReferenceData ToDto(this MatrixReferenceModel model)
    {
        var cognitiveLevels = MatrixCognitiveLevels.All
            .Select(level => new MatrixCognitiveLevelOption(level.Code, level.Label))
            .ToArray();

        return new MatrixReferenceData(
            model.AcademicContexts,
            model.Semesters,
            model.Lessons,
            model.TeamLeads,
            cognitiveLevels);
    }

    public static MatrixTaskResponse ToResponse(this WorkTask task, ulong? matrixId)
    {
        return new MatrixTaskResponse(
            task.Id,
            task.AssignedToUserId,
            task.AcademicContextId ?? 0,
            task.SemesterId,
            task.DueAt,
            task.Status,
            task.TaskType,
            task.Description,
            matrixId);
    }
}
