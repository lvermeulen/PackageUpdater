using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PackageUpdater.Paket.Tests
{
    public class PaketFindDependencyFilesShould
    {
        [Theory(Skip = "Add test parameters")]
        [InlineData(@"")]
        public async Task FindDependencyFiles(string path)
        {
            var findRepo = new PaketFindDependencyFiles();
            var results = (await findRepo.FindDependencyFiles(path)).ToList();

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.All(results.Select(x => x.DependenciesFiles.First()), x => Assert.False(string.IsNullOrWhiteSpace(x)));
        }
    }
}
