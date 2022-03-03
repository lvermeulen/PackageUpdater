using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PackageUpdater.DotNetFramework.Tests
{
    public class DotNetFrameworkFindDependencyFilesShould
    {
        [Theory(Skip = "Add test parameters")]
        [InlineData(@"")]
        public async Task FindDependencyFiles(string path)
        {
            var findRepo = new DotNetFrameworkFindDependencyFiles();
            var results = (await findRepo.FindDependencyFiles(path)).ToList();

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.All(results.Select(x => x.DependenciesFiles.First()), x => Assert.False(string.IsNullOrWhiteSpace(x)));
        }
    }
}
