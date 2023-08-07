using System;

namespace PackageUpdater.Abstractions.CommandInput;

public class PathInput
{
    public string Path { get; set; } = Environment.CurrentDirectory;
}