using Application.DTOs;

namespace Application.Interfaces;

public interface IProvinceProvider
{
    string Name { get; }

    Task<IReadOnlyList<ProvinceCatalogItem>> FetchAsync(
        DateOnly asOfDate,
        CancellationToken cancellationToken);
}
