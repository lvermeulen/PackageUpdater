using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.FindRepositories;

public class DotNetFrameworkFindDependencyFiles : IFindDependencyFiles
{
    public Task<IEnumerable<Repository>> FindDependencyFiles(string path, CancellationToken cancellationToken = default)
    {
        var files = new DirectoryInfo(path).EnumerateFiles("packages.config", SearchOption.AllDirectories);
        var results = files.Select(file => new Repository(file.Directory?.FullName!, new[] { file.FullName }));

        return Task.FromResult(results);
    }
}