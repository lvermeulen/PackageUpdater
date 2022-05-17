using Oakton;

namespace PackageUpdater.Abstractions.CommandInput
{
    public class ForEachCommandInput : PathInput
    {
        [Description("URL")]
        [FlagAlias("url", 'u')]
        public string UrlFlag { get; set; }

        [Description("Action")]
        [FlagAlias("action", 'a')]
        public string ActionFlag { get; set; }
    }
}
