using Application.DTOs;
using Application.Interfaces;
using Application.Services.Implement;
using Xunit;

namespace Application.Tests;

public sealed class ProvinceSyncServiceTests
{
    [Fact]
    public async Task SynchronizeAsync_PersistsOnlyAfterProviderPayloadIsAccepted()
    {
        var provinces = Enumerable.Range(1, 34)
            .Select(index => new ProvinceCatalogItem(
                index.ToString("00"),
                $"Tỉnh {index}",
                "Tỉnh"))
            .ToArray();
        var repository = new FakeRepository(new HashSet<string>());
        var service = new ProvinceSyncService(
            new FakeProvider(provinces),
            repository,
            new FixedTimeProvider(new DateTimeOffset(2026, 9, 18, 10, 0, 0, TimeSpan.Zero)));

        var result = await service.SynchronizeAsync(CancellationToken.None);

        Assert.Equal("TEST_PROVIDER", result.Provider);
        Assert.Equal(34, result.ProvinceCount);
        Assert.Equal(provinces, repository.SynchronizedProvinces);
    }

    [Fact]
    public async Task SynchronizeAsync_RejectsSuspiciousRemovalWithoutChangingCatalog()
    {
        var existingCodes = Enumerable.Range(1, 40)
            .Select(index => index.ToString("00"))
            .ToHashSet(StringComparer.Ordinal);
        var incoming = Enumerable.Range(1, 20)
            .Select(index => new ProvinceCatalogItem(index.ToString("00"), $"Tỉnh {index}", "Tỉnh"))
            .ToArray();
        var repository = new FakeRepository(existingCodes);
        var service = new ProvinceSyncService(
            new FakeProvider(incoming),
            repository,
            TimeProvider.System);

        await Assert.ThrowsAsync<InvalidDataException>(() =>
            service.SynchronizeAsync(CancellationToken.None));

        Assert.Null(repository.SynchronizedProvinces);
    }

    private sealed class FakeProvider(IReadOnlyList<ProvinceCatalogItem> provinces)
        : IProvinceProvider
    {
        public string Name => "TEST_PROVIDER";

        public Task<IReadOnlyList<ProvinceCatalogItem>> FetchAsync(
            DateOnly asOfDate,
            CancellationToken cancellationToken) => Task.FromResult(provinces);
    }

    private sealed class FakeRepository(IReadOnlySet<string> activeCodes) : IProvinceRepository
    {
        public IReadOnlyList<ProvinceCatalogItem>? SynchronizedProvinces { get; private set; }

        public Task<IReadOnlyList<ProvinceOption>> ListActiveAsync(
            CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<ProvinceOption>>([]);

        public Task<IReadOnlySet<string>> ListActiveCodesAsync(
            CancellationToken cancellationToken) => Task.FromResult(activeCodes);

        public Task SynchronizeAsync(
            IReadOnlyList<ProvinceCatalogItem> provinces,
            string source,
            DateTimeOffset synchronizedAt,
            CancellationToken cancellationToken)
        {
            SynchronizedProvinces = provinces;
            return Task.CompletedTask;
        }
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }
}
