using System.Threading.Tasks;
using Xunit;

namespace PackageUpdater.GitHub.Tests
{
    public class GitHubUrlHandlerShould
    {
        [Fact]
        public async Task GetRepositoriesAsync()
        {
            var options = new GitHubOptions
            {
                UserName = "luk.vermeulen@gmail.com",
                Password = "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"
            };
            var urlHandler = new GitHubUrlHandler(options);

            var repoOptions = new GitHubRepositoryOptions
            {
                Organization = "InformatieVlaanderen",
                Topic = "base-registries"
            };
            var repositories = await urlHandler.GetRepositories(repoOptions);

            Assert.NotNull(repositories);
            Assert.NotEmpty(repositories);
        }
    }
}
