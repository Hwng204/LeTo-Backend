using Domain.Entities.Academic;

namespace Domain.Entities.QuestionBank;

public sealed class MatrixDetail
{
    public ulong Id { get; set; }
    public ulong ExamMatrixId { get; set; }
    public ulong LessonId { get; set; }
    public string CognitiveLevel { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public uint QuestionCount { get; set; }
    public decimal AllocatedScore { get; set; }

    public ExamMatrix ExamMatrix { get; set; } = null!;
    public TextbookLesson Lesson { get; set; } = null!;
}
