using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Commands.DependencyGraph;
using QuikGraph.Algorithms;
using Xunit;
using Xunit.Abstractions;

namespace PackageUpdater.Paket.Tests;

public class PaketBuildDependencyGraphShould
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PaketBuildDependencyGraphShould(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("")]
    public async Task BuildDependencyGraph(string _)
    {
        var path = Path.GetTempPath();
        var input = new DependencyGraphCommandInput(path);
        var grapher = new PaketBuildDependencyGraph();
        var graph = await grapher.BuildDependencyGraph(input, CancellationToken.None);

        if (graph.Edges.Any())
        {
            //_testOutputHelper.WriteLine("Dependencies exist between the following repositories:");
            //foreach (var edge in graph.Edges.Distinct())
            //{
            //    _testOutputHelper.WriteLine($"\t{string.Join(", ", edge.Target.DependenciesFiles)}");
            //}

            var vertices = graph.TopologicalSort();
            _testOutputHelper.WriteLine("\nTopological sort:");
            foreach (var vertex in vertices)
            {
                _testOutputHelper.WriteLine(vertex.Path);
            }
        }

        Assert.NotNull(graph);
    }
}