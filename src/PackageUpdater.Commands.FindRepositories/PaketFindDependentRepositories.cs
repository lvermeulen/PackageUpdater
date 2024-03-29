﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.FindRepositories;

public class PaketFindDependentRepositories : IFindDependentRepositories
{
    private async Task<InspectFolderResult> InspectFolder(string packageName, DirectoryInfo directoryInfo, CancellationToken cancellationToken = default)
    {
        // find paket.dependencies files
        var findRepository = new PaketFindDependencyFiles();
        var repositories = await findRepository.FindDependencyFiles(directoryInfo.FullName, cancellationToken);
        foreach (var repository in repositories)
        {
            var contents = await File.ReadAllLinesAsync(repository.DependenciesFiles.First(), cancellationToken);
            if (contents.Any(x => x.StartsWith("nuget ") && x[6..].StartsWith(packageName, true, CultureInfo.InvariantCulture)))
            {
                return InspectFolderResult.Success(new Repository(directoryInfo.FullName, repository.DependenciesFiles));
            }
        }

        return InspectFolderResult.Failure();
    }

    public async Task<IEnumerable<Repository>> FindDependentRepositories(FindRepositoriesCommandInput input, Action<Repository>? repositoryFound = default, CancellationToken cancellationToken = default)
    {
        var results = new List<Repository>();

        foreach (var directoryInfo in new DirectoryInfo(input.Path).EnumerateDirectories("*.*", SearchOption.TopDirectoryOnly))
        {
            var result = await InspectFolder(input.PackageName, directoryInfo, cancellationToken);
            if (result.IsSuccess)
            {
                var repository = new Repository(directoryInfo.FullName, result.Repository!.DependenciesFiles);
                results.Add(repository);
                repositoryFound?.Invoke(repository);
            }
        }

        return results;
    }
}