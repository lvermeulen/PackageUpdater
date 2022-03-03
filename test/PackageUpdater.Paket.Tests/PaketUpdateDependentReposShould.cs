using System.Linq;
using System.Threading.Tasks;
using PackageUpdater.Abstractions.CommandInput;
using Xunit;
using Xunit.Abstractions;

namespace PackageUpdater.Paket.Tests
{
    public class PaketUpdateDependentReposShould
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PaketUpdateDependentReposShould(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory(Skip = "Add test parameters")]
        [InlineData("", @"", "")]
        public async Task UpdateDependentRepositories(string packageName, string path, string packageVersion)
        {
            var input = new UpdatePackageCommandInput
            {
                PackageNameFlag = packageName,
                PackageVersionFlag = packageVersion,
                PathFlag = path,
                WhatIfFlag = true
            };
            var finder = new PaketFindDependentRepositories();
            var results = await finder.FindDependentRepositories(input);

            var updater = new PaketUpdateDependentRepositories();
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
}
