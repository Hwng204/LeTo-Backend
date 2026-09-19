namespace Domain.Entities.QuestionBank;

public sealed record MatrixDetailValue(
    ulong LessonId,
    string CognitiveLevel,
    string QuestionType,
    uint QuestionCount,
    decimal AllocatedScore);

