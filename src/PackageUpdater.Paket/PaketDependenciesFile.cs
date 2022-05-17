using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Paket
{
    public class PaketDependenciesFile
    {
        // framework: net5.0
        // source https://api.nuget.org/v3/index.json

        // // PRODUCTION STUFF
        // nuget Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer 5.0.0

        public static async Task<PaketDependenciesFile> LoadAsync(Repository repository, CancellationToken cancellationToken)
        {
            var lines = await File.ReadAllLinesAsync(repository.DependenciesFiles.First(), cancellationToken);
            var dependencies = ParseDependencies(repository, lines);
            
            return new PaketDependenciesFile(dependencies);
        }

        private static IEnumerable<Dependency> ParseDependencies(Repository repository, IEnumerable<string> lines) => lines
            .Where(x => x.StartsWith("nuget", StringComparison.InvariantCultureIgnoreCase))
            .Select(x => x[6..])
            .Select(x => x.Split(new[] { " ", "\t" }, 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Where(x => x.Length == 2)
            .Select(x => new
            {
                repository,
                packageName = x[0],
                packageVersion = x[1]
            })
            .Select(x => new Dependency(x.repository, x.packageName, x.packageVersion));

        public IEnumerable<Dependency> Dependencies { get; }

        public PaketDependenciesFile(IEnumerable<Dependency> dependencies = null)
        {
            Dependencies = dependencies ?? Enumerable.Empty<Dependency>();
        }
    }
}
