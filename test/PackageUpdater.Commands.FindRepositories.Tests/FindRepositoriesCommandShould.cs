using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace PackageUpdater.Commands.FindRepositories.Tests;

public class FindRepositoriesCommandShould
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(new []{ new KeyValuePair<string, string?>("NuGetExePath", @"C:\Tools\nuget\nuget.exe")})
        .Build();

    [Theory]
    [InlineData("Newtonsoft.Json", @"C:\wgkrepo\WgkOvl.Logging.Serilog")]
    public void Handle(string packageName, string path)
    {
        var command = new FindRepositoriesCommand(_configuration);
        var parseResult = command.Parse($"--packageName {packageName} --path {path}");
        var ctx = new InvocationContext(parseResult, new SystemConsole());
        var handler = command.Handler;
        var result = handler?.Invoke(ctx);

        Assert.Equal(0, result);
    }
}