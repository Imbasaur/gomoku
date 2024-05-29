using Gomoku.Core.Profiles;
using Gomoku.Core.Services;
using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Gomoku.Infrastructure.Configuration;
public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<GameTimeoutManager>();

        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IWaitingListRepository, WaitingListRepository>();

        services.Scan(scan => scan
            .FromAssemblyOf<IGameService>()
            .AddClasses()
            .AsMatchingInterface()
            .WithTransientLifetime());

        services.AddAutoMapper(Assembly.GetAssembly(typeof(GameProfiles)));

        return services;
    }
}
