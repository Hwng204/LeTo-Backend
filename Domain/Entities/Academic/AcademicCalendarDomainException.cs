namespace Domain.Entities.Academic;

public sealed class AcademicCalendarDomainException(string code, string message)
    : InvalidOperationException(message)
{
    public string Code { get; } = code;
}
