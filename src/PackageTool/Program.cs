using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Oakton;
using Oakton.Help;
using PackageTool.Commands;

[assembly:OaktonCommandAssembly]

namespace PackageTool
{
    public static class Program
    {
        public static Task<int> Main(string[] args) => CreateHostBuilder(args)
            .ConfigureHostConfiguration(_ =>
            {
                if (args.Length <= 1)
                {
                    new HelpCommand().Execute(new HelpInput
                    {
                        AppName = nameof(PackageTool),
                        CommandTypes = new []
                        {
                            typeof(FindRepositoriesCommand),
                            typeof(UpdatePackageCommand)
                        }
                    });
                    Environment.Exit(1);
                }
            })
            .RunOaktonCommands(args);

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, services) => services.AddJsonFile("appsettings.json"));
    }
}
