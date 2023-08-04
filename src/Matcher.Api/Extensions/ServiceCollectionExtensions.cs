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
        // data services

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
