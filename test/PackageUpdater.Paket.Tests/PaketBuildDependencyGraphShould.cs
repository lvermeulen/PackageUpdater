using System.Linq;
using PackageUpdater.Abstractions.CommandInput;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using System.Threading;
using PackageUpdater.Abstractions;
using QuikGraph.Algorithms;

namespace PackageUpdater.Paket.Tests
{
    public class PaketBuildDependencyGraphShould
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PaketBuildDependencyGraphShould(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(@"C:\vlrepo")]
        public async Task BuildDependencyGraph(string path)
        {
            var input = new DependencyGraphCommandInput
            {
                PathFlag = path,
                StrategyFlag = UpdatePackageStrategy.Paket
            };

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
}
