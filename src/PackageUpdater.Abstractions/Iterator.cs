using System.IO;
using System.Threading.Tasks;
using PackageUpdater.Abstractions.CommandInput;

namespace PackageUpdater.Abstractions
{
    public class Iterator
    {
        public async Task Iterate(ForEachCommandInput input)
        {
            string url = input.UrlFlag ?? string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                //var response = url.Get("https://github.com/topics/base-registries");
            }

            var folders = new DirectoryInfo(input.PathFlag).EnumerateDirectories("*.*", SearchOption.TopDirectoryOnly);
            foreach (var folder in folders)
            {
                var processRunner = new ProcessRunner();
                await processRunner.RunProcessAsync(input.ActionFlag, "", folder.FullName);
            }
        }
    }
}
