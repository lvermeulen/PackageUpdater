using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions.CommandInput;

namespace PackageUpdater.Abstractions
{
    public interface IUpdateDependentRepositories
    {
        Task<IEnumerable<Repository>> UpdateDependentRepositories(UpdatePackageCommandInput input, IEnumerable<Repository> repositories,
            Action<Repository> beforeUpdateRepository = null, Action<string> afterUpdateRepository = null, CancellationToken cancellationToken = default);
    }
}
