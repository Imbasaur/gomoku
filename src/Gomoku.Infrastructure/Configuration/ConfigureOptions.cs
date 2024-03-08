using Gomoku.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gomoku.Infrastructure.Configuration;
public static class ConfigureOptions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions();
        services.Configure<ConnectionStringsConfig>(config.GetSection("ConnectionStrings"));

        return services;
    }
}
