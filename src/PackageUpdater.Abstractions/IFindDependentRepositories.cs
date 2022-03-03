using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PackageUpdater.Abstractions.CommandInput;

namespace PackageUpdater.Abstractions
{
    public interface IFindDependentRepositories
    {
        Task<IEnumerable<Repository>> FindDependentRepositories(FindRepositoriesCommandInput input, Action<Repository> repositoryFound = null, CancellationToken cancellationToken = default);
    }
}
