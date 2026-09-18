using System.Text.RegularExpressions;
using Application.AcademicYears;
using Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Authorize(Policy = "OperationalAdmin")]
[Route("api/academic-years")]
public sealed class AcademicYearsController(IAcademicYearService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<AcademicYearPage>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AcademicYearPage>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> List(
        [FromQuery] string provinceCode,
        [FromQuery] string? status = null,
        [FromQuery] string? search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var errors = ValidateListRequest(provinceCode, status, search, page, pageSize);
        if (errors.Count > 0)
        {
            return UnprocessableEntity(ApiResponse<AcademicYearPage>.Fail(
                "VALIDATION_ERROR",
                "Tham số truy vấn không hợp lệ.",
                errors));
        }

        var result = await service.ListAsync(
            new AcademicYearListQuery(provinceCode, status, search, page, pageSize),
            cancellationToken);
        return Ok(ApiResponse<AcademicYearPage>.Ok(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AcademicYearListItem>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<AcademicYearListItem>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<AcademicYearListItem>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<AcademicYearListItem>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAcademicYearRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        if (result.IsSuccess && result.Value is not null)
        {
            return Created(
                $"/api/academic-years/{result.Value.Id}",
                ApiResponse<AcademicYearListItem>.Ok(result.Value, "Đã tạo năm học."));
        }

        var error = result.Error!;
        var response = ApiResponse<AcademicYearListItem>.Fail(
            error.Code,
            error.Message,
            error.Details);

        return error.Code switch
        {
            "VALIDATION_ERROR" => UnprocessableEntity(response),
            "PROVINCE_NOT_FOUND" => NotFound(response),
            "ACADEMIC_YEAR_CONFLICT" => Conflict(response),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    private static IReadOnlyDictionary<string, string[]> ValidateListRequest(
        string? provinceCode,
        string? status,
        string? search,
        int page,
        int pageSize)
    {
        var errors = new Dictionary<string, string[]>();
        if (string.IsNullOrWhiteSpace(provinceCode) ||
            !Regex.IsMatch(provinceCode.Trim(), @"^\d{2}$", RegexOptions.CultureInvariant))
        {
            errors["provinceCode"] = ["Mã tỉnh phải gồm đúng 2 chữ số."];
        }

        if (page < 1) errors["page"] = ["Trang phải lớn hơn hoặc bằng 1."];
        if (pageSize is < 1 or > 100) errors["pageSize"] = ["Kích thước trang phải từ 1 đến 100."];
        if (status is not null && status is not ("DRAFT" or "ACTIVE" or "CLOSED"))
        {
            errors["status"] = ["Trạng thái phải là DRAFT, ACTIVE hoặc CLOSED."];
        }

        if (search?.Length > 100)
        {
            errors["search"] = ["Từ khóa tìm kiếm không được vượt quá 100 ký tự."];
        }

        return errors;
    }
}
