using System.CommandLine;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PackageTool.Extensions;
using PackageUpdater.Commands.DependencyGraph;
using PackageUpdater.Commands.FindRepositories;
using PackageUpdater.Commands.ForEach;
using PackageUpdater.Commands.UpdatePackage;

namespace PackageTool;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Perform operations on multiple repositories.");
        rootCommand.AddCommandsFromAssemblyOf<DependencyGraphCommand>();
        rootCommand.AddCommandsFromAssemblyOf<FindRepositoriesCommand>();
        rootCommand.AddCommandsFromAssemblyOf<ForEachCommand>();
        rootCommand.AddCommandsFromAssemblyOf<UpdatePackageCommand>();

        if (args.Length == 0)
        {
            args = new [] { "--help" };
        }

        return await rootCommand.InvokeAsync(args);
    }

    public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((_, services) => services.AddJsonFile("appsettings.json"));
}