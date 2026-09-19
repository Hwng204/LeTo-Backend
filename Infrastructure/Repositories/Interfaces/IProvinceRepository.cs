using Infrastructure.External.Provinces;

namespace Infrastructure.Repositories.Interface;

public sealed record ProvinceOptionRow(
    string Code,
    string Name,
    bool HasSchools,
    int ActiveSchoolCount);

public interface IProvinceRepository
{
    Task<IReadOnlyList<ProvinceOptionRow>> ListActiveAsync(CancellationToken cancellationToken);

    Task<IReadOnlySet<string>> ListActiveCodesAsync(CancellationToken cancellationToken);

    Task SynchronizeAsync(
        IReadOnlyList<ProvinceCatalogRecord> provinces,
        string source,
        DateTimeOffset synchronizedAt,
        CancellationToken cancellationToken);
}
