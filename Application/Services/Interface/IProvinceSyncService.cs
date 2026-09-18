using Application.DTOs;

namespace Application.Services.Interface;

public interface IProvinceSyncService
{
    Task<ProvinceSyncResult> SynchronizeAsync(CancellationToken cancellationToken);
}
