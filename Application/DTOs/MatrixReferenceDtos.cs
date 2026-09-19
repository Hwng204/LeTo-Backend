using Infrastructure.Models;

namespace Application.DTOs;

public sealed record MatrixCognitiveLevelOption(string Code, string Label);

public sealed record MatrixReferenceData(
    IReadOnlyList<MatrixAcademicContextOption> AcademicContexts,
    IReadOnlyList<MatrixSemesterOption> Semesters,
    IReadOnlyList<MatrixLessonOption> Lessons,
    IReadOnlyList<MatrixTeamLeadOption> TeamLeads,
    IReadOnlyList<MatrixCognitiveLevelOption>? CognitiveLevels = null);
