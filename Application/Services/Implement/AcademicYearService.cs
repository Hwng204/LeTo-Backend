using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Application.Services.Interface;
using Domain.Entities.Academic;

namespace Application.Services.Implement;

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
        var (items, totalCount) = await repository.ListAsync(normalizedQuery, cancellationToken);

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
        var year = await repository.GetByIdWithSemestersAsync(id, cancellationToken);
        if (year is null)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_NOT_FOUND",
                "Không tìm thấy năm học.");
        }

        return ServiceResult<AcademicYearDetailDto>.Success(year.ToDetailDto());
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

        var year = await repository.GetByIdWithSemestersAsync(id, cancellationToken);
        if (year is null)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_NOT_FOUND",
                "Không tìm thấy năm học.");
        }

        if (year.Status == "CLOSED")
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_CLOSED",
                "Năm học đã đóng không thể chỉnh sửa.");
        }

        foreach (var sem in year.Semesters)
        {
            if (sem.StartDate.HasValue && sem.StartDate.Value < request.StartDate)
            {
                return ServiceResult<AcademicYearDetailDto>.Failure(
                    "SEMESTER_OUT_OF_BOUNDS",
                    $"Ngày bắt đầu năm học ({request.StartDate:dd/MM/yyyy}) không thể sau ngày bắt đầu của {sem.Name} ({sem.StartDate.Value:dd/MM/yyyy}).");
            }
            if (sem.EndDate.HasValue && sem.EndDate.Value > request.EndDate)
            {
                return ServiceResult<AcademicYearDetailDto>.Failure(
                    "SEMESTER_OUT_OF_BOUNDS",
                    $"Ngày kết thúc năm học ({request.EndDate:dd/MM/yyyy}) không thể trước ngày kết thúc của {sem.Name} ({sem.EndDate.Value:dd/MM/yyyy}).");
            }
        }

        var name = request.Name.Trim();
        var provinceCode = year.ProvinceCode ?? string.Empty;

        if (await repository.HasConflictExceptCurrentAsync(
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

        year.Name = name;
        year.StartDate = request.StartDate;
        year.EndDate = request.EndDate;
        year.Version++;

        if (!await repository.UpdateAsync(year, cancellationToken))
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
        var year = await repository.GetByIdWithSemestersAsync(id, cancellationToken);
        if (year is null)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_NOT_FOUND",
                "Không tìm thấy năm học.");
        }

        if (year.Status == "ACTIVE")
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_ALREADY_ACTIVE",
                "Năm học này đang ở trạng thái áp dụng.");
        }

        if (year.Status == "CLOSED")
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_CLOSED",
                "Năm học đã đóng không thể kích hoạt lại.");
        }

        if (year.Semesters.Count != 2)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "INCOMPLETE_TERMS",
                "Năm học cần có đầy đủ 2 học kỳ trước khi kích hoạt.");
        }

        var provinceCode = year.ProvinceCode ?? string.Empty;
        if (await repository.HasActiveYearInProvinceAsync(provinceCode, year.Id, cancellationToken))
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACTIVE_YEAR_CONFLICT",
                "Tỉnh này đã có một năm học khác đang ở trạng thái áp dụng.");
        }

        year.Status = "ACTIVE";
        year.Version++;

        if (!await repository.UpdateAsync(year, cancellationToken))
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
        var year = await repository.GetByIdWithSemestersAsync(id, cancellationToken);
        if (year is null)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_NOT_FOUND",
                "Không tìm thấy năm học.");
        }

        if (year.Status == "CLOSED")
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_ALREADY_CLOSED",
                "Năm học đã đóng từ trước.");
        }

        year.Status = "CLOSED";
        year.Version++;

        foreach (var semester in year.Semesters)
        {
            if (semester.Status != "CLOSED")
            {
                semester.Status = "CLOSED";
                semester.Version++;
            }
        }

        await repository.CommitAsync(cancellationToken);
        return ServiceResult<AcademicYearDetailDto>.Success(year.ToDetailDto());
    }

    public async Task<ServiceResult<AcademicYearDetailDto>> ConfigureTermsAsync(
        ulong id,
        ConfigureTermsRequest request,
        CancellationToken cancellationToken)
    {
        var year = await repository.GetByIdWithSemestersAsync(id, cancellationToken);
        if (year is null)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_NOT_FOUND",
                "Không tìm thấy năm học.");
        }

        if (year.Status == "CLOSED")
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "ACADEMIC_YEAR_CLOSED",
                "Năm học đã đóng không thể cấu hình học kỳ.");
        }

        var validation = AcademicYearValidator.ValidateConfigureTerms(request, year.StartDate, year.EndDate);
        if (!validation.IsValid)
        {
            return ServiceResult<AcademicYearDetailDto>.Failure(
                "VALIDATION_ERROR",
                "Dữ liệu học kỳ không hợp lệ.",
                validation.Errors);
        }

        foreach (var item in request.Terms)
        {
            var term = year.Semesters.FirstOrDefault(s => s.Order == item.Order);
            if (term is not null)
            {
                if (term.Status == "CLOSED")
                {
                    return ServiceResult<AcademicYearDetailDto>.Failure(
                        "TERM_CLOSED",
                        $"Học kỳ {item.Order} đã đóng không thể sửa.");
                }

                term.Name = item.Name.Trim();
                term.StartDate = item.StartDate;
                term.EndDate = item.EndDate;
                term.Version++;
            }
        }

        await repository.CommitAsync(cancellationToken);
        return ServiceResult<AcademicYearDetailDto>.Success(year.ToDetailDto());
    }

    public async Task<ServiceResult<SemesterDto>> CloseTermAsync(
        ulong yearId,
        ulong termId,
        CancellationToken cancellationToken)
    {
        var year = await repository.GetByIdWithSemestersAsync(yearId, cancellationToken);
        if (year is null)
        {
            return ServiceResult<SemesterDto>.Failure(
                "ACADEMIC_YEAR_NOT_FOUND",
                "Không tìm thấy năm học.");
        }

        var term = year.Semesters.FirstOrDefault(s => s.Id == termId);
        if (term is null)
        {
            return ServiceResult<SemesterDto>.Failure(
                "TERM_NOT_FOUND",
                "Không tìm thấy học kỳ.");
        }

        if (term.Status == "CLOSED")
        {
            return ServiceResult<SemesterDto>.Failure(
                "TERM_ALREADY_CLOSED",
                "Học kỳ đã đóng từ trước.");
        }

        term.Status = "CLOSED";
        term.Version++;

        await repository.CommitAsync(cancellationToken);
        return ServiceResult<SemesterDto>.Success(term.ToDto());
    }
}
