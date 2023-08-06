using Matcher.Data;
using Matcher.Data.Repositories;
using Matcher.Business.Interfaces;

namespace Matcher.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        return services
            .AddDataServices(configurationManager)
            .AddBusinessServices(configurationManager)
            .AddApiServices(configurationManager);
    }

    private static IServiceCollection AddDataServices(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        var postgresHost = configurationManager["Postgres:Host"];
        var postgresPort = configurationManager["Postgres:Port"];
        var postgresDatabase = configurationManager["Postgres:Database"];
        var postgresUser = configurationManager["Postgres:User"];
        var postgresPassword = configurationManager["Postgres:Password"];

        var connectionString =
            $"Host={postgresHost};Port={postgresPort};Database={postgresDatabase};Username={postgresUser};Password={postgresPassword}";

        services.AddNpgsql<ApplicationContext>(connectionString);

        services.AddScoped<IProfileRepository, EfCoreProfileRepository>();

        return services;
    }

    private static IServiceCollection AddBusinessServices(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        // business services

        return services;
    }

    private static IServiceCollection AddApiServices(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services.AddControllers();
        services.AddSwaggerGen();

        return services;
    }
}
