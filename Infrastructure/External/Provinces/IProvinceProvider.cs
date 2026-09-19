namespace Infrastructure.External.Provinces;

public sealed record ProvinceCatalogRecord(
    string Code,
    string Name,
    string DivisionType);

public interface IProvinceProvider
{
    string Name { get; }

    Task<IReadOnlyList<ProvinceCatalogRecord>> FetchAsync(
        DateOnly asOfDate,
        CancellationToken cancellationToken);
}
