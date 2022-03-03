using Microsoft.Extensions.DependencyInjection;
using PackageUpdater.Abstractions;
using PackageUpdater.DotNet;
using PackageUpdater.DotNetFramework;
using PackageUpdater.Paket;

namespace PackageTool.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddStrategy(this IServiceCollection services, UpdatePackageStrategy strategy)
        {
            switch (strategy)
            {
                case UpdatePackageStrategy.DotNet:
                    services.AddDotNetServices();
                    break;

                case UpdatePackageStrategy.DotNetFramework:
                    services.AddDotNetFrameworkServices();
                    break;

                case UpdatePackageStrategy.Paket:
                    services.AddPaketServices();
                    break;
            }

        }

        public static void AddDotNetServices(this IServiceCollection services)
        {
            services.AddSingleton<IFindDependencyFiles>(new DotNetFindDependencyFiles());
            services.AddSingleton<IFindDependentRepositories>(new DotNetFindDependentRepositories());
            services.AddSingleton<IUpdateDependentRepositories>(new DotNetUpdateDependentRepositories());
        }

        public static void AddDotNetFrameworkServices(this IServiceCollection services)
        {
            services.AddSingleton<IFindDependencyFiles>(new DotNetFrameworkFindDependencyFiles());
            services.AddSingleton<IFindDependentRepositories>(new DotNetFrameworkFindDependentRepositories());
            services.AddSingleton<IUpdateDependentRepositories>(new DotNetFrameworkUpdateDependentRepositories());
        }

        public static void AddPaketServices(this IServiceCollection services)
        {
            services.AddSingleton<IFindDependencyFiles>(new PaketFindDependencyFiles());
            services.AddSingleton<IFindDependentRepositories>(new PaketFindDependentRepositories());
            services.AddSingleton<IUpdateDependentRepositories>(new PaketUpdateDependentRepositories());
        }
    }
}
