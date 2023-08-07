using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.FindRepositories;

public class PaketFindDependencyFiles : IFindDependencyFiles
{
    public Task<IEnumerable<Repository>> FindDependencyFiles(string path, CancellationToken cancellationToken = default)
    {
        var files = new DirectoryInfo(path).EnumerateFiles("paket.dependencies", SearchOption.AllDirectories);
        var results = files.Select(file => new Repository(file.Directory?.FullName!, new[] { file.FullName }));

        return Task.FromResult(results);
    }
}