using System.CommandLine;
using System.CommandLine.Binding;

namespace PackageUpdater.Commands.DependencyGraph;

public record DependencyGraphCommandInput(string Path);

public class DependencyGraphCommandInputBinder : BinderBase<DependencyGraphCommandInput?>
{
    private readonly Option<string> _path;

    public DependencyGraphCommandInputBinder(Option<string> path)
    {
        _path = path;
    }

    protected override DependencyGraphCommandInput? GetBoundValue(BindingContext bindingContext)
    {
        var path = bindingContext.ParseResult.GetValueForOption(_path);
        if (path is null)
        {
            return default;
        }

        return new DependencyGraphCommandInput(path);
    }
}
