using Oakton;

namespace PackageUpdater.Abstractions.CommandInput
{
    public class DependencyGraphCommandInput : PathInput
    {
        [Description("Strategy to update package (Net, Netframework, Paket)")]
        [FlagAlias("strategy", 's')]
        public UpdatePackageStrategy StrategyFlag { get; set; } = UpdatePackageStrategy.DotNet;
    }
}