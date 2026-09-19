using Application.Common;
using Application.DTOs;

namespace Application.Services.Interface;

public interface IAcademicYearService
{
    Task<ServiceResult<AcademicYearListItem>> CreateAsync(
        CreateAcademicYearRequest request,
        CancellationToken cancellationToken);

    Task<AcademicYearPage> ListAsync(
        AcademicYearListQuery query,
        CancellationToken cancellationToken);

    Task<ServiceResult<AcademicYearDetailDto>> GetByIdAsync(
        ulong id,
        CancellationToken cancellationToken);

    Task<ServiceResult<AcademicYearDetailDto>> UpdateAsync(
        ulong id,
        UpdateAcademicYearRequest request,
        CancellationToken cancellationToken);

    Task<ServiceResult<AcademicYearDetailDto>> ActivateAsync(
        ulong id,
        CancellationToken cancellationToken);

    Task<ServiceResult<AcademicYearDetailDto>> CloseAsync(
        ulong id,
        CancellationToken cancellationToken);

    Task<ServiceResult<AcademicYearDetailDto>> ConfigureTermsAsync(
        ulong id,
        ConfigureTermsRequest request,
        CancellationToken cancellationToken);

    Task<ServiceResult<SemesterDto>> CloseTermAsync(
        ulong yearId,
        ulong termId,
        CancellationToken cancellationToken);
}
