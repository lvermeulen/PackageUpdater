using System.CommandLine;
using System.CommandLine.Binding;
using PackageUpdater.Abstractions;
using PackageUpdater.Commands.FindRepositories;

namespace PackageUpdater.Commands.UpdatePackage;

public record UpdatePackageCommandInput(string PackageVersion, string PackageName, PackageStrategy Strategy, string Path, bool WhatIf)
    : FindRepositoriesCommandInput(PackageName, Strategy, Path);

public class UpdatePackageCommandInputBinder : BinderBase<UpdatePackageCommandInput?>
{
    private readonly Option<string> _packageVersion;
    private readonly Option<string> _packageName;
    private readonly Option<PackageStrategy> _strategy;
    private readonly Option<string> _path;
    private readonly Option<bool> _whatIf;

    public UpdatePackageCommandInputBinder(Option<string> packageVersion, Option<string> packageName, Option<PackageStrategy> strategy, Option<string> path, Option<bool> whatIf)
    {
        _packageVersion = packageVersion;
        _packageName = packageName;
        _strategy = strategy;
        _path = path;
        _whatIf = whatIf;
    }

    protected override UpdatePackageCommandInput? GetBoundValue(BindingContext bindingContext)
    {
        var packageVersion = bindingContext.ParseResult.GetValueForOption(_packageVersion);
        var packageName = bindingContext.ParseResult.GetValueForOption(_packageName);
        var strategy = bindingContext.ParseResult.GetValueForOption(_strategy);
        var path = bindingContext.ParseResult.GetValueForOption(_path);
        var whatIf = bindingContext.ParseResult.GetValueForOption(_whatIf);
        if (packageVersion is null || packageName is null || path is null)
        {
            return default;
        }

        return new UpdatePackageCommandInput(packageVersion, packageName, strategy, path, whatIf);
    }
}
