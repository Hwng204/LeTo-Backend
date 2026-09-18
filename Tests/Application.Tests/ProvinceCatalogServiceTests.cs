using Application.DTOs;
using Application.Interfaces;
using Application.Services.Implement;
using Xunit;

namespace Application.Tests;

public sealed class ProvinceCatalogServiceTests
{
    [Fact]
    public async Task ListAsync_PrioritizesProvincesWithSchoolsThenSortsByName()
    {
        var repository = new FakeProvinceRepository(
        [
            new ProvinceOption("79", "Thành phố Hồ Chí Minh", false, 0),
            new ProvinceOption("48", "Đà Nẵng", true, 3),
            new ProvinceOption("01", "Hà Nội", true, 1)
        ]);
        var service = new ProvinceCatalogService(repository);

        var result = await service.ListAsync(CancellationToken.None);

        Assert.Collection(
            result,
            province => Assert.Equal("48", province.Code),
            province => Assert.Equal("01", province.Code),
            province => Assert.Equal("79", province.Code));
    }

    private sealed class FakeProvinceRepository(IReadOnlyList<ProvinceOption> options)
        : IProvinceRepository
    {
        public Task<IReadOnlyList<ProvinceOption>> ListActiveAsync(
            CancellationToken cancellationToken) => Task.FromResult(options);

        public Task<IReadOnlySet<string>> ListActiveCodesAsync(
            CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlySet<string>>(new HashSet<string>());

        public Task SynchronizeAsync(
            IReadOnlyList<ProvinceCatalogItem> provinces,
            string source,
            DateTimeOffset synchronizedAt,
            CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
