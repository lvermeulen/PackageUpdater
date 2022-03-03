using System;
using Oakton;

namespace PackageUpdater.Abstractions.CommandInput
{
    public class FindRepositoriesCommandInput : NetCoreInput
    {
        [Description("Package name")]
        [FlagAlias("name", 'n')]
        public string PackageNameFlag { get; set; }

        [Description("Path (parent to all your repository folders)")]
        [FlagAlias("path", 'p')]
        public string PathFlag { get; set; } = Environment.CurrentDirectory;

        [Description("Strategy to update package (Net, Netframework, Paket)")]
        [FlagAlias("strategy", 's')]
        public UpdatePackageStrategy StrategyFlag { get; set; } = UpdatePackageStrategy.DotNet;

        public FindRepositoriesCommandInput With(Action<FindRepositoriesCommandInput> action)
        {
            var result = new FindRepositoriesCommandInput
            {
                PackageNameFlag = PackageNameFlag,
                PathFlag = PathFlag,
                StrategyFlag = StrategyFlag
            };
            action?.Invoke(result);

            return result;
        }
    }
}