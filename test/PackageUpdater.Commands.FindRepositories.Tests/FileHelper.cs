using System.IO;

namespace PackageUpdater.Commands.FindRepositories.Tests;

public static class FileHelper
{
    public static void Touch(this string fileName)
    {
        using var _ = File.Create(fileName);
    }
}
