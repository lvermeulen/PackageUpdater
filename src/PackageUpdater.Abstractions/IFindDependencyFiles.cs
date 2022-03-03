using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PackageUpdater.Abstractions
{
    public interface IFindDependencyFiles
    {
        Task<IEnumerable<Repository>> FindDependencyFiles(string path, CancellationToken cancellationToken = default);
    }
}
