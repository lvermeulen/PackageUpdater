using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;
using PackageUpdater.Commands.FindRepositories;
using QuikGraph;

namespace PackageUpdater.Commands.DependencyGraph;

public class PaketBuildDependencyGraph : IBuildDependencyGraph
{
    public async Task<AdjacencyGraph<Repository, Edge<Repository>>> BuildDependencyGraph(DependencyGraphCommandInput input, CancellationToken cancellationToken = default)
    {
        var graph = new AdjacencyGraph<Repository, Edge<Repository>>();

        var inputRepository = new Repository(input.Path, Enumerable.Empty<string>());

        var finder = new PaketFindDependencyFiles();
        var repositories = await finder.FindDependencyFiles(inputRepository.Path, cancellationToken);
        foreach (var repository in repositories)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return graph;
            }

            var dependenciesFile = await PaketDependenciesFile.LoadAsync(repository, cancellationToken);
            repository.Dependencies = dependenciesFile.Dependencies;

            foreach (var dependency in repository.Dependencies)
            {
                graph.AddVerticesAndEdge(new Edge<Repository>(inputRepository, dependency.Repository));
            }
        }

        return graph;
    }
}