namespace Infrastructure.Security;

public interface IMatrixRoleCatalog
{
    IReadOnlyCollection<string> PhtRoleCodes { get; }
    IReadOnlyCollection<string> TeamLeadRoleCodes { get; }
    IReadOnlyCollection<string> PrincipalRoleCodes { get; }
    bool IsPht(string roleCode);
    bool IsTeamLead(string roleCode);
    bool IsPrincipal(string roleCode);
}
