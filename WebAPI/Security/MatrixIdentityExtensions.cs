using Application.Common.Security;

namespace WebAPI.Security;

public static class MatrixClaims
{
    // Claim that carries the branch a PHT belongs to. The access token must set it.
    public const string BranchId = "branch_id";
}

public static class MatrixIdentityExtensions
{
    // Lets the matrix feature know who is calling, from the validated access token
    // (NameIdentifier, Role and branch_id claims).
    public static IServiceCollection AddMatrixIdentity(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IMatrixCurrentUser, HttpMatrixCurrentUser>();
        services.AddAuthorization();

        return services;
    }
}
