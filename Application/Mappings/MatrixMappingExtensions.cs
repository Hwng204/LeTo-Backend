using Application.DTOs;
using Domain.Entities.QuestionBank;

namespace Application.Mappings;

public static class MatrixMappingExtensions
{
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
            AllowedActions(matrix, actor));
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
            if (actor.Role == MatrixActorRole.TeamLead)
            {
                actions.Add("Withdraw");
            }

            if (actor.Role == MatrixActorRole.Pht)
            {
                actions.Add("Approve");
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
