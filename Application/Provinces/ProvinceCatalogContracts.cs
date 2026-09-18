namespace Application.Provinces;

public sealed record ProvinceOption(
    string Code,
    string Name,
    bool HasSchools,
    int ActiveSchoolCount);

public sealed record ProvinceSyncResult(
    string Provider,
    int ProvinceCount,
    DateTimeOffset SynchronizedAt);

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

public interface IProvinceCatalogService
{
    Task<IReadOnlyList<ProvinceOption>> ListAsync(CancellationToken cancellationToken);
}

public interface IProvinceSyncService
{
    Task<ProvinceSyncResult> SynchronizeAsync(CancellationToken cancellationToken);
}
