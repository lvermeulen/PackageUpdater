using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions.CommandInput;
using QuikGraph;

namespace PackageUpdater.Abstractions
{
    public interface IBuildDependencyGraph
    {
        Task<AdjacencyGraph<Repository, Edge<Repository>>> BuildDependencyGraph(DependencyGraphCommandInput input, CancellationToken cancellationToken = default);
    }
}
