namespace Domain.Entities.Academic;

public sealed class TextbookLesson
{
    public ulong Id { get; set; }
    public ulong ChapterId { get; set; }
    public string? Content { get; set; }
    public string Title { get; set; } = string.Empty;
    public uint SortOrder { get; set; }

    public TextbookChapter Chapter { get; set; } = null!;
}
