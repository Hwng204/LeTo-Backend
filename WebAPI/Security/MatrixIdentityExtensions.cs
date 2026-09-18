using System.Security.Claims;
using System.Text.Encodings.Web;
using Application.Common.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace WebAPI.Security;

public static class MatrixClaims
{
    // Claim that carries the branch a PHT belongs to. Whatever issues the token must set it.
    public const string BranchId = "branch_id";
}

public static class MatrixIdentityExtensions
{
    // Wires the matrix feature to "who is calling" without owning login.
    // Login/JWT belongs to the auth workstream: its token needs NameIdentifier, Role
    // (HIEU_TRUONG / PHT / TEAM_LEAD) and branch_id claims.
    // Development only: set Dev:UserId, Dev:Role, Dev:BranchId to skip login.
    public static IServiceCollection AddMatrixIdentity(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IMatrixCurrentUser, HttpMatrixCurrentUser>();
        services.AddAuthorization();

        if (environment.IsDevelopment() &&
            !string.IsNullOrWhiteSpace(configuration["Dev:UserId"]))
        {
            services
                .AddAuthentication(DevAuthenticationHandler.Scheme)
                .AddScheme<AuthenticationSchemeOptions, DevAuthenticationHandler>(
                    DevAuthenticationHandler.Scheme,
                    _ => { });
        }

        return services;
    }
}

// Development stub: every request is authenticated as the user in Dev:* config.
// Headers X-Dev-UserId / X-Dev-Role / X-Dev-BranchId override it per request.
public sealed class DevAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration configuration)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public new const string Scheme = "DevIdentity";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var userId = Pick("X-Dev-UserId", "Dev:UserId");
        var role = Pick("X-Dev-Role", "Dev:Role");
        var branchId = Pick("X-Dev-BranchId", "Dev:BranchId");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Role, role)
        };
        if (!string.IsNullOrWhiteSpace(branchId))
        {
            claims.Add(new Claim(MatrixClaims.BranchId, branchId));
        }

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme));
        return Task.FromResult(
            AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme)));
    }

    private string Pick(string header, string key) =>
        Request.Headers.TryGetValue(header, out var value) && !string.IsNullOrWhiteSpace(value)
            ? value.ToString()
            : configuration[key] ?? string.Empty;
}
