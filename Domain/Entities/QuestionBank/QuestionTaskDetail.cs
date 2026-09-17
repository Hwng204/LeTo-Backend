namespace Domain.Entities.QuestionBank;

public sealed class QuestionTaskDetail
{
    public ulong Id { get; set; }
    public ulong QuestionTaskId { get; set; }
    public string CognitiveLevel { get; set; } = string.Empty;
    public uint QuestionCount { get; set; }
    public string QuestionType { get; set; } = string.Empty;

    public QuestionTask QuestionTask { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
