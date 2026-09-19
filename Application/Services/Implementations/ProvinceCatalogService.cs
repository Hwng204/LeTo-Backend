using System.Globalization;
using Application.DTOs;
using Application.Services.Interface;
using Infrastructure.UnitOfWork;

namespace Application.Services.Implement;

public sealed class ProvinceCatalogService(IUnitOfWork uow) : IProvinceCatalogService
{
    private static readonly StringComparer VietnameseNameComparer =
        StringComparer.Create(CultureInfo.GetCultureInfo("vi-VN"), ignoreCase: true);

    public async Task<IReadOnlyList<ProvinceOption>> ListAsync(
        CancellationToken cancellationToken)
    {
        var provinces = await uow.Provinces.ListActiveAsync(cancellationToken);
        return provinces
            .OrderByDescending(province => province.HasSchools)
            .ThenBy(province => province.Name, VietnameseNameComparer)
            .ThenBy(province => province.Code, StringComparer.Ordinal)
            .Select(province => new ProvinceOption(
                province.Code,
                province.Name,
                province.HasSchools,
                province.ActiveSchoolCount))
            .ToArray();
    }
}
