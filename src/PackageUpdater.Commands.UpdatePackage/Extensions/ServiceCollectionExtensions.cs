using Microsoft.Extensions.DependencyInjection;
using PackageUpdater.Abstractions;
using PackageUpdater.Commands.FindRepositories.Extensions;

namespace PackageUpdater.Commands.UpdatePackage.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddUpdatePackageStrategy(this IServiceCollection services, PackageStrategy strategy)
    {
        services.AddFindRepositoriesStrategy(strategy);
        switch (strategy)
        {
            case PackageStrategy.DotNet:
                services.AddUpdatePackageDotNetServices();
                break;
            case PackageStrategy.DotNetFramework:
                services.AddUpdatePackageDotNetFrameworkServices();
                break;
            case PackageStrategy.Paket:
                services.AddUpdatePackagePaketServices();
                break;
        }
    }

    private static void AddUpdatePackageDotNetServices(this IServiceCollection services)
    {
        services.AddSingleton<IUpdateDependentRepositories>(new DotNetUpdateDependentRepositories());
    }

    private static void AddUpdatePackageDotNetFrameworkServices(this IServiceCollection services)
    {
        services.AddSingleton<IUpdateDependentRepositories>(new DotNetFrameworkUpdateDependentRepositories());
    }

    private static void AddUpdatePackagePaketServices(this IServiceCollection services)
    {
        services.AddSingleton<IUpdateDependentRepositories>(new PaketUpdateDependentRepositories());
    }
}
