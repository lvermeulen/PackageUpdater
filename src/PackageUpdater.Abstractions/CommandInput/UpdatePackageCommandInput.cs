using Oakton;

namespace PackageUpdater.Abstractions.CommandInput
{
    public class UpdatePackageCommandInput : FindRepositoriesCommandInput
    {
        [Description("Package version")]
        [FlagAlias("version", 'v')]
        public string PackageVersionFlag { get; set; }

        [Description("Show what would happen if you were to execute this command")]
        [FlagAlias("whatif", 'w')]
        public bool WhatIfFlag { get; set; } = false;
    }
}