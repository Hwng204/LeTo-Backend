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

        if (!await repository.ProvinceExistsAsync(provinceCode, cancellationToken))
        {
            return ServiceResult<AcademicYearListItem>.Failure(
                "PROVINCE_NOT_FOUND",
                "Tỉnh đã chọn không tồn tại hoặc không còn hoạt động.");
        }

        if (await repository.HasConflictAsync(
                provinceCode,
                name,
                request.StartDate,
                request.EndDate,
                cancellationToken))
        {
            return ServiceResult<AcademicYearListItem>.Failure(
                "ACADEMIC_YEAR_CONFLICT",
                "Năm học bị trùng tên hoặc chồng lấn thời gian trong cùng tỉnh.");
        }

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

        if (!await repository.TryAddAsync(academicYear, cancellationToken))
        {
            return ServiceResult<AcademicYearListItem>.Failure(
                "ACADEMIC_YEAR_CONFLICT",
                "Năm học vừa được tạo bởi một yêu cầu khác. Vui lòng tải lại dữ liệu.");
        }

        return ServiceResult<AcademicYearListItem>.Success(Map(academicYear));
    }

    public async Task<AcademicYearPage> ListAsync(
        string provinceCode,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await repository.ListAsync(
            provinceCode.Trim(),
            page,
            pageSize,
            cancellationToken);

        return new AcademicYearPage(items.Select(Map).ToArray(), page, pageSize, totalCount);
    }

    private static AcademicYearListItem Map(AcademicYear year) =>
        new(
            year.Id,
            year.ProvinceCode ?? string.Empty,
            year.Name,
            year.StartDate,
            year.EndDate,
            year.Status,
            year.Version);
}
