using System;
using Oakton;

namespace PackageUpdater.Abstractions.CommandInput
{
    public class FindRepositoriesCommandInput : DependencyGraphCommandInput
    {
        [Description("Package name")]
        [FlagAlias("name", 'n')]
        public string PackageNameFlag { get; set; }

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