using System.Globalization;

namespace Application.Provinces;

public sealed class ProvinceCatalogService(IProvinceRepository repository) : IProvinceCatalogService
{
    private static readonly StringComparer VietnameseNameComparer =
        StringComparer.Create(CultureInfo.GetCultureInfo("vi-VN"), ignoreCase: true);

    public async Task<IReadOnlyList<ProvinceOption>> ListAsync(
        CancellationToken cancellationToken)
    {
        var provinces = await repository.ListActiveAsync(cancellationToken);
        return provinces
            .OrderByDescending(province => province.HasSchools)
            .ThenBy(province => province.Name, VietnameseNameComparer)
            .ThenBy(province => province.Code, StringComparer.Ordinal)
            .ToArray();
    }
}
