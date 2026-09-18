using Application.AcademicYears;
using Domain.Entities.Academic;
using Xunit;

namespace Application.Tests;

public sealed class AcademicYearServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatesDraftYearWithExactlyTwoPlannedTerms()
    {
        var repository = new FakeAcademicYearRepository();
        var service = new AcademicYearService(repository);
        var request = ValidRequest();

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("DRAFT", result.Value.Status);
        Assert.Equal("01-2026-2027", result.Value.Code);
        var year = Assert.Single(repository.AddedYears);
        Assert.Equal("01", year.ProvinceCode);
        Assert.Equal("01-2026-2027", year.Code);
        Assert.Throws<InvalidOperationException>(() => year.AssignCode("01-2027-2028"));
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
        var repository = new FakeAcademicYearRepository();
        var service = new AcademicYearService(repository);
        var request = ValidRequest() with { Name = "2026/2027" };

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("VALIDATION_ERROR", result.Error?.Code);
        Assert.Contains("name", result.Error?.Details?.Keys ?? []);
        Assert.Equal(0, repository.CreateCallCount);
    }

    [Fact]
    public async Task CreateAsync_RejectsAnUnknownProvince()
    {
        var repository = new FakeAcademicYearRepository
        {
            CreateOutcome = AcademicYearCreateOutcome.ProvinceNotFound
        };
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
            CreateOutcome = AcademicYearCreateOutcome.Conflict
        };
        var service = new AcademicYearService(repository);

        var result = await service.CreateAsync(ValidRequest(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("ACADEMIC_YEAR_CONFLICT", result.Error?.Code);
        Assert.Empty(repository.AddedYears);
    }

    [Fact]
    public async Task CreateAsync_ReturnsConflictWhenTheAtomicCreateLosesAConcurrentRace()
    {
        var repository = new FakeAcademicYearRepository
        {
            CreateOutcome = AcademicYearCreateOutcome.Conflict
        };
        var service = new AcademicYearService(repository);

        var result = await service.CreateAsync(ValidRequest(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("ACADEMIC_YEAR_CONFLICT", result.Error?.Code);
    }

    private static CreateAcademicYearRequest ValidRequest() =>
        new("01", "2026-2027", new DateOnly(2026, 8, 15), new DateOnly(2027, 5, 31));

    private sealed class FakeAcademicYearRepository : IAcademicYearRepository
    {
        public AcademicYearCreateOutcome CreateOutcome { get; init; } =
            AcademicYearCreateOutcome.Created;
        public int CreateCallCount { get; private set; }
        public List<AcademicYear> AddedYears { get; } = [];

        public Task<AcademicYearCreateOutcome> TryAddAsync(
            AcademicYear academicYear,
            CancellationToken cancellationToken)
        {
            CreateCallCount++;
            if (CreateOutcome == AcademicYearCreateOutcome.Created)
            {
                AddedYears.Add(academicYear);
                academicYear.Id = 10;
            }

            return Task.FromResult(CreateOutcome);
        }

        public Task<(IReadOnlyList<AcademicYear> Items, int TotalCount)> ListAsync(
            string provinceCode,
            int page,
            int pageSize,
            CancellationToken cancellationToken) =>
            Task.FromResult<(IReadOnlyList<AcademicYear>, int)>(([], 0));
    }
}
