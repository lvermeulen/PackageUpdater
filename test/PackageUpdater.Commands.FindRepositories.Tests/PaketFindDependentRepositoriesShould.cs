using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace PackageUpdater.Commands.FindRepositories.Tests;

public class PaketFindDependentRepositoriesShould
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PaketFindDependentRepositoriesShould(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("Newtonsoft.Json", "")]
    public async Task FindDependentRepositories(string packageName, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            path = Path.GetTempPath();
        }
        var input = new FindRepositoriesCommandInput(packageName, PackageStrategy.Paket, path);
        var finder = new PaketFindDependentRepositories();
        var results = (await finder.FindDependentRepositories(input)).ToList();

        Assert.NotNull(results);
        Assert.All(results.Select(x => x.Path), Assert.NotNull);

        foreach (var repository in results)
        {
            _testOutputHelper.WriteLine(string.Join(", ", repository.DependenciesFiles));
        }
    }
}