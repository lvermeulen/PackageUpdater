using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.FindRepositories;

public interface IFindDependentRepositories
{
    Task<IEnumerable<Repository>> FindDependentRepositories(FindRepositoriesCommandInput input, Action<Repository>? repositoryFound = default, CancellationToken cancellationToken = default);
}