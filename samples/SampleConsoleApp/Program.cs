using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PackageUpdater.Abstractions;
using PackageUpdater.Abstractions.CommandInput;
using PackageUpdater.Paket;

namespace SampleConsoleApp
{
    public static class Program
    {
        private static UpdatePackageCommandInput ParseArgs(string[] args) => args.Length != 3
            ? null
            : new UpdatePackageCommandInput
            {
                PackageNameFlag = args[0],
                PackageVersionFlag = args[1],
                PathFlag = args[2],
                WhatIfFlag = true
            };

        public static void DisplayUsage()
        {
            Console.WriteLine("Usage: SampleConsoleApp [package_name] [package_version] [file_path]");
        }

        public static async Task Main(string[] args)
        {
            var input = ParseArgs(args);
            if (input is null)
            {
                DisplayUsage();
                return;
            }

            var services = new ServiceCollection();
            services.AddSingleton<IFindDependencyFiles>(new PaketFindDependencyFiles());
            services.AddSingleton<IFindDependentRepositories>(new PaketFindDependentRepositories());
            services.AddSingleton<IUpdateDependentRepositories>(new PaketUpdateDependentRepositories());
            var serviceProvider = services.BuildServiceProvider();

            var updater = new Updater(serviceProvider);
            await updater.UpdatePackage(input, x => x.DependenciesFiles.ToList().ForEach(Console.WriteLine));
        }
    }
}
