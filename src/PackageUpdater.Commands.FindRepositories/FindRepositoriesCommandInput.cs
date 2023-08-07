using System.CommandLine;
using System.CommandLine.Binding;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.FindRepositories;

public record FindRepositoriesCommandInput(string PackageName, PackageStrategy Strategy, string Path);

public class FindRepositoriesCommandInputBinder : BinderBase<FindRepositoriesCommandInput?>
{
    private readonly Option<string> _packageName;
    private readonly Option<PackageStrategy> _strategy;
    private readonly Option<string> _path;

    public FindRepositoriesCommandInputBinder(Option<string> packageName, Option<PackageStrategy> strategy, Option<string> path)
    {
        _packageName = packageName;
        _strategy = strategy;
        _path = path;
    }

    protected override FindRepositoriesCommandInput? GetBoundValue(BindingContext bindingContext)
    {
        var packageName = bindingContext.ParseResult.GetValueForOption(_packageName);
        var strategy = bindingContext.ParseResult.GetValueForOption(_strategy);
        var path = bindingContext.ParseResult.GetValueForOption(_path);
        if (packageName is null || path is null)
        {
            return default;
        }

        return new FindRepositoriesCommandInput(packageName, strategy, path);
    }
}
