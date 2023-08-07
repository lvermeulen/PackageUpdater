using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.FindRepositories;

public interface IFindDependencyFiles
{
    Task<IEnumerable<Repository>> FindDependencyFiles(string path, CancellationToken cancellationToken = default);
}