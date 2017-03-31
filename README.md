[![Build status](https://ci.appveyor.com/api/projects/status/ac9jreo07s3eb405?svg=true)](https://ci.appveyor.com/project/pburls/dewey)

[![](https://codescene.io/projects/204/status.svg)](https://codescene.io/projects/204/jobs/latest-successful/results)
Dewey
===
- Introducing a standard for cataloguing software components throughout multiple repositories using simple JSON manifest files.
- A simple command line application for reading the manifests and performing simple actions described by them. Such as building, deploying and graphing dependencies.

# Command Line application
The current trend in software design is to break up large, complicated software systems into a collection of many smaller, simpler components. e.g. micro services.  
The `dewey` command line tool is designed to help alleviate some of the problems of working on a large system.  
By reading the necessary information from the appropriate set of manifest files, `dewey` can help perform more complex tasks in a simple instruction. Such as:
- Build a component.
- Deploy a component.
- Graph component dependencies.

## Getting Started
Dewey is available as a [chocolatey](https://chocolatey.org/) package (latest package can be found [here](https://chocolatey.org/packages/dewey/)) and can be installed in a single command.
```
$ choco install dewey
```
The installation can be simply tested by typing 'dewey'. If successful, the version number and a list of available commands will be displayed.
```
$ dewey
Dewey Development Tool. v0.0.60
Usage: dewey <command>
Commands:
 - list
 - build
 - deploy
 - graph
```

## Commands
- The `list` command loads all discoverable manifests and lists the known components and runtime-resources. More information can be found [here](Dewey.ListItems).
- The `build` command loads all discoverable manifests and attempts to build the component that matches the given name. All the component's dependencies can also be built. More information can be found [here](Dewey.Build).
- The `deploy` command loads all discoverable manifests and attempts to deploy the component that matches the given name. All the component's dependencies can also be built. More information can be found [here](Dewey.Deploy).
- The `graph` command loads all discoverable manifests and builds a dependency graph of all the known components and runtime resources. More information can be found [here](Dewey.Graph).

# Manifest File
Dewey manifest files `dewey.json` are used to capture the location of components in a repository and information required to perform different tasks.
The following root information can be captured:
- Location of other manifest files.
- Details about components.
- Details about runtime resources used by components.

More information and examples about Manifest Files can be found [here](Dewey.Manifest).
