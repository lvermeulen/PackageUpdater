using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace PackageUpdater.Abstractions;

public static class Project
{
    private static readonly Dictionary<string, string> s_targetMap = new Dictionary<string, string>
    {
        ["v1.1"] = "net11",
        ["v2.0"] = "net20",
        ["v3.5"] = "net35",
        ["v4.0"] = "net40",
        ["v4.0.3"] = "net403",
        ["v4.5"] = "net45",
        ["v4.5.1"] = "net451",
        ["v4.5.2"] = "net452",
        ["v4.6"] = "net46",
        ["v4.6.1"] = "net461",
        ["v4.6.2"] = "net462",
        ["v4.7"] = "net47",
        ["v4.7.1"] = "net471",
        ["v4.7.2"] = "net472",
        ["v4.8"] = "net48"
    };

    public static bool IsPackageReferenceProjectFolder(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            return false;
        }

        var firstProjectFile = Directory.GetFiles(folderPath, "*.csproj", SearchOption.AllDirectories)
            .FirstOrDefault();

        return IsPackageReferenceProject(firstProjectFile);
    }

    private static bool IsPackageReferenceProject(XDocument projectDocument)
    {
        var sdkAttribute = projectDocument.Root
            ?.Attributes()
            .FirstOrDefault(x => x.Name == "Sdk");

        return sdkAttribute is not null;
    }

    public static bool IsPackageReferenceProject(string? projectFileName)
    {
        if (string.IsNullOrEmpty(projectFileName))
        {
            return false;
        }

        var doc = XDocument.Load(projectFileName);
        return IsPackageReferenceProject(doc);
    }

    public static IEnumerable<string> FindPackagesConfigPaths(string folderPath) => Directory.GetFiles(folderPath, "packages.config", SearchOption.AllDirectories);

    public static IEnumerable<string> FindProjectTargets(string projectFileName, bool includeNetCore, bool includeNetFramework)
    {
        static XElement? FindTargetFrameworkElement(XDocument document, bool includeNetCore, bool includeNetFramework)
        {
            var root = document.Root;
            var items = new List<XElement>();

            if (includeNetCore)
            {
                items = root?.Descendants("TargetFramework").ToList();
                if (items?.Count == 0)
                {
                    items = root?.Descendants("TargetFrameworks").ToList();
                }
            }

            if (includeNetFramework && items?.Count == 0)
            {
                items = root?.Descendants().Where(x => x.Name.LocalName == "TargetFrameworkVersion").ToList();
            }

            return items?.FirstOrDefault();
        }

        static string GetTargetName(string s) => s_targetMap.TryGetValue(s, out var target)
            ? target
            : s;

        if (string.IsNullOrEmpty(projectFileName))
        {
            return Enumerable.Empty<string>();
        }

        var doc = XDocument.Load(projectFileName);
        var targetFrameworks = FindTargetFrameworkElement(doc, includeNetCore, includeNetFramework)
            ?.Value;
        var pieces = targetFrameworks!
            .Split(";", StringSplitOptions.RemoveEmptyEntries);
        var findProjectTargets = pieces
            .Select(GetTargetName);
        return findProjectTargets;
    }

    public static IDictionary<string, IEnumerable<string>>? FindSolutionTargets(string solutionFile, bool includeNetCore, bool includeNetFramework)
    {
        if (string.IsNullOrEmpty(solutionFile))
        {
            return default;
        }

        return Directory
            .GetFiles(Path.GetDirectoryName(solutionFile) ?? Environment.CurrentDirectory, "*.csproj", SearchOption.AllDirectories)
            .ToDictionary(x => x, x => FindProjectTargets(x, includeNetCore, includeNetFramework));
    }

    private static XElement? WithNuGetPackageReferenceInProject(string projectFileName, string packageName, Action<XDocument, XElement>? elementAction = default)
    {
        static string PackageName(XElement element) => element.Attribute("Include")?.Value.Split(',')[0].Trim() ?? string.Empty;

        var doc = XDocument.Load(projectFileName);
        var ns = doc.Root?.GetDefaultNamespace();
        var packages = doc.Root
            ?.Descendants(ns! + "Reference");

        var first = (packages ?? Array.Empty<XElement>())
            .FirstOrDefault(x => PackageName(x).Equals(packageName, StringComparison.InvariantCultureIgnoreCase));
        if (first is not null)
        {
            elementAction?.Invoke(doc, first);
        }

        return first;
    }

    public static XElement? FindNuGetPackageReferenceInProject(string projectFileName, string packageName) => WithNuGetPackageReferenceInProject(projectFileName, packageName);

    public static XElement? WithNuGetPackageReferenceInPackagesConfig(string projectFileName, string packageName, Action<XDocument, XElement>? elementAction = default)
    {
        static string PackageName(XElement element) => element.Attribute("id")?.Value ?? string.Empty;

        var projectFolder = Path.GetDirectoryName(projectFileName);
        var packagesConfigFile = new DirectoryInfo(projectFolder ?? Environment.CurrentDirectory)
            .GetFiles("packages.config", SearchOption.TopDirectoryOnly)
            .FirstOrDefault()
            ?.FullName;

        if (packagesConfigFile is null)
        {
            return default;
        }

        var doc = XDocument.Load(packagesConfigFile);
        var packages = doc.Root
            ?.Descendants("package");

        var first = (packages ?? Array.Empty<XElement>())
            .FirstOrDefault(x => PackageName(x).Equals(packageName, StringComparison.InvariantCultureIgnoreCase));
        if (first is not null)
        {
            elementAction?.Invoke(doc, first);
        }

        return first;
    }

    public static XElement? FindNuGetPackageReferenceInPackagesConfig(string projectFileName, string packageName) => WithNuGetPackageReferenceInPackagesConfig(projectFileName, packageName);

    private static bool RemoveNuGetPackageReferenceFromProject(string projectFileName, string packageName)
    {
        WithNuGetPackageReferenceInProject(projectFileName, packageName, (doc, x) =>
        {
            using var writer = new XmlTextWriter(projectFileName, Encoding.UTF8);
            x.Remove();
            doc.Save(writer);
        });

        return true;
    }

    private static bool RemoveNuGetPackageReferenceFromPackageConfig(string projectFileName, string packageName)
    {
        var projectFolder = Path.GetDirectoryName(projectFileName);
        var packagesConfigFile = new DirectoryInfo(projectFolder ?? Environment.CurrentDirectory)
            .GetFiles("packages.config", SearchOption.TopDirectoryOnly)
            .FirstOrDefault()
            ?.FullName;

        if (packagesConfigFile is null)
        {
            return false;
        }

        WithNuGetPackageReferenceInPackagesConfig(projectFileName, packageName, (doc, x) =>
        {
            using var writer = new XmlTextWriter(packagesConfigFile, Encoding.UTF8);
            x.Remove();
            doc.Save(writer);
        });

        return true;
    }

    public static bool RemoveNuGetPackageReference(string projectFileName, string packageName) => RemoveNuGetPackageReferenceFromProject(projectFileName, packageName)
        && RemoveNuGetPackageReferenceFromPackageConfig(projectFileName, packageName);

    private static bool ContainsPackageReference(XDocument projectDocument, string packageName, string? packageVersion = default)
    {
        static string PackageReferenceName(XElement element) => element.Attribute("Include")?.Value ?? string.Empty;
        static string PackageReferenceVersion(XElement element) => element.Attribute("Version")?.Value ?? string.Empty;

        var references = projectDocument.Root
            ?.Descendants("PackageReference");

        return (references ?? Array.Empty<XElement>())
            .Any(x => PackageReferenceName(x).Equals(packageName, StringComparison.InvariantCultureIgnoreCase)
                && (string.IsNullOrEmpty(packageVersion) || PackageReferenceVersion(x).Equals(packageVersion, StringComparison.InvariantCultureIgnoreCase)));
    }

    private static bool ContainsNuGetReferenceVersion(string projectFileName, string packageName, string? packageVersion = default)
    {
        static string PackageName(XElement element) => element.Attribute("id")?.Value ?? string.Empty;
        static string PackageVersion(XElement element) => element.Attribute("version")?.Value ?? string.Empty;

        var projectFolder = Path.GetDirectoryName(projectFileName);
        var packagesConfigFile = new DirectoryInfo(projectFolder ?? Environment.CurrentDirectory)
            .GetFiles("packages.config", SearchOption.TopDirectoryOnly)
            .FirstOrDefault()
            ?.FullName;

        if (packagesConfigFile == null)
        {
            return false;
        }

        var doc = XDocument.Load(packagesConfigFile);
        var packages = doc.Root
            ?.Descendants("package");

        return (packages ?? Array.Empty<XElement>())
            .Any(x => PackageName(x).Equals(packageName, StringComparison.InvariantCultureIgnoreCase)
                && (string.IsNullOrEmpty(packageVersion) || PackageVersion(x).Equals(packageVersion, StringComparison.InvariantCultureIgnoreCase)));
    }

    public static bool ContainsPackage(string projectFileName, string packageName, string? packageVersion = default)
    {
        var doc = XDocument.Load(projectFileName);
        return IsPackageReferenceProject(doc)
            ? ContainsPackageReference(doc, packageName, packageVersion)
            : ContainsNuGetReferenceVersion(projectFileName, packageName, packageVersion);
    }

    public static void UpdatePackageReference(string projectFileName, string packageName, string packageVersion)
    {
        var doc = XDocument.Load(projectFileName);
        var element = doc.Root?.Descendants("ItemGroup")
            .Descendants("PackageReference")
            .FirstOrDefault(x => x.Attributes("Include").FirstOrDefault(y => y.Value.Equals(packageName, StringComparison.InvariantCultureIgnoreCase)) is not null);

        element?.SetAttributeValue("Include", packageName);
        element?.SetAttributeValue("Version", packageVersion);
    }

    public static void UpdatePackagesConfigReference(string packagesConfigFileName, string packageName, string packageVersion)
    {
        var doc = XDocument.Load(packagesConfigFileName);
        var element = doc.Root?.Descendants("package")
            .FirstOrDefault(x => x.Attributes("id").FirstOrDefault(y => y.Value.Equals(packageName, StringComparison.InvariantCultureIgnoreCase)) is not null);

        element?.SetAttributeValue("id", packageName);
        element?.SetAttributeValue("version", packageVersion);
    }
}