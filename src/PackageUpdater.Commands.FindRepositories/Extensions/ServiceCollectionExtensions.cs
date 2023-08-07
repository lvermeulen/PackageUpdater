using Microsoft.Extensions.DependencyInjection;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.FindRepositories.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddFindRepositoriesStrategy(this IServiceCollection services, PackageStrategy strategy)
    {
        switch (strategy)
        {
            case PackageStrategy.DotNet:
                services.AddFindRepositoriesDotNetServices();
                break;
            case PackageStrategy.DotNetFramework:
                services.AddFindRepositoriesDotNetFrameworkServices();
                break;
            case PackageStrategy.Paket:
                services.AddFindRepositoriesPaketServices();
                break;
        }
    }

    private static void AddFindRepositoriesDotNetServices(this IServiceCollection services)
    {
        services.AddSingleton<IFindDependencyFiles>(new DotNetFindDependencyFiles());
        services.AddSingleton<IFindDependentRepositories>(new DotNetFindDependentRepositories());
    }

    private static void AddFindRepositoriesDotNetFrameworkServices(this IServiceCollection services)
    {
        services.AddSingleton<IFindDependencyFiles>(new DotNetFrameworkFindDependencyFiles());
        services.AddSingleton<IFindDependentRepositories>(new DotNetFrameworkFindDependentRepositories());
    }

    private static void AddFindRepositoriesPaketServices(this IServiceCollection services)
    {
        services.AddSingleton<IFindDependencyFiles>(new PaketFindDependencyFiles());
        services.AddSingleton<IFindDependentRepositories>(new PaketFindDependentRepositories());
    }
}
