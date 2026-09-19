using Application.Common;
using Application.DTOs;
using Application.Mappings;
using Application.Services.Interface;
using Domain.Entities.Academic;
using Infrastructure.Repositories.Interface;
using Infrastructure.UnitOfWork;

namespace Application.Services.Implement;

public sealed class AcademicYearService(IUnitOfWork uow) : IAcademicYearService
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

        var createOutcome = await uow.AcademicYears.TryAddAsync(academicYear, cancellationToken);
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

        return ServiceResult<AcademicYearListItem>.Success(academicYear.ToListItem());
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
        var (items, totalCount) = await uow.AcademicYears.ListAsync(
            normalizedQuery.ToFilter(),
            cancellationToken);

        return new AcademicYearPage(
            items.Select(year => year.ToListItem()).ToArray(),
            normalizedQuery.Page,
            normalizedQuery.PageSize,
            totalCount);
    }

    public async Task<ServiceResult<AcademicYearDetailDto>> GetByIdAsync(
        ulong id,
        CancellationToken cancellationToken)
    {
        var year = await uow.AcademicYears.GetByIdWithSemestersAsync(id, cancellationToken);
        return year is null
            ? ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_NOT_FOUND",
                "Không tìm thấy năm học.")
            : ServiceResult<AcademicYearDetailDto>.Success(year.ToDetailDto());
    }

    public async Task<ServiceResult<AcademicYearDetailDto>> UpdateAsync(
        ulong id,
        UpdateAcademicYearRequest request,
        CancellationToken cancellationToken)
    {
        var validation = AcademicYearValidator.ValidateUpdate(request);
        if (!validation.IsValid)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "VALIDATION_ERROR",
                "Dữ liệu cập nhật năm học không hợp lệ.",
                validation.Errors);
        }

        var year = await uow.AcademicYears.GetByIdWithSemestersAsync(id, cancellationToken);
        if (year is null)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_NOT_FOUND",
                "Không tìm thấy năm học.");
        }

        try
        {
            year.EnsureCanUpdateSchedule(request.StartDate, request.EndDate);
        }
        catch (AcademicCalendarDomainException exception)
        {
            return DomainFailure<AcademicYearDetailDto>(exception);
        }

        var name = request.Name.Trim();
        var provinceCode = year.ProvinceCode ?? string.Empty;
        if (await uow.AcademicYears.HasConflictExceptCurrentAsync(
                provinceCode,
                year.Id,
                name,
                request.StartDate,
                request.EndDate,
                cancellationToken))
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_CONFLICT",
                "Năm học bị trùng tên hoặc chồng lấn thời gian với năm học khác trong cùng tỉnh.");
        }

        year.UpdateSchedule(name, request.StartDate, request.EndDate);
        if (!await uow.AcademicYears.UpdateAsync(year, cancellationToken))
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_CONFLICT",
                "Xung đột dữ liệu khi cập nhật năm học.");
        }

        return ServiceResult<AcademicYearDetailDto>.Success(year.ToDetailDto());
    }

    public async Task<ServiceResult<AcademicYearDetailDto>> ActivateAsync(
        ulong id,
        CancellationToken cancellationToken)
    {
        var year = await uow.AcademicYears.GetByIdWithSemestersAsync(id, cancellationToken);
        if (year is null)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_NOT_FOUND",
                "Không tìm thấy năm học.");
        }

        try
        {
            year.EnsureCanActivate();
        }
        catch (AcademicCalendarDomainException exception)
        {
            return DomainFailure<AcademicYearDetailDto>(exception);
        }

        var provinceCode = year.ProvinceCode ?? string.Empty;
        if (await uow.AcademicYears.HasActiveYearInProvinceAsync(
                provinceCode,
                year.Id,
                cancellationToken))
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACTIVE_YEAR_CONFLICT",
                "Tỉnh này đã có một năm học khác đang ở trạng thái áp dụng.");
        }

        year.Activate();
        if (!await uow.AcademicYears.UpdateAsync(year, cancellationToken))
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACTIVE_YEAR_CONFLICT",
                "Không thể kích hoạt vì đã có năm học khác đang áp dụng.");
        }

        return ServiceResult<AcademicYearDetailDto>.Success(year.ToDetailDto());
    }

    public async Task<ServiceResult<AcademicYearDetailDto>> CloseAsync(
        ulong id,
        CancellationToken cancellationToken)
    {
        var year = await uow.AcademicYears.GetByIdWithSemestersAsync(id, cancellationToken);
        if (year is null)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_NOT_FOUND",
                "Không tìm thấy năm học.");
        }

        try
        {
            year.Close();
        }
        catch (AcademicCalendarDomainException exception)
        {
            return DomainFailure<AcademicYearDetailDto>(exception);
        }

        await uow.CompleteAsync(cancellationToken);
        return ServiceResult<AcademicYearDetailDto>.Success(year.ToDetailDto());
    }

    public async Task<ServiceResult<AcademicYearDetailDto>> ConfigureTermsAsync(
        ulong id,
        ConfigureTermsRequest request,
        CancellationToken cancellationToken)
    {
        var year = await uow.AcademicYears.GetByIdWithSemestersAsync(id, cancellationToken);
        if (year is null)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_NOT_FOUND",
                "Không tìm thấy năm học.");
        }

        try
        {
            year.EnsureCanConfigureTerms();
        }
        catch (AcademicCalendarDomainException exception)
        {
            return DomainFailure<AcademicYearDetailDto>(exception);
        }

        var validation = AcademicYearValidator.ValidateConfigureTerms(
            request,
            year.StartDate,
            year.EndDate);
        if (!validation.IsValid)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "VALIDATION_ERROR",
                "Dữ liệu học kỳ không hợp lệ.",
                validation.Errors);
        }

        try
        {
            foreach (var item in request.Terms)
            {
                year.EnsureCanConfigureTerm(item.Order);
            }

            foreach (var item in request.Terms)
            {
                year.ConfigureTerm(
                    item.Order,
                    item.Name.Trim(),
                    item.StartDate,
                    item.EndDate);
            }
        }
        catch (AcademicCalendarDomainException exception)
        {
            return DomainFailure<AcademicYearDetailDto>(exception);
        }

        await uow.CompleteAsync(cancellationToken);
        return ServiceResult<AcademicYearDetailDto>.Success(year.ToDetailDto());
    }

    public async Task<ServiceResult<SemesterDto>> CloseTermAsync(
        ulong yearId,
        ulong termId,
        CancellationToken cancellationToken)
    {
        var year = await uow.AcademicYears.GetByIdWithSemestersAsync(yearId, cancellationToken);
        if (year is null)
        {
            return ServiceResult<SemesterDto>.Failure(
                "ACADEMIC_YEAR_NOT_FOUND",
                "Không tìm thấy năm học.");
        }

        Semester term;
        try
        {
            term = year.CloseTerm(termId);
        }
        catch (AcademicCalendarDomainException exception)
        {
            return DomainFailure<SemesterDto>(exception);
        }

        await uow.CompleteAsync(cancellationToken);
        return ServiceResult<SemesterDto>.Success(term.ToDto());
    }

    private static ServiceResult<T> DomainFailure<T>(AcademicCalendarDomainException exception) =>
        ServiceResult<T>.Failure(exception.Code, exception.Message);
}
