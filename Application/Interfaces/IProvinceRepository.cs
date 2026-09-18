using Application.DTOs;

namespace Application.Interfaces;

public interface IProvinceRepository
{
    Task<IReadOnlyList<ProvinceOption>> ListActiveAsync(CancellationToken cancellationToken);

    Task<IReadOnlySet<string>> ListActiveCodesAsync(CancellationToken cancellationToken);

    Task SynchronizeAsync(
        IReadOnlyList<ProvinceCatalogItem> provinces,
        string source,
        DateTimeOffset synchronizedAt,
        CancellationToken cancellationToken);
}
