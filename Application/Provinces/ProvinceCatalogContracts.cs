namespace Application.Provinces;

public sealed record ProvinceOption(string Code, string Name, bool HasSchools);

public interface IProvinceRepository
{
    Task<IReadOnlyList<ProvinceOption>> ListActiveAsync(CancellationToken cancellationToken);
}

public interface IProvinceCatalogService
{
    Task<IReadOnlyList<ProvinceOption>> ListAsync(CancellationToken cancellationToken);
}
