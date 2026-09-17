using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:DefaultConnection"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "ConnectionStrings:DefaultConnection must be configured before registering Infrastructure.");
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 0)),
                mysql => mysql.MigrationsAssembly(
                    typeof(ApplicationDbContext).Assembly.GetName().Name)));

        return services;
    }
}
