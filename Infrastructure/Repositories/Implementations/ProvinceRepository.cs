using Application.Provinces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementations;

public sealed class ProvinceRepository(ApplicationDbContext context) : IProvinceRepository
{
    public async Task<IReadOnlyList<ProvinceOption>> ListActiveAsync(
        CancellationToken cancellationToken) =>
        await context.Provinces
            .AsNoTracking()
            .Where(province => province.IsActive)
            .Select(province => new ProvinceOption(
                province.Code,
                province.Name,
                province.Schools.Any(school => school.Status == "ACTIVE")))
            .ToArrayAsync(cancellationToken);
}
