using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;
using PackageUpdater.Abstractions.CommandInput;

namespace PackageUpdater.DotNet
{
    public class DotNetUpdateDependentRepositories : IUpdateDependentRepositories
    {
        private Task<Repository> UpdateDependentRepository(UpdatePackageCommandInput input, Repository repository,
            Action<Repository> beforeUpdateRepository = null, Action<string> afterUpdateRepository = null, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromResult<Repository>(null);
            }

            foreach (string dependenciesFile in repository.DependenciesFiles)
            {
                if (!File.Exists(dependenciesFile))
                {
                    return Task.FromResult<Repository>(null);
                }

                beforeUpdateRepository?.Invoke(repository);
                if (!input.WhatIfFlag)
                {
                    Project.UpdatePackageReference(dependenciesFile, input.PackageNameFlag, input.PackageVersionFlag);
                    afterUpdateRepository?.Invoke(dependenciesFile);
                }
            }

            return Task.FromResult(repository);
        }

        public async Task<IEnumerable<Repository>> UpdateDependentRepositories(UpdatePackageCommandInput input, IEnumerable<Repository> repositories,
            Action<Repository> beforeUpdateRepository = null, Action<string> afterUpdateRepository = null, CancellationToken cancellationToken = default)
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
}
