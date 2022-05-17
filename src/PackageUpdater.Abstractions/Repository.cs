using System.Collections.Generic;
using System.Linq;

namespace PackageUpdater.Abstractions
{
    public class Repository
    {
        public string Path { get; }
        public IEnumerable<string> DependenciesFiles { get; }
        public IEnumerable<Dependency> Dependencies { get; set; }

        public Repository(string path, IEnumerable<string> dependenciesFiles = null)
        {
            Path = path;
            DependenciesFiles = dependenciesFiles ?? Enumerable.Empty<string>();
        }

        public override string ToString() => $"Path: {Path}, DependenciesFiles: {string.Join(", ", DependenciesFiles)}";
    }
}
