namespace PackageUpdater.Abstractions;

public class Dependency
{
    public Repository Repository { get; }
    public string PackageName { get; }
    public string PackageVersion { get; }

    public Dependency(Repository repository, string packageName, string packageVersion)
    {
        Repository = repository;
        PackageName = packageName;
        PackageVersion = packageVersion;
    }

    public override string ToString() => $"{PackageName} {PackageVersion}";
}