using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PackageUpdater.Commands.FindRepositories.Tests;

public class DotNetFindDependencyFilesShould
{
    [Fact]
    public async Task FindDependencyFiles()
    {
        var path = Path.GetTempPath();
        var findRepo = new DotNetFindDependencyFiles();
        var results = (await findRepo.FindDependencyFiles(path)).ToList();

        Assert.NotNull(results);
        Assert.NotEmpty(results);
        Assert.All(results.Select(x => x.DependenciesFiles.First()), x => Assert.False(string.IsNullOrWhiteSpace(x)));
    }
}