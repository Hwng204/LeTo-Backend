using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementations;

public sealed class ProvinceRepository(ApplicationDbContext context) : IProvinceRepository
{
    public async Task<IReadOnlyList<ProvinceOption>> ListActiveAsync(
        CancellationToken cancellationToken)
    {
        var provinces = await context.Provinces
            .AsNoTracking()
            .Where(province => province.IsActive)
            .Select(province => new
            {
                province.Code,
                province.Name,
                ActiveSchoolCount = province.Schools.Count(school => school.Status == "ACTIVE"),
            })
            .ToArrayAsync(cancellationToken);

        return provinces
            .Select(province => new ProvinceOption(
                province.Code,
                province.Name,
                province.ActiveSchoolCount > 0,
                province.ActiveSchoolCount))
            .ToArray();
    }

    public async Task<IReadOnlySet<string>> ListActiveCodesAsync(
        CancellationToken cancellationToken) =>
        (await context.Provinces
            .AsNoTracking()
            .Where(province => province.IsActive)
            .Select(province => province.Code)
            .ToArrayAsync(cancellationToken))
        .ToHashSet(StringComparer.Ordinal);

    public async Task SynchronizeAsync(
        IReadOnlyList<ProvinceCatalogItem> provinces,
        string source,
        DateTimeOffset synchronizedAt,
        CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var existing = await context.Provinces.ToDictionaryAsync(
            province => province.Code,
            StringComparer.Ordinal,
            cancellationToken);
        var incomingCodes = provinces
            .Select(province => province.Code)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var item in provinces)
        {
            if (!existing.TryGetValue(item.Code, out var province))
            {
                province = new Domain.Entities.Organization.Province { Code = item.Code };
                context.Provinces.Add(province);
            }

            province.Name = item.Name;
            province.DivisionType = item.DivisionType;
            province.Source = source;
            province.LastSyncedAt = synchronizedAt;
            province.IsActive = true;
        }

        foreach (var province in existing.Values.Where(
                     province => province.IsActive && !incomingCodes.Contains(province.Code)))
        {
            province.IsActive = false;
            province.Source = source;
            province.LastSyncedAt = synchronizedAt;
        }

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}
