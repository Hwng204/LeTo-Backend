using Domain.Entities.Academic;

namespace Application.AcademicYears;

public sealed class AcademicYearService(IAcademicYearRepository repository) : IAcademicYearService
{
    public async Task<ServiceResult<AcademicYearListItem>> CreateAsync(
        CreateAcademicYearRequest request,
        CancellationToken cancellationToken)
    {
        var validation = AcademicYearValidator.Validate(request);
        if (!validation.IsValid)
        {
            return ServiceResult<AcademicYearListItem>.Failure(
                "VALIDATION_ERROR",
                "Dữ liệu năm học không hợp lệ.",
                validation.Errors);
        }

        var provinceCode = request.ProvinceCode.Trim();
        var name = request.Name.Trim();

        var academicYear = new AcademicYear
        {
            ProvinceCode = provinceCode,
            Name = name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = "DRAFT",
            Semesters =
            [
                new Semester { Order = 1, Name = "Học kỳ 1", Status = "PLANNED" },
                new Semester { Order = 2, Name = "Học kỳ 2", Status = "PLANNED" }
            ]
        };
        academicYear.AssignCode($"{provinceCode}-{name}");

        var createOutcome = await repository.TryAddAsync(academicYear, cancellationToken);
        if (createOutcome == AcademicYearCreateOutcome.ProvinceNotFound)
        {
            return ServiceResult<AcademicYearListItem>.Failure(
                "PROVINCE_NOT_FOUND",
                "Tỉnh đã chọn không tồn tại hoặc không còn hoạt động.");
        }

        if (createOutcome == AcademicYearCreateOutcome.Conflict)
        {
            return ServiceResult<AcademicYearListItem>.Failure(
                "ACADEMIC_YEAR_CONFLICT",
                "Năm học bị trùng tên hoặc chồng lấn thời gian trong cùng tỉnh.");
        }

        return ServiceResult<AcademicYearListItem>.Success(Map(academicYear));
    }

    public async Task<AcademicYearPage> ListAsync(
        AcademicYearListQuery query,
        CancellationToken cancellationToken)
    {
        var normalizedQuery = query with
        {
            ProvinceCode = query.ProvinceCode.Trim(),
            Search = string.IsNullOrWhiteSpace(query.Search) ? null : query.Search.Trim()
        };
        var (items, totalCount) = await repository.ListAsync(normalizedQuery, cancellationToken);

        return new AcademicYearPage(
            items.Select(Map).ToArray(),
            normalizedQuery.Page,
            normalizedQuery.PageSize,
            totalCount);
    }

    private static AcademicYearListItem Map(AcademicYear year) =>
        new(
            year.Id,
            year.Code ?? string.Empty,
            year.ProvinceCode ?? string.Empty,
            year.Name,
            year.StartDate,
            year.EndDate,
            year.Status,
            year.Version);
}
