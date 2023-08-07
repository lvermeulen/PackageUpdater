using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace PackageUpdater.Commands.UpdatePackage.Tests;

public class UpdatePackageCommandShould
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(new []{ new KeyValuePair<string, string?>("NuGetExePath", @"C:\Tools\nuget\nuget.exe")})
        .Build();

    [Theory]
    [InlineData("14.0.0", "Newtonsoft.Json", @"C:\wgkrepo\WgkOvl.Logging.Serilog")]
    public void Handle(string packageVersion, string packageName, string path)
    {
        var command = new UpdatePackageCommand(_configuration);
        var parseResult = command.Parse($"--packageVersion {packageVersion} --packageName {packageName} --path {path}");
        var ctx = new InvocationContext(parseResult, new SystemConsole());
        var handler = command.Handler;
        var result = handler?.Invoke(ctx);

        Assert.Equal(0, result);
    }
}