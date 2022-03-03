using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;
using PackageUpdater.Abstractions.CommandInput;

namespace PackageUpdater.Paket
{
    public class PaketUpdateDependentRepositories : IUpdateDependentRepositories
    {
        private async Task<Repository> UpdateDependentRepository(UpdatePackageCommandInput input, Repository repository,
            Action<Repository> beforeUpdateRepository = null, Action<string> afterUpdateRepository = null, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            string dependenciesFile = repository.DependenciesFiles.First();
            if (!File.Exists(dependenciesFile))
            {
                return null;
            }
            var contents = (await File.ReadAllLinesAsync(dependenciesFile, cancellationToken)).ToList();
            int index = contents.IndexOf(contents.Find(x => x.StartsWith($"nuget {input.PackageNameFlag}", StringComparison.InvariantCultureIgnoreCase)));
            contents[index] = $"nuget {input.PackageNameFlag} {input.PackageVersionFlag}";

            beforeUpdateRepository?.Invoke(repository);
            if (!input.WhatIfFlag)
            {
                await File.WriteAllLinesAsync(repository.DependenciesFiles.First(), contents, cancellationToken);
                afterUpdateRepository?.Invoke(dependenciesFile);
            }

            return repository;
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
