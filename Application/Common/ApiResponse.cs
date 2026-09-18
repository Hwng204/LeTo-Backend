namespace Application.Common;

public sealed record ApiError(
    string Code,
    string Message,
    IReadOnlyDictionary<string, string[]>? Details = null);

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public ApiError? Error { get; set; }

    public static ApiResponse<T> Ok(T data, string? message = null) =>
        new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> Fail(
        string code,
        string message,
        IReadOnlyDictionary<string, string[]>? details = null) =>
        new()
        {
            Success = false,
            Message = message,
            Error = new ApiError(code, message, details)
        };
}
