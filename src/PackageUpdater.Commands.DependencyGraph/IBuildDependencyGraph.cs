using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;
using QuikGraph;

namespace PackageUpdater.Commands.DependencyGraph;

public interface IBuildDependencyGraph
{
    Task<AdjacencyGraph<Repository, Edge<Repository>>> BuildDependencyGraph(DependencyGraphCommandInput input, CancellationToken cancellationToken = default);
}