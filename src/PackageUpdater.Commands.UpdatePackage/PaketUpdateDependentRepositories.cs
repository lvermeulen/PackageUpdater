using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.UpdatePackage;

public class PaketUpdateDependentRepositories : IUpdateDependentRepositories
{
    private async Task<Repository?> UpdateDependentRepository(UpdatePackageCommandInput input, Repository repository,
        Action<Repository>? beforeUpdateRepository = default, Action<string>? afterUpdateRepository = default, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return default;
        }

        var dependenciesFile = repository.DependenciesFiles.First();
        if (!File.Exists(dependenciesFile))
        {
            return default;
        }
        var contents = (await File.ReadAllLinesAsync(dependenciesFile, cancellationToken)).ToList();
        var contentsFound = contents.Find(x => x.StartsWith($"nuget {input.PackageName}", StringComparison.InvariantCultureIgnoreCase));
        var index = contents.IndexOf(contentsFound);
        contents[index] = $"nuget {input.PackageName} {input.PackageVersion}";

        beforeUpdateRepository?.Invoke(repository);
        if (!input.WhatIf)
        {
            await File.WriteAllLinesAsync(repository.DependenciesFiles.First(), contents, cancellationToken);
            afterUpdateRepository?.Invoke(dependenciesFile);
        }

        return repository;
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