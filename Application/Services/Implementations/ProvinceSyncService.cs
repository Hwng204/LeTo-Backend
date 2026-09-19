using Application.DTOs;
using Application.Services.Interface;
using Infrastructure.External.Provinces;
using Infrastructure.UnitOfWork;

namespace Application.Services.Implement;

public sealed class ProvinceSyncService(
    IProvinceProvider provider,
    IUnitOfWork uow,
    TimeProvider timeProvider) : IProvinceSyncService
{
    private const double MaximumRemovalRatio = 0.25;
    private const int MinimumUnexpectedRemovals = 2;

    public async Task<ProvinceSyncResult> SynchronizeAsync(CancellationToken cancellationToken)
    {
        var synchronizedAt = timeProvider.GetUtcNow();
        var catalog = await provider.FetchAsync(
            DateOnly.FromDateTime(synchronizedAt.UtcDateTime),
            cancellationToken);
        var activeCodes = await uow.Provinces.ListActiveCodesAsync(cancellationToken);

        RejectSuspiciousRemovals(activeCodes, catalog);

        await uow.Provinces.SynchronizeAsync(
            catalog,
            provider.Name,
            synchronizedAt,
            cancellationToken);

        return new ProvinceSyncResult(provider.Name, catalog.Count, synchronizedAt);
    }

    private static void RejectSuspiciousRemovals(
        IReadOnlySet<string> activeCodes,
        IReadOnlyList<ProvinceCatalogRecord> catalog)
    {
        if (activeCodes.Count == 0)
        {
            return;
        }

        var incomingCodes = catalog
            .Select(province => province.Code)
            .ToHashSet(StringComparer.Ordinal);
        var removedCount = activeCodes.Count(code => !incomingCodes.Contains(code));
        var maximumAllowedRemovals = Math.Max(
            MinimumUnexpectedRemovals,
            (int)Math.Ceiling(activeCodes.Count * MaximumRemovalRatio));

        if (removedCount > maximumAllowedRemovals)
        {
            throw new InvalidDataException(
                $"Province catalog unexpectedly removed {removedCount} of " +
                $"{activeCodes.Count} active records.");
        }
    }
}
