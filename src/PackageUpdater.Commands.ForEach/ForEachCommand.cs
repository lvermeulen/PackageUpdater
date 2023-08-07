using System.CommandLine;
using System.CommandLine.IO;
using System.IO;
using System.Threading.Tasks;

namespace PackageUpdater.Commands.ForEach;

public class ForEachCommand : Command
{
    private const string CommandName = "for-each";
    private const string CommandDescription = "execute a command for each found repository";

    public ForEachCommand()
        : this(CommandName, CommandDescription)
    {
        var url = new Option<string>(new[] { "--url" }, () => "", "URL.")
        {
            IsRequired = false
        };
        var action = new Option<string>(new[] { "--action" }, "Action on each repository.");
        var path = new Option<string>(new[] { "--path" }, Path.GetTempPath, "Local file path.");
        AddOption(url);
        AddOption(action);
        AddOption(path);
        this.SetHandler(Handle, url, action, path);
    }

    public ForEachCommand(string name, string? description = null)
        : base(name, description)
    { }

    private static async Task Handle(string url, string action, string path)
    {
        var iterator = new Iterator();
        await iterator.Iterate(new ForEachCommandInput(url, action, path));

        IConsole console = new SystemConsole();
        console.WriteLine($"Successfully executed action {action} for repositories at {url}.");
    }
}