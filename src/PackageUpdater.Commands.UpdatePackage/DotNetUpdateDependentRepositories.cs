using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.UpdatePackage;

public class DotNetUpdateDependentRepositories : IUpdateDependentRepositories
{
    private Task<Repository?> UpdateDependentRepository(UpdatePackageCommandInput input, Repository repository,
        Action<Repository>? beforeUpdateRepository = default, Action<string>? afterUpdateRepository = default, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromResult<Repository?>(default);
        }

        foreach (var dependenciesFile in repository.DependenciesFiles)
        {
            if (!File.Exists(dependenciesFile))
            {
                return Task.FromResult<Repository?>(default);
            }

            beforeUpdateRepository?.Invoke(repository);
            if (!input.WhatIf)
            {
                Project.UpdatePackageReference(dependenciesFile, input.PackageName, input.PackageVersion);
                afterUpdateRepository?.Invoke(dependenciesFile);
            }
        }

        return Task.FromResult<Repository?>(repository);
    }

    public async Task<IEnumerable<Repository>> UpdateDependentRepositories(UpdatePackageCommandInput input, IEnumerable<Repository> repositories,
        Action<Repository>? beforeUpdateRepository = default, Action<string>? afterUpdateRepository = default, CancellationToken cancellationToken = default)
    {
        var results = new List<Repository>();

        foreach (var repository in repositories)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return results;
            }

            var result = await UpdateDependentRepository(input, repository, beforeUpdateRepository, afterUpdateRepository, cancellationToken);
            if (result is not null)
            {
                results.Add(result);
            }
        }

        return results;
    }
}