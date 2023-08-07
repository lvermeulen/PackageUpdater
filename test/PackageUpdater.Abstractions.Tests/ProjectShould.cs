using Xunit;

namespace PackageUpdater.Abstractions.Tests;

public class ProjectShould
{
    [Theory]
    [InlineData(@"c:\wgkrepo\WgkOvl.Omz.Api.Core\src\WgkOvl.Omz.Api.Core\WgkOvl.Omz.Api.Core.csproj", true)]
    public void IsPackageReferenceProject(string projectFileName, bool expectedResult)
    {
        Assert.Equal(expectedResult, Project.IsPackageReferenceProject(projectFileName));
    }
}