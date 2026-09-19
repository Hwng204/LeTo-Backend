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
