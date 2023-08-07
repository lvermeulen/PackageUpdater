using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using Octokit.Internal;

namespace PackageUpdater.GitHub;

public class GitHubUrlHandler
{
    public async Task<IEnumerable<Repository>> GetRepositories(GitHubRepositoryOptions? options = null)
    {
        options ??= new GitHubRepositoryOptions();

        var client = new GitHubClient(new ProductHeaderValue(nameof(GitHubUrlHandler)), new InMemoryCredentialStore(Credentials.Anonymous));
        var repositories = (await client.Repository.GetAllForOrg(options.Organization))
            .AsEnumerable();

        if (!string.IsNullOrEmpty(options.Topic))
        {
            repositories = repositories.Where(x => x.Topics.Contains(options.Topic, StringComparer.InvariantCultureIgnoreCase));
        }

        return repositories;
    }
}