using Application.Common.Security;
using Application.Interfaces;
using Application.Services.Implement;
using Application.Services.Interface;
using Infrastructure.Context;
using Infrastructure.Exports;
using Infrastructure.Repositories.Implement;
using Infrastructure.Security;
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

        services.AddScoped<IMatrixRepository, ExamMatrixRepository>();
        services.AddScoped<IMatrixTaskReader, MatrixTaskReader>();
        services.AddScoped<IMatrixReferenceReader, MatrixReferenceReader>();
        services.AddScoped<IMatrixTransaction, MatrixTransaction>();
        services.AddScoped<IMatrixApplicationService, MatrixApplicationService>();
        services.AddScoped<IMatrixTaskApplicationService, MatrixTaskApplicationService>();
        services.AddScoped<IMatrixTaskRepository, MatrixTaskRepository>();
        services.AddScoped<IMatrixTaskReferenceReader, MatrixTaskReferenceReader>();
        services.AddSingleton<IMatrixRoleCatalog, ConfiguredMatrixRoleCatalog>();
        services.AddSingleton<IMatrixWorkbookExporter, ClosedXmlMatrixWorkbookExporter>();

        return services;
    }
}
