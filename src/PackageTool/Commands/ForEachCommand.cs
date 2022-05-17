using System.Threading.Tasks;
using Oakton;
using PackageUpdater.Abstractions;
using PackageUpdater.Abstractions.CommandInput;

namespace PackageTool.Commands
{
    [Description("Execute a command for each repository found", Name = "for-each")]
    public class ForEachCommand : OaktonAsyncCommand<ForEachCommandInput>
    {
        public ForEachCommand()
        {
            Usage("For each repository found")
                .ValidFlags(
                    x => x.PathFlag,
                    x => x.ActionFlag
                );
        }

        public override async Task<bool> Execute(ForEachCommandInput input)
        {
            var iterator = new Iterator();
            await iterator.Iterate(input);

            return true;
        }
    }
}
