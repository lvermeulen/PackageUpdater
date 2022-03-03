using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Oakton;
using PackageTool.Extensions;
using PackageUpdater.Abstractions;
using PackageUpdater.Abstractions.CommandInput;

namespace PackageTool.Commands
{
    [Description("Find dependent repositories", Name = "find-repositories")]
    public class FindRepositoriesCommand : OaktonAsyncCommand<FindRepositoriesCommandInput>
    {
        public FindRepositoriesCommand()
        {
            Usage("Find dependent repositories")
                .ValidFlags(
                    x => x.PackageNameFlag, 
                    x => x.PathFlag,
                    x => x.StrategyFlag
                );
        }

        public override async Task<bool> Execute(FindRepositoriesCommandInput input)
        {
            using var host = input.BuildHost();

            var services = new ServiceCollection();
            services.AddStrategy(input.StrategyFlag);
            var serviceProvider = services.BuildServiceProvider();

            var finder = serviceProvider.GetRequiredService<IFindDependentRepositories>();
            await finder.FindDependentRepositories(input, x => Console.WriteLine($"* {x.Path}"));

            return true;
        }
    }
}
