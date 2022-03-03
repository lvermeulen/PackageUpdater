using System.Linq;
using System.Threading.Tasks;
using PackageUpdater.Abstractions.CommandInput;
using Xunit;
using Xunit.Abstractions;

namespace PackageUpdater.Paket.Tests
{
    public class PaketFindDependentRepositoriesShould
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PaketFindDependentRepositoriesShould(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory(Skip = "Add test parameters")]
        [InlineData("", @"")]
        public async Task FindDependentRepositories(string packageName, string path)
        {
            var input = new FindRepositoriesCommandInput
            {
                PackageNameFlag = packageName,
                PathFlag = path
            };
            var finder = new PaketFindDependentRepositories();
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
}
