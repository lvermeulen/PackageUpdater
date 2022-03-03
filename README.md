![Icon](https://i.imgur.com/tImwSPb.png)
# PackageTool 
[![Build status](https://ci.appveyor.com/api/projects/status/w6a2x8xqxwlyb1g0?svg=true)](https://ci.appveyor.com/project/lvermeulen/packageupdater)
[![license](https://img.shields.io/github/license/lvermeulen/PackageUpdater.svg?maxAge=2592000)](https://github.com/lvermeulen/PackageUpdater/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/vpre/PackageUpdater.svg?maxAge=2592000)](https://www.nuget.org/packages/PackageUpdater/)
![downloads](https://img.shields.io/nuget/dt/PackageUpdater)
![](https://img.shields.io/badge/.net-5.0-yellowgreen.svg)

A .NET tool for updating packages.

## Installation
*dotnet tool install --global PackageTool*


## Features

### * find-repositories

* Find all repositories under --path that use package in --name, using --strategy
* Usage: *packagetool find-repositories --name Newtonsoft.Json --path /c/all_my_repos --strategy DotNet*

### * update-package

* Update all repositories under --path that use package in --name with version in --version, using --strategy (with an optional --whatif parameter)
* Usage: *packagetool update-package --name Newtonsoft.Json --version 12.0.1 --path /c/all_my_repos --strategy DotNet --whatif*

## Parameters

### --name (-n)
The package's name.

### --version (-v)
The package's version.

### --path (-p)
The parent path of all repositories that you want to process.

### --strategy (-s)
The strategy to be used in looking for dependent repositories.

Currently supported are:
* *DotNet*: will look for Sdk-style projects in subfolders
* *DotNetFramework*: will look for packages.config files in subfolders
* *Paket*: will look for paket.dependencies files in subfolders

### --whatif (-w)
Shows the repositories that would be updated.

## Examples
* *packagetool find-repositories --name Newtonsoft.Json --path /c/all_my_repos --strategy Paket*
* *packagetool update-package --name Newtonsoft.Json --version 12.0.1 --path /c/all_my_repos --strategy DotNet --whatif*

## Thanks
* [Packaging](https://thenounproject.com/icon/packaging-4642069/) icon by [Ayub Irawan](https://thenounproject.com/ayub12/) from [The Noun Project](https://thenounproject.com)