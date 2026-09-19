using Domain.Entities.Identity;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Security;

public sealed class UserAuthenticationService(
    ApplicationDbContext db,
    IPasswordHasher<User> passwordHasher) : IUserAuthenticationService
{
    public async Task<AuthenticatedUser?> AuthenticateAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null ||
            string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }

        var username = request.Username.Trim();
        var user = await db.Users
            .AsNoTracking()
            .Include(item => item.UserRoles)
                .ThenInclude(item => item.Role)
            .SingleOrDefaultAsync(
                item => item.Username == username && item.Status == "ACTIVE",
                cancellationToken);

        if (user is null)
        {
            return null;
        }

        var passwordResult = passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);
        if (passwordResult is PasswordVerificationResult.Failed)
        {
            return null;
        }

        var roleCodes = user.UserRoles
            .Select(item => item.Role.Code.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return new AuthenticatedUser(user.Id, user.Username, roleCodes, user.SchoolBranchId);
    }
}
