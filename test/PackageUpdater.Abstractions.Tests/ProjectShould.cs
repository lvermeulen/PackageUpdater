using Xunit;

namespace PackageUpdater.Abstractions.Tests
{
    public class ProjectShould
    {
        [Theory(Skip = "Add test parameters")]
        [InlineData(@"", true)]
        public void IsPackageReferenceProject(string projectFileName, bool expectedResult)
        {
            Assert.Equal(expectedResult, Project.IsPackageReferenceProject(projectFileName));
        }
    }
}
