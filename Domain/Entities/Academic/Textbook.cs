namespace Domain.Entities.Academic;

public sealed class Textbook
{
    public ulong Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? BookSet { get; set; }

    public ICollection<TextbookChapter> Chapters { get; set; } = new List<TextbookChapter>();
}
