using System;
using System.CommandLine;
using System.Linq;

namespace PackageTool.Extensions;

internal static class RootCommandExtensions
{
    public static void AddCommandsFromAssemblyOf<T>(this RootCommand rootCommand)
    {
        var assembly = typeof(T).Assembly;
        var commands = assembly.GetTypes()
            .Where(x => x.IsSubclassOf(typeof(Command)));

        foreach (var item in commands)
        {
            if (Activator.CreateInstance(item) is Command command)
            {
                rootCommand.AddCommand(command);
            }
        }
    }
}
