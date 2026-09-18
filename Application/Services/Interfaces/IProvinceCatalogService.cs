using Application.DTOs;

namespace Application.Services.Interfaces;

public interface IProvinceCatalogService
{
    Task<IReadOnlyList<ProvinceOption>> ListAsync(CancellationToken cancellationToken);
}
