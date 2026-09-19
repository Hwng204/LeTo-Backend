namespace Infrastructure.Exports;

public sealed record MatrixWorkbookRow(
    string LessonTitle,
    string CognitiveLevel,
    string QuestionType,
    uint QuestionCount,
    decimal AllocatedScore);

public sealed record MatrixWorkbookModel(
    string Name,
    string Status,
    ulong? TaskId,
    string ContextLabel,
    string SemesterName,
    uint TotalQuestions,
    decimal TotalScore,
    IReadOnlyList<MatrixWorkbookRow> Rows);

public interface IMatrixWorkbookExporter
{
    byte[] Create(MatrixWorkbookModel matrix);
}
