namespace Application.DTOs;

public sealed record MatrixListQuery(
    int Page = 1,
    int PageSize = 20,
    string? Keyword = null,
    ulong? AcademicContextId = null,
    ulong? SemesterId = null,
    string? Status = null,
    ulong? AssignedToUserId = null,
    ulong? BranchId = null);

public sealed record MatrixListItem(
    ulong Id,
    string Name,
    string Status,
    ulong? TaskId,
    ulong AcademicContextId,
    ulong? SemesterId,
    uint TotalQuestions,
    decimal TotalScore)
{
    public string StatusLabel => Domain.Entities.QuestionBank.MatrixStatusCodes.Label(Status);
}

public sealed record MatrixPage(
    IReadOnlyList<MatrixListItem> Items,
    int Page,
    int PageSize,
    int TotalCount);
