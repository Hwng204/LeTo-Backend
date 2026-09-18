using Application.DTOs;

namespace Application.Services.Interfaces;

public interface IProvinceSyncService
{
    Task<ProvinceSyncResult> SynchronizeAsync(CancellationToken cancellationToken);
}
