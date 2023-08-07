using System.IO;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.ForEach;

public class Iterator
{
    public async Task Iterate(ForEachCommandInput input)
    {
        var url = input.Url;
        if (string.IsNullOrEmpty(url))
        {
            // TODO
            //var response = url.Get("https://github.com/topics/base-registries");
        }

        var folders = new DirectoryInfo(input.Path).EnumerateDirectories("*.*", SearchOption.TopDirectoryOnly);
        foreach (var folder in folders)
        {
            var processRunner = new ProcessRunner();
            await processRunner.RunProcessAsync(input.Action, "", folder.FullName);
        }
    }
}