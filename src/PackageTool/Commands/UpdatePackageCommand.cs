using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oakton;
using PackageTool.Extensions;
using PackageUpdater.Abstractions;
using PackageUpdater.Abstractions.CommandInput;

namespace PackageTool.Commands
{
    [Description("Update package for all repositories", Name = "update-package")]
    public class UpdatePackageCommand : OaktonAsyncCommand<UpdatePackageCommandInput>
    {
        public UpdatePackageCommand()
        {
            Usage("Update package for all repositories")
                .ValidFlags(
                    x => x.PackageNameFlag, 
                    x => x.PackageVersionFlag, 
                    x => x.PathFlag,
                    x => x.StrategyFlag,
                    x => x.WhatIfFlag
                );
        }

        public override async Task<bool> Execute(UpdatePackageCommandInput input)
        {
            using var host = input.BuildHost();

            // get nuget.exe path
            var configuration = host.Services.GetRequiredService<IConfiguration>();
            string nugetExePath = configuration["NugetExePath"];
            
            var processRunner = new ProcessRunner();
            Action<string> afterUpdateRepository = input.StrategyFlag switch
            {
                UpdatePackageStrategy.DotNet => x => processRunner.RunProcessAsync("dotnet", $"add {x} package {input.PackageNameFlag} --version {input.PackageVersionFlag}", input.PathFlag).GetAwaiter().GetResult(),
                UpdatePackageStrategy.DotNetFramework => x => processRunner.RunProcessAsync(nugetExePath, $"install {input.PackageNameFlag} -Version {input.PackageVersionFlag}", input.PathFlag).GetAwaiter().GetResult(),
                UpdatePackageStrategy.Paket => x => processRunner.RunProcessAsync("dotnet", "paket install", input.PathFlag).GetAwaiter().GetResult(),
                var _ => throw new InvalidOperationException()
            };

            var services = new ServiceCollection();
            services.AddStrategy(input.StrategyFlag);

            var updater = new Updater(services.BuildServiceProvider());
            await updater.UpdatePackage(input, x => Console.WriteLine($"* {x.Path}"), afterUpdateRepository);

            return true;
        }
    }
}
