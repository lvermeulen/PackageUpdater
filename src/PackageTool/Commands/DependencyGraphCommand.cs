using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Oakton;
using PackageUpdater.Abstractions.CommandInput;
using PackageTool.Extensions;

namespace PackageTool.Commands
{
    public class DependencyGraphCommand : OaktonAsyncCommand<DependencyGraphCommandInput>
    {
        public DependencyGraphCommand()
        {
            Usage("Show dependency graph for all repositories")
                .ValidFlags(
                    x => x.PathFlag,
                    x => x.StrategyFlag
                );
        }

        public override Task<bool> Execute(DependencyGraphCommandInput input)
        {
            using var host = input.BuildHost();

            //var configuration = host.Services.GetRequiredService<IConfiguration>();
            
            var services = new ServiceCollection();
            services.AddStrategy(input.StrategyFlag);

            return Task.FromResult(true);
        }
    }
}
