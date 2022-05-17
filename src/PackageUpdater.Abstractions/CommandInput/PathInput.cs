using System;
using Oakton;

namespace PackageUpdater.Abstractions.CommandInput
{
    public class PathInput : NetCoreInput
    {
        [Description("Path (parent to all your repository folders)")]
        [FlagAlias("path", 'p')]
        public string PathFlag { get; set; } = Environment.CurrentDirectory;
    }
}
