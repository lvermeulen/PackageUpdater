using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;
using Xunit;

namespace PackageUpdater.Commands.FindRepositories.Tests;

public class PaketDependenciesFileShould
{
    [Fact]
    public async Task LoadAsync()
    {
        var path = Path.GetTempPath();
        var fileName = Path.Combine(path, "paket.dependencies");
        fileName.Touch();

        var result = await PaketDependenciesFile.LoadAsync(new Repository(path, new[] { fileName }), CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotNull(result.Dependencies);
    }
}