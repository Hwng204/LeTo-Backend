namespace Application.DTOs;

public sealed record MatrixAcademicContextOption(
    ulong Id,
    string Label,
    ulong AcademicYearId,
    ulong SchoolBranchId,
    ulong TextbookId,
    ulong SubjectId,
    ulong GradeLevelId);

public sealed record MatrixSemesterOption(
    ulong Id,
    ulong AcademicYearId,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate);

public sealed record MatrixLessonOption(
    ulong Id,
    ulong ContextId,
    ulong ChapterId,
    string Title,
    uint SortOrder);

public sealed record MatrixTeamLeadOption(
    ulong Id,
    string Username,
    string FullName,
    ulong? SchoolBranchId);

public sealed record MatrixCognitiveLevelOption(string Code, string Label);

public sealed record MatrixReferenceData(
    IReadOnlyList<MatrixAcademicContextOption> AcademicContexts,
    IReadOnlyList<MatrixSemesterOption> Semesters,
    IReadOnlyList<MatrixLessonOption> Lessons,
    IReadOnlyList<MatrixTeamLeadOption> TeamLeads,
    IReadOnlyList<MatrixCognitiveLevelOption>? CognitiveLevels = null);
