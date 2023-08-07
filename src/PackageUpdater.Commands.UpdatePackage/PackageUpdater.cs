using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PackageUpdater.Abstractions;
using PackageUpdater.Commands.FindRepositories;

namespace PackageUpdater.Commands.UpdatePackage;

public class Updater
{
    private readonly IFindDependentRepositories _finder;
    private readonly IUpdateDependentRepositories _updater;

    public Updater(IServiceProvider services)
    {
        _finder = services.GetRequiredService<IFindDependentRepositories>();
        _updater = services.GetRequiredService<IUpdateDependentRepositories>();
    }

    public async Task<IEnumerable<Repository>> UpdatePackage(UpdatePackageCommandInput input,
        Action<Repository>? beforeUpdateRepository = default, Action<string>? afterUpdateRepository = default, CancellationToken cancellationToken = default)
    {
        var results = new List<Repository>();

        var repositories = await _finder.FindDependentRepositories(input, cancellationToken:cancellationToken);
        results.AddRange(await _updater.UpdateDependentRepositories(input, repositories, beforeUpdateRepository, afterUpdateRepository, cancellationToken));

        return results;
    }
}