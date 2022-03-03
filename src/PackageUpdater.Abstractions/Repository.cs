using System.Collections.Generic;

namespace PackageUpdater.Abstractions
{
    public class Repository
    {
        public string Path { get; }
        public IEnumerable<string> DependenciesFiles { get; }

        public Repository(string path, IEnumerable<string> dependenciesFiles)
        {
            Path = path;
            DependenciesFiles = dependenciesFiles;
        }

        public override string ToString() => $"Path: {Path}, DependenciesFiles: {string.Join(", ", DependenciesFiles)}";
    }
}
