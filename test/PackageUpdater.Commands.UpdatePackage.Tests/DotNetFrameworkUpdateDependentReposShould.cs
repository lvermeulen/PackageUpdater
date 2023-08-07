using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;
using PackageUpdater.Commands.FindRepositories;
using Xunit;
using Xunit.Abstractions;

namespace PackageUpdater.Commands.UpdatePackage.Tests;

public class DotNetFrameworkUpdateDependentReposShould
{
    private readonly ITestOutputHelper _testOutputHelper;

    public DotNetFrameworkUpdateDependentReposShould(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("", "", "")]
    public async Task UpdateDependentRepositories(string packageName, string path, string packageVersion)
    {
        if (string.IsNullOrEmpty(path))
        {
            path = Path.GetTempPath();
        }
        var input = new UpdatePackageCommandInput(packageVersion, packageName, PackageStrategy.DotNetFramework, path, WhatIf: true);
        var finder = new DotNetFrameworkFindDependentRepositories();
        var results = await finder.FindDependentRepositories(input);

        var updater = new DotNetFrameworkUpdateDependentRepositories();
        var updatedRepositories = (await updater.UpdateDependentRepositories(input, results)).ToList();

        if (updatedRepositories.Any())
        {
            _testOutputHelper.WriteLine("The following files are updated:");
            foreach (var updatedRepository in updatedRepositories)
            {
                _testOutputHelper.WriteLine($"\t{string.Join(", ", updatedRepository.DependenciesFiles)}");
            }
        }
    }
}