using Application.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace WebAPI.ExceptionHandling;

public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Unhandled API exception for {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(
            ApiResponse<object>.Fail(
                "UNEXPECTED_ERROR",
                "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau."),
            cancellationToken);

        return true;
    }
}
