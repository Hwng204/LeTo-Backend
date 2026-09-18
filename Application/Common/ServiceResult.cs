namespace Application.Common;

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
