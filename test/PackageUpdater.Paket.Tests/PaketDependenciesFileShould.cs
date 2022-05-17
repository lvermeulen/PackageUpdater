using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;
using Xunit;

namespace PackageUpdater.Paket.Tests
{
    public class PaketDependenciesFileShould
    {
        [Fact]
        public async Task LoadAsync()
        {
            var result = await PaketDependenciesFile.LoadAsync(new Repository(@"C:\vlrepo\api", new [] { @"C:\vlrepo\api\paket.dependencies" }), CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotNull(result.Dependencies);
            Assert.NotEmpty(result.Dependencies);
        }
    }
}
