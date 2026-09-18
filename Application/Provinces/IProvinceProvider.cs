namespace Application.Provinces;

public sealed record ProvinceCatalogItem(string Code, string Name, string DivisionType);

public interface IProvinceProvider
{
    string Name { get; }

    Task<IReadOnlyList<ProvinceCatalogItem>> FetchAsync(
        DateOnly asOfDate,
        CancellationToken cancellationToken);
}
