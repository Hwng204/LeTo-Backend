using System.Text.Json;
using Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using WebAPI.Errors;
using WebAPI.ExceptionHandling;
using Xunit;

namespace WebAPI.Tests;

public sealed class ApiExceptionHandlerTests
{
    [Fact]
    public async Task MatrixHandler_ReturnsFalseForUnknownExceptions()
    {
        var handled = await new MatrixExceptionHandler().TryHandleAsync(
            new DefaultHttpContext(),
            new InvalidOperationException("not a matrix error"),
            CancellationToken.None);

        Assert.False(handled);
    }

    [Fact]
    public async Task TryHandleAsync_ReturnsGenericApiResponseWithoutExceptionDetails()
    {
        var handler = new ApiExceptionHandler(NullLogger<ApiExceptionHandler>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var handled = await handler.TryHandleAsync(
            context,
            new InvalidOperationException("sensitive database detail"),
            CancellationToken.None);

        context.Response.Body.Position = 0;
        var response = await JsonSerializer.DeserializeAsync<ApiResponse<object>>(
            context.Response.Body,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Equal("UNEXPECTED_ERROR", response?.Error?.Code);
        Assert.DoesNotContain("sensitive", response?.Error?.Message ?? string.Empty);
    }
}
