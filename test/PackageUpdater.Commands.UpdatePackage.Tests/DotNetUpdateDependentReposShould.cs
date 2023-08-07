using System.Linq;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;
using PackageUpdater.Commands.FindRepositories;
using Xunit;
using Xunit.Abstractions;

namespace PackageUpdater.Commands.UpdatePackage.Tests;

public class DotNetUpdateDependentReposShould
{
    private readonly ITestOutputHelper _testOutputHelper;

    public DotNetUpdateDependentReposShould(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task UpdateDependentRepositories()
    {
        const string packageName = "Serilog";
        const string packageVersion = "3.3.0";
        const string path = @"C:\wgkrepo\WgkOvl.Logging.Serilog";

        var input = new UpdatePackageCommandInput(packageVersion, packageName, PackageStrategy.DotNet, path, WhatIf: true);
        var finder = new DotNetFindDependentRepositories();
        var results = await finder.FindDependentRepositories(input);

        var updater = new DotNetUpdateDependentRepositories();
        var updatedRepositories = (await updater.UpdateDependentRepositories(input, results)).ToList();

        if (updatedRepositories.Any())
        {
            _testOutputHelper.WriteLine("The following files are updated:");
            foreach (var updatedRepository in updatedRepositories)
            {
                foreach (var updatedRepositoryDependenciesFile in updatedRepository.DependenciesFiles)
                {
                    _testOutputHelper.WriteLine($"\t{updatedRepositoryDependenciesFile}");
                }
            }
        }
    }
}