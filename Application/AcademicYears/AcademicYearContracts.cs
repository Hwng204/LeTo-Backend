namespace Application.AcademicYears;

public sealed record AcademicYearListItem(
    ulong Id,
    string ProvinceCode,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    string Status,
    uint Version);

public sealed record AcademicYearPage(
    IReadOnlyList<AcademicYearListItem> Items,
    int Page,
    int PageSize,
    int TotalCount)
{
    public int TotalPages => TotalCount == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
}

public sealed record ServiceError(
    string Code,
    string Message,
    IReadOnlyDictionary<string, string[]>? Details = null);

public sealed record ServiceResult<T>(T? Value, ServiceError? Error)
{
    public bool IsSuccess => Error is null;

    public static ServiceResult<T> Success(T value) => new(value, null);

    public static ServiceResult<T> Failure(
        string code,
        string message,
        IReadOnlyDictionary<string, string[]>? details = null) =>
        new(default, new ServiceError(code, message, details));
}

public interface IAcademicYearService
{
    Task<ServiceResult<AcademicYearListItem>> CreateAsync(
        CreateAcademicYearRequest request,
        CancellationToken cancellationToken);

    Task<AcademicYearPage> ListAsync(
        string provinceCode,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
