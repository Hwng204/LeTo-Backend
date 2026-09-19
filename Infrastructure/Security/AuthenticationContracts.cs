namespace Infrastructure.Security;

public sealed record LoginRequest(string Username, string Password);

public sealed record AuthenticatedUser(
    ulong UserId,
    string Username,
    IReadOnlyList<string> RoleCodes,
    ulong? BranchId = null);

public interface IUserAuthenticationService
{
    Task<AuthenticatedUser?> AuthenticateAsync(
        LoginRequest request,
        CancellationToken cancellationToken);
}
