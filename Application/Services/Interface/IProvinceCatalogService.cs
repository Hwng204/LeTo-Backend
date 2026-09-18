using Application.DTOs;

namespace Application.Services.Interface;

public interface IProvinceCatalogService
{
    Task<IReadOnlyList<ProvinceOption>> ListAsync(CancellationToken cancellationToken);
}
