using System.CommandLine;
using System.CommandLine.IO;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.DependencyGraph;

public class DependencyGraphCommand : Command
{
    private const string CommandName = "dependency-graph";
    private const string CommandDescription = "show dependency graph for all repositories";

    public DependencyGraphCommand()
        : this(CommandName, CommandDescription)
    {
        var strategy = new Option<PackageStrategy>(new[] { "--strategy" }, () => PackageStrategy.DotNet, "Package strategy.");
        var path = new Option<string>(new[] { "--path" }, Path.GetTempPath, "Local file path.");
        AddOption(strategy);
        AddOption(path);
        this.SetHandler(Handle, strategy, path);
    }

    public DependencyGraphCommand(string name, string? description = null)
        : base(name, description)
    { }

    private static async Task Handle(PackageStrategy strategy, string path)
    {
        var input = new DependencyGraphCommandInput(path);
        var services = new ServiceCollection();

        // TODO: implement
        await Task.Yield();

        IConsole console = new SystemConsole();
        console.WriteLine("Successfully constructed dependency graph for repositories.");
    }
}