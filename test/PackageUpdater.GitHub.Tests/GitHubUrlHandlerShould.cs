using System.Threading.Tasks;
using Xunit;

namespace PackageUpdater.GitHub.Tests;

public class GitHubUrlHandlerShould
{
    [Fact]
    public async Task GetRepositoriesAsync()
    {
        var urlHandler = new GitHubUrlHandler();

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