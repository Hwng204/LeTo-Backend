using Application.AcademicYears;
using Domain.Entities.Academic;
using Xunit;

namespace Application.Tests;

public sealed class AcademicYearServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatesDraftYearWithExactlyTwoPlannedTerms()
    {
        var repository = new FakeAcademicYearRepository { ProvinceExists = true };
        var service = new AcademicYearService(repository);
        var request = ValidRequest();

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("DRAFT", result.Value.Status);
        var year = Assert.Single(repository.AddedYears);
        Assert.Equal("01", year.ProvinceCode);
        Assert.Collection(
            year.Semesters.OrderBy(term => term.Order),
            first =>
            {
                Assert.Equal((byte)1, first.Order);
                Assert.Equal("Học kỳ 1", first.Name);
                Assert.Equal("PLANNED", first.Status);
                Assert.Null(first.StartDate);
                Assert.Null(first.EndDate);
            },
            second =>
            {
                Assert.Equal((byte)2, second.Order);
                Assert.Equal("Học kỳ 2", second.Name);
                Assert.Equal("PLANNED", second.Status);
                Assert.Null(second.StartDate);
                Assert.Null(second.EndDate);
            });
    }

    [Fact]
    public async Task CreateAsync_ReturnsValidationDetailsBeforeUsingTheRepository()
    {
        var repository = new FakeAcademicYearRepository { ProvinceExists = true };
        var service = new AcademicYearService(repository);
        var request = ValidRequest() with { Name = "2026/2027" };

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("VALIDATION_ERROR", result.Error?.Code);
        Assert.Contains("name", result.Error?.Details?.Keys ?? []);
        Assert.Equal(0, repository.ProvinceLookupCount);
    }

    [Fact]
    public async Task CreateAsync_RejectsAnUnknownProvince()
    {
        var repository = new FakeAcademicYearRepository { ProvinceExists = false };
        var service = new AcademicYearService(repository);

        var result = await service.CreateAsync(ValidRequest(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("PROVINCE_NOT_FOUND", result.Error?.Code);
        Assert.Empty(repository.AddedYears);
    }

    [Fact]
    public async Task CreateAsync_RejectsAnOverlappingOrDuplicateYear()
    {
        var repository = new FakeAcademicYearRepository
        {
            ProvinceExists = true,
            HasConflict = true
        };
        var service = new AcademicYearService(repository);

        var result = await service.CreateAsync(ValidRequest(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("ACADEMIC_YEAR_CONFLICT", result.Error?.Code);
        Assert.Empty(repository.AddedYears);
    }

    private static CreateAcademicYearRequest ValidRequest() =>
        new("01", "2026-2027", new DateOnly(2026, 8, 15), new DateOnly(2027, 5, 31));

    private sealed class FakeAcademicYearRepository : IAcademicYearRepository
    {
        public bool ProvinceExists { get; init; }
        public bool HasConflict { get; init; }
        public int ProvinceLookupCount { get; private set; }
        public List<AcademicYear> AddedYears { get; } = [];

        public Task<bool> ProvinceExistsAsync(string provinceCode, CancellationToken cancellationToken)
        {
            ProvinceLookupCount++;
            return Task.FromResult(ProvinceExists);
        }

        public Task<bool> HasConflictAsync(
            string provinceCode,
            string name,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken) => Task.FromResult(HasConflict);

        public Task AddAsync(AcademicYear academicYear, CancellationToken cancellationToken)
        {
            AddedYears.Add(academicYear);
            academicYear.Id = 10;
            return Task.CompletedTask;
        }

        public Task<(IReadOnlyList<AcademicYear> Items, int TotalCount)> ListAsync(
            string provinceCode,
            int page,
            int pageSize,
            CancellationToken cancellationToken) =>
            Task.FromResult<(IReadOnlyList<AcademicYear>, int)>(([], 0));
    }
}
