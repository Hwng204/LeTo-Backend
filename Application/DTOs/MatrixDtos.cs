namespace Application.DTOs;

public sealed record MatrixDetailRequest(
    ulong LessonId,
    string CognitiveLevel,
    uint QuestionCount,
    decimal AllocatedScore);

public sealed record SaveMatrixRequest(
    string Name,
    ulong AcademicContextId,
    ulong? SemesterId,
    ulong? TaskId,
    IReadOnlyList<MatrixDetailRequest> Details);

public sealed record MatrixDetailResponse(
    ulong Id,
    ulong LessonId,
    string CognitiveLevel,
    string QuestionType,
    uint QuestionCount,
    decimal AllocatedScore);

public sealed record MatrixResponse(
    ulong Id,
    string Name,
    string Status,
    ulong? TaskId,
    ulong AcademicContextId,
    ulong? SemesterId,
    IReadOnlyList<MatrixDetailResponse> Details,
    uint TotalQuestions,
    decimal TotalScore,
    IReadOnlyList<string> AllowedActions)
{
    public string StatusLabel => Domain.Entities.QuestionBank.MatrixStatusCodes.Label(Status);
}


public sealed record MatrixExportInfo(
    string ContextLabel,
    string? SemesterName,
    IReadOnlyDictionary<ulong, string> LessonTitles);

public sealed record MatrixExportFile(string FileName, byte[] Content);
