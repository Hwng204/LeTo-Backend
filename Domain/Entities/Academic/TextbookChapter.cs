namespace Domain.Entities.Academic;

public sealed class TextbookChapter
{
    public ulong Id { get; set; }
    public ulong TextbookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public uint SortOrder { get; set; }

    public Textbook Textbook { get; set; } = null!;
    public ICollection<TextbookLesson> Lessons { get; set; } = new List<TextbookLesson>();
}
