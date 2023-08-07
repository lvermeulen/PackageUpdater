using System;
using System.CommandLine;
using System.CommandLine.IO;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PackageUpdater.Abstractions;
using PackageUpdater.Commands.UpdatePackage.Extensions;

namespace PackageUpdater.Commands.UpdatePackage;

public class UpdatePackageCommand : Command
{
    private readonly IConfiguration _configuration;
    private const string CommandName = "update-package";
    private const string CommandDescription = "update package for all repositories";

    public UpdatePackageCommand()
        : base(CommandName, CommandDescription)
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, false)
            .Build();

        var packageVersion = new Option<string>(new[] { "--packageVersion" }, "Package version to update to.");
        var packageName = new Option<string>(new[] { "--packageName" }, "Package name.");
        var strategy = new Option<PackageStrategy>(new[] { "--strategy" }, () => PackageStrategy.DotNet, "Package strategy.");
        var path = new Option<string>(new[] { "--path" }, Path.GetTempPath, "Local file path.");
        var whatIf = new Option<bool>("--whatIf", () => true, "Shows what would happen if this command were to be executed.");
        AddOption(packageVersion);
        AddOption(packageName);
        AddOption(strategy);
        AddOption(path);
        AddOption(whatIf);
        this.SetHandler(Handle, packageVersion, packageName, strategy, path, whatIf);
    }

    public UpdatePackageCommand(IConfiguration configuration)
        : this()
    {
        _configuration = configuration;
    }

    private async Task Handle(string packageVersion, string packageName, PackageStrategy strategy, string path, bool whatIf)
    {
        // get nuget.exe path
        var nugetExePath = _configuration["NugetExePath"] ?? string.Empty;

        var input = new UpdatePackageCommandInput(packageVersion, packageName, strategy, path, whatIf);

        var processRunner = new ProcessRunner();
        Action<string> afterUpdateRepository = input.Strategy switch
        {
            PackageStrategy.DotNet => x => processRunner.RunProcessAsync("dotnet", $"add {x} package {input.PackageName} --version {input.PackageVersion}", input.Path).GetAwaiter().GetResult(),
            PackageStrategy.DotNetFramework => _ => processRunner.RunProcessAsync(nugetExePath, $"install {input.PackageName} -Version {input.PackageVersion}", input.Path).GetAwaiter().GetResult(),
            PackageStrategy.Paket => _ => processRunner.RunProcessAsync("dotnet", "paket install", input.Path).GetAwaiter().GetResult(),
            _ => throw new InvalidOperationException()
        };

        var services = new ServiceCollection();
        services.AddUpdatePackageStrategy(input.Strategy);
        var updater = new Updater(services.BuildServiceProvider());

        IConsole console = new SystemConsole();
        await updater.UpdatePackage(input, x => console.WriteLine($"* {x.Path}"), afterUpdateRepository);

        console.WriteLine($"Successfully update package {input.PackageName} to version {input.PackageVersion} for repositories.");
    }

}