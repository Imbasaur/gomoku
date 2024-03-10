using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Gomoku.Infrastructure.Configuration;
public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IGameRepository, GameRepository>();

        services.Scan(scan => scan
            .FromAssemblyOf<IGameService>()
                .AddClasses(classes => classes.AssignableTo<IGameService>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        return services;
    }
}
