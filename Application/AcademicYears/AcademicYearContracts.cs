namespace Application.AcademicYears;

public sealed record CreateAcademicYearRequest(
    string ProvinceCode,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate);

public sealed record AcademicYearListItem(
    ulong Id,
    string Code,
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

public sealed record AcademicYearListQuery(
    string ProvinceCode,
    string? Status,
    string? Search,
    int Page,
    int PageSize);

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

public sealed record SemesterDto(
    ulong Id,
    byte Order,
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate,
    string Status,
    uint Version);

public sealed record AcademicYearDetailDto(
    ulong Id,
    string Code,
    string ProvinceCode,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    string Status,
    uint Version,
    IReadOnlyList<SemesterDto> Semesters);

public sealed record UpdateAcademicYearRequest(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate);

public sealed record ConfigureTermItem(
    byte Order,
    string Name,
    DateOnly? StartDate,
    DateOnly? EndDate);

public sealed record ConfigureTermsRequest(
    IReadOnlyList<ConfigureTermItem> Terms);

public interface IAcademicYearService
{
    Task<ServiceResult<AcademicYearListItem>> CreateAsync(
        CreateAcademicYearRequest request,
        CancellationToken cancellationToken);

    Task<AcademicYearPage> ListAsync(
        AcademicYearListQuery query,
        CancellationToken cancellationToken);

    Task<ServiceResult<AcademicYearDetailDto>> GetByIdAsync(
        ulong id,
        CancellationToken cancellationToken);

    Task<ServiceResult<AcademicYearDetailDto>> UpdateAsync(
        ulong id,
        UpdateAcademicYearRequest request,
        CancellationToken cancellationToken);

    Task<ServiceResult<AcademicYearDetailDto>> ActivateAsync(
        ulong id,
        CancellationToken cancellationToken);

    Task<ServiceResult<AcademicYearDetailDto>> CloseAsync(
        ulong id,
        CancellationToken cancellationToken);

    Task<ServiceResult<AcademicYearDetailDto>> ConfigureTermsAsync(
        ulong id,
        ConfigureTermsRequest request,
        CancellationToken cancellationToken);

    Task<ServiceResult<SemesterDto>> CloseTermAsync(
        ulong yearId,
        ulong termId,
        CancellationToken cancellationToken);
}
