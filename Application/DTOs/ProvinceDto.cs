namespace Application.DTOs;

public sealed record ProvinceOption(
    string Code,
    string Name,
    bool HasSchools,
    int ActiveSchoolCount);

public sealed record ProvinceSyncResult(
    string Provider,
    int ProvinceCount,
    DateTimeOffset SynchronizedAt);

public sealed record ProvinceCatalogItem(
    string Code,
    string Name,
    string DivisionType);
