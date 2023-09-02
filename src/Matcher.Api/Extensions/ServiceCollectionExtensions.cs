using System.Text.Json.Serialization;
using Matcher.Data;
using Matcher.Data.Services;
using Matcher.Data.Repositories;
using Matcher.Business.Services;
using Matcher.Business.Interfaces;
using StackExchange.Redis;

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

        var redisConfigOptions = new ConfigurationOptions
        {
            Password = configurationManager["Redis:Password"],
            EndPoints = { configurationManager["Redis:EndPoint"], },
        };

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfigOptions));

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }

    private static IServiceCollection AddBusinessServices(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services.AddScoped<MatchService>();
        services.AddScoped<ProfileService>();

        return services;
    }

    private static IServiceCollection AddApiServices(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddSwaggerGen();

        return services;
    }
}
