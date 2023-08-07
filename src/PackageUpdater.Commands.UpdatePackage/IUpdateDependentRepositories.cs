using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions;

namespace PackageUpdater.Commands.UpdatePackage;

public interface IUpdateDependentRepositories
{
    Task<IEnumerable<Repository>> UpdateDependentRepositories(UpdatePackageCommandInput input, IEnumerable<Repository> repositories,
        Action<Repository>? beforeUpdateRepository = default, Action<string>? afterUpdateRepository = default, CancellationToken cancellationToken = default);
}