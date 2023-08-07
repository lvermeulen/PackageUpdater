using System.Linq;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace PackageUpdater.Commands.FindRepositories.Tests;

public class DotNetFindDependentRepositoriesShould
{
    private readonly ITestOutputHelper _testOutputHelper;

    public DotNetFindDependentRepositoriesShould(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("Serilog")]
    public async Task FindDependentRepositories(string packageName)
    {
        const string path = @"C:\wgkrepo\WgkOvl.Logging.Serilog";
        var input = new FindRepositoriesCommandInput(packageName, PackageStrategy.DotNet, path);
        var finder = new DotNetFindDependentRepositories();
        var results = (await finder.FindDependentRepositories(input)).ToList();

        Assert.NotNull(results);
        Assert.NotEmpty(results);
        Assert.All(results.Select(x => x.Path), Assert.NotNull);

        foreach (var repository in results)
        {
            _testOutputHelper.WriteLine(string.Join(", ", repository.DependenciesFiles));
        }
    }
}