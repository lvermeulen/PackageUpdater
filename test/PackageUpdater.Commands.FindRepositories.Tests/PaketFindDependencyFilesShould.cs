using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PackageUpdater.Commands.FindRepositories.Tests;

public class PaketFindDependencyFilesShould
{
    [Theory]
    [InlineData("")]
    public async Task FindDependencyFiles(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            path = Path.GetTempPath();
        }
        var findRepo = new PaketFindDependencyFiles();
        var results = (await findRepo.FindDependencyFiles(path)).ToList();

        Assert.NotNull(results);
        Assert.All(results.Select(x => x.DependenciesFiles.First()), x => Assert.False(string.IsNullOrWhiteSpace(x)));
    }
}