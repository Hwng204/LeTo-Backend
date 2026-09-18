using Application.Common;
using Xunit;

namespace Application.Tests;

public sealed class ApiResponseContractTests
{
    [Fact]
    public void Fail_ProvidesStableCodeMessageAndFieldDetails()
    {
        var details = new Dictionary<string, string[]>
        {
            ["name"] = ["Name is required."]
        };

        var response = ApiResponse<object>.Fail(
            "VALIDATION_ERROR",
            "Request validation failed.",
            details);

        Assert.False(response.Success);
        Assert.Null(response.Data);
        Assert.NotNull(response.Error);
        Assert.Equal("VALIDATION_ERROR", response.Error.Code);
        Assert.Equal("Request validation failed.", response.Error.Message);
        Assert.NotNull(response.Error.Details);
        Assert.Equal(["Name is required."], response.Error.Details["name"]);
    }

    [Fact]
    public void Ok_DoesNotIncludeAnError()
    {
        var response = ApiResponse<string>.Ok("ready", "API is ready.");

        Assert.True(response.Success);
        Assert.Equal("ready", response.Data);
        Assert.Equal("API is ready.", response.Message);
        Assert.Null(response.Error);
    }
}
