using Application.AcademicYears;
using Xunit;

namespace Application.Tests;

public sealed class AcademicYearValidatorTests
{
    [Fact]
    public void Validate_AcceptsAValidAcademicYear()
    {
        var input = new CreateAcademicYearRequest(
            "01",
            "2026-2027",
            new DateOnly(2026, 8, 15),
            new DateOnly(2027, 5, 31));

        var result = AcademicYearValidator.Validate(input);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("", "Tên năm học là bắt buộc.")]
    [InlineData("2026/2027", "Tên năm học phải có định dạng YYYY-YYYY.")]
    [InlineData("2026-2028", "Năm kết thúc phải ngay sau năm bắt đầu.")]
    public void Validate_RejectsInvalidNames(string name, string expectedMessage)
    {
        var input = new CreateAcademicYearRequest(
            "01",
            name,
            new DateOnly(2026, 8, 15),
            new DateOnly(2027, 5, 31));

        var result = AcademicYearValidator.Validate(input);

        Assert.False(result.IsValid);
        Assert.Contains(expectedMessage, result.Errors["name"]);
    }

    [Theory]
    [InlineData(2026, 8, 15, 2026, 8, 15)]
    [InlineData(2027, 5, 31, 2026, 8, 15)]
    public void Validate_RequiresEndDateAfterStartDate(
        int startYear,
        int startMonth,
        int startDay,
        int endYear,
        int endMonth,
        int endDay)
    {
        var input = new CreateAcademicYearRequest(
            "01",
            "2026-2027",
            new DateOnly(startYear, startMonth, startDay),
            new DateOnly(endYear, endMonth, endDay));

        var result = AcademicYearValidator.Validate(input);

        Assert.False(result.IsValid);
        Assert.Contains("Ngày kết thúc phải sau ngày bắt đầu.", result.Errors["endDate"]);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("VN-01")]
    public void Validate_RejectsInvalidProvinceCodes(string provinceCode)
    {
        var input = new CreateAcademicYearRequest(
            provinceCode,
            "2026-2027",
            new DateOnly(2026, 8, 15),
            new DateOnly(2027, 5, 31));

        var result = AcademicYearValidator.Validate(input);

        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors["provinceCode"]);
    }

    [Fact]
    public void Validate_RequiresDatesToMatchTheNamedYears()
    {
        var input = new CreateAcademicYearRequest(
            "01",
            "2026-2027",
            new DateOnly(2025, 8, 15),
            new DateOnly(2027, 5, 31));

        var result = AcademicYearValidator.Validate(input);

        Assert.False(result.IsValid);
        Assert.Contains("Ngày bắt đầu phải thuộc năm 2026.", result.Errors["startDate"]);
    }
}
