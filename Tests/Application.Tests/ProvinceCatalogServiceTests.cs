using Application.Provinces;
using Xunit;

namespace Application.Tests;

public sealed class ProvinceCatalogServiceTests
{
    [Fact]
    public async Task ListAsync_PrioritizesProvincesWithSchoolsThenSortsByName()
    {
        var repository = new FakeProvinceRepository(
        [
            new ProvinceOption("79", "Thành phố Hồ Chí Minh", false),
            new ProvinceOption("48", "Đà Nẵng", true),
            new ProvinceOption("01", "Hà Nội", true)
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
    }
}
