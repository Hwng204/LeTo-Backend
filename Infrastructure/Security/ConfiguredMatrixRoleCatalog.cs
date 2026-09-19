using Microsoft.Extensions.Configuration;

namespace Infrastructure.Security;

public sealed class ConfiguredMatrixRoleCatalog : IMatrixRoleCatalog
{
    public ConfiguredMatrixRoleCatalog(IConfiguration configuration)
    {
        PhtRoleCodes = ReadCodes(
            configuration,
            "MatrixAuth:PhtRoleCodes",
            "PHT");
        PrincipalRoleCodes = ReadCodes(
            configuration,
            "MatrixAuth:PrincipalRoleCodes",
            "HIEU_TRUONG",
            "PRINCIPAL");
        TeamLeadRoleCodes = ReadCodes(
            configuration,
            "MatrixAuth:TeamLeadRoleCodes",
            "TEAM_LEAD",
            "TO_TRUONG");
    }

    public IReadOnlyCollection<string> PhtRoleCodes { get; }
    public IReadOnlyCollection<string> TeamLeadRoleCodes { get; }
    public IReadOnlyCollection<string> PrincipalRoleCodes { get; }

    public bool IsPht(string roleCode)
    {
        return PhtRoleCodes.Contains(Normalize(roleCode), StringComparer.OrdinalIgnoreCase);
    }

    public bool IsPrincipal(string roleCode)
    {
        return PrincipalRoleCodes.Contains(Normalize(roleCode), StringComparer.OrdinalIgnoreCase);
    }

    public bool IsTeamLead(string roleCode)
    {
        return TeamLeadRoleCodes.Contains(Normalize(roleCode), StringComparer.OrdinalIgnoreCase);
    }

    private static IReadOnlyCollection<string> ReadCodes(
        IConfiguration configuration,
        string key,
        params string[] defaults)
    {
        var configured = configuration
            .GetSection(key)
            .GetChildren()
            .Select(item => Normalize(item.Value))
            .Where(item => item.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return configured.Length > 0
            ? configured
            : defaults.Select(Normalize).ToArray();
    }

    private static string Normalize(string? value)
    {
        return (value ?? string.Empty).Trim().ToUpperInvariant();
    }
}
