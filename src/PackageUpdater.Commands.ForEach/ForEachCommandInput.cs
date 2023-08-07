using System.CommandLine;
using System.CommandLine.Binding;

namespace PackageUpdater.Commands.ForEach;

public record ForEachCommandInput(string Url, string Action, string Path);

public class ForEachCommandInputBinder : BinderBase<ForEachCommandInput?>
{
    private readonly Option<string> _url;
    private readonly Option<string> _action;
    private readonly Option<string> _path;

    public ForEachCommandInputBinder(Option<string> url, Option<string> action, Option<string> path)
    {
        _url = url;
        _action = action;
        _path = path;
    }

    protected override ForEachCommandInput? GetBoundValue(BindingContext bindingContext)
    {
        var url = bindingContext.ParseResult.GetValueForOption(_url);
        var action = bindingContext.ParseResult.GetValueForOption(_action);
        var path = bindingContext.ParseResult.GetValueForOption(_path);
        if (url is null || action is null || path is null)
        {
            return default;
        }

        return new ForEachCommandInput(url, action, path);
    }
}
