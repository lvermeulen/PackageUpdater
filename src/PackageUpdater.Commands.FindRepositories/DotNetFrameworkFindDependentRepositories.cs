using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.FindRepositories;

public class DotNetFrameworkFindDependentRepositories : IFindDependentRepositories
{
    private static async Task<InspectFolderResult> InspectFolder(string packageName, DirectoryInfo directoryInfo, CancellationToken cancellationToken = default)
    {
        var results = new List<string>();

        // find .csproj files
        var findRepository = new DotNetFrameworkFindDependencyFiles();
        var repositories = await findRepository.FindDependencyFiles(directoryInfo.FullName, cancellationToken);
        foreach (var repository in repositories)
        {
            results.AddRange(repository.DependenciesFiles.Where(dependenciesFile => !Project.IsPackageReferenceProject(dependenciesFile)
                && Project.ContainsPackage(dependenciesFile, packageName)));
        }

        return results.Any()
            ? InspectFolderResult.Success(new Repository(directoryInfo.FullName, results))
            : InspectFolderResult.Failure();
    }

    private async Task<IEnumerable<Repository>> FindDependentRepositoriesInDirectory(FindRepositoriesCommandInput input, CancellationToken cancellationToken = default)
    {
        var results = new List<Repository>();
        var directoryInfo = new DirectoryInfo(input.Path);

        var result = await InspectFolder(input.PackageName, directoryInfo, cancellationToken);
        if (result.IsSuccess)
        {
            results.Add(new Repository(directoryInfo.FullName, result.Repository!.DependenciesFiles));
        }

        return results;
    }

    public async Task<IEnumerable<Repository>> FindDependentRepositories(FindRepositoriesCommandInput input, Action<Repository>? repositoryFound = default, CancellationToken cancellationToken = default)
    {
        var results = new List<Repository>();

        var enumerateDirectories = new DirectoryInfo(input.Path).EnumerateDirectories("*.*", SearchOption.TopDirectoryOnly);

        // top folder
        var repositories = (await FindDependentRepositoriesInDirectory(input, cancellationToken)).ToList();
        results.AddRange(repositories);
        repositories.ForEach(x => repositoryFound?.Invoke(x));

        // subfolders
        foreach (var directoryInfo in enumerateDirectories)
        {
            var directoryInput = input with { Path = directoryInfo.FullName };
            repositories = (await FindDependentRepositoriesInDirectory(directoryInput, cancellationToken)).ToList();
            results.AddRange(repositories);
            repositories.ForEach(x => repositoryFound?.Invoke(x));
        }

        return results;
    }
}