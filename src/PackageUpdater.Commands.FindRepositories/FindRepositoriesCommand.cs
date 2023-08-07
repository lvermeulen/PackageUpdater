using System.CommandLine;
using System.CommandLine.IO;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PackageUpdater.Abstractions;
using PackageUpdater.Commands.FindRepositories.Extensions;

namespace PackageUpdater.Commands.FindRepositories;

public class FindRepositoriesCommand : Command
{
    private readonly IConfiguration? _configuration;
    private const string CommandName = "find-repositories";
    private const string CommandDescription = "find dependent repositories";

    public FindRepositoriesCommand()
        : base(CommandName, CommandDescription)
    {
        //_configuration = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json", true, false)
        //    .Build();

        var packageName = new Option<string>(new[] { "--packageName" }, "Package name.");
        var strategy = new Option<PackageStrategy>(new[] { "--strategy" }, () => PackageStrategy.DotNet, "Package strategy.");
        var path = new Option<string>(new[] { "--path" }, Path.GetTempPath, "Local file path.");
        AddOption(packageName);
        AddOption(strategy);
        AddOption(path);
        this.SetHandler(Handle, packageName, strategy, path);
    }

    public FindRepositoriesCommand(IConfiguration configuration)
        : this()
    {
        _configuration = configuration;
    }
        
    private async Task Handle(string packageName, PackageStrategy strategy, string path)
    {
        // get nuget.exe path
        var nugetExePath = _configuration?["NugetExePath"] ?? string.Empty;

        var input = new FindRepositoriesCommandInput(packageName, strategy, path);
        var services = new ServiceCollection();
        services.AddFindRepositoriesStrategy(input.Strategy);
        var serviceProvider = services.BuildServiceProvider();

        IConsole console = new SystemConsole();
        var finder = serviceProvider.GetRequiredService<IFindDependentRepositories>();
        await finder.FindDependentRepositories(input, x => console.WriteLine($"* {x.Path}"));

        console.WriteLine($"Successfully found dependent repositories for package name {packageName}.");
    }
}
