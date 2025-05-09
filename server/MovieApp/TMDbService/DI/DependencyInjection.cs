using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TMDbService.Config;
using TMDbService.Services;

namespace TMDbService.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddTMDbClient(this IServiceCollection services, IConfiguration configuration)
    {
        configuration.LoadConfigSectionFromConfiguration<TMDbSettings>();
        services.ConfigureWithConfigSection<TMDbSettings>(configuration);

        services.AddTransient<TMDbApiKeyHandler>();

        services.AddHttpClient<ITMDBQueryService, TMDBQueryService>()
            .AddHttpMessageHandler<TMDbApiKeyHandler>()
            .ConfigureHttpClient((sp, client) =>
            {
                var config = sp.GetRequiredService<IOptions<TMDbSettings>>().Value;
                client.BaseAddress = new Uri(config.BaseUrl);
            });

        return services;
    }

    private static void ConfigureWithConfigSection<T>(this IServiceCollection services,
        IConfiguration config) where T : class =>
        services.Configure<T>(config.GetSection(typeof(T).Name));

    private static void LoadConfigSectionFromConfiguration<T>(this IConfiguration config) =>
        config.GetSection(typeof(T).Name).Get<T>();
}