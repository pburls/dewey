Dewey
===

[![Build status](https://ci.appveyor.com/api/projects/status/ac9jreo07s3eb405?svg=true)](https://ci.appveyor.com/project/pburls/dewey)

[![](https://codescene.io/projects/204/status.svg) Get more details at **codescene.io**.](https://codescene.io/projects/204/jobs/latest-successful/results)

- Introducing a standard for cataloguing software components throughout multiple repositories using simple xml manifest files.
- A simple command line application for reading the manifests and performing simple actions described by them. Such as building and deploying.

# Command Line application
The current trend in software design is to break up large, complicated software systems into a collection of many smaller, simpler components. e.g. micro services.  
The 'dewey' command line tool is designed to help alleviate some of the problems of working on a large system.  
By reading the necessary information from the appropriate set of manifest files, dewey can help perform more complex tasks in a simple instruction. Such as:
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
- The `list` command loads all discoverable manifests and lists the known components.
- The `build` command loads all discoverable manifests and attempts to build the component that matches the given name. All the component's dependencies can also be built.
- The `deploy` command loads all discoverable manifests and attempts to deploy the component that matches the given name. All the component's dependencies can also be built.
- The `graph` command loads all discoverable manifests and builds a dependency graph of all the known components and runtime resources. More information can be found [here](Dewey.Graph).

# Manifest File
Dewey manifest files `dewey.json` are used to capture the location of components in a repository and information required to perform different tasks.
The following root information can be captured:
- Location of other manifest files.
- Details about components.
- Details about runtime resources used by components.

## Examples
### Locations
When Dewey is run in a folder, it looks for a `dewey.json` file.
If the manifest contains locations to other manifest files under the root `manifestFiles` array field, Dewey will in turn try load them.
Each location item is required to have the following attributes:
* `name` - The name of the component for reference purposes.
* `location` - The relative path of a directory containing a manifest file.
```
{
  "manifestFiles": [
    { "name": "ExampleComp1", "location": "ExampleComp1/" },
    { "name": "ExampleComp2", "location": "ExampleComp2/" }
  ]
}
```
### Component
The root JSON object of manifest file can have a `components` array field.
Each component object in this array can be used to describe a component.
* `name` - A mandatory unique name of the component.
* `type` - A mandatory field describing the type of the component. e.g. executable, web.
* `subtype` - An option field describing in more detail the type of the component. e.g. serivce, worker.
* `context` - The context that the component relates to.
* `build` - A Build object describing how to build the component.
* `deploy` - A Deploy object describing how to deploy the component.
* `dependencies` - An array of Dependency objects.
```
{
"components": [{
  "name": "ExampleAgent",
  "type": "executable",
  "subtype": "worker",
  "context": "context1",
  "build": {
    "type": "msbuild",
    "target": "src/ExampleWebApiApplication/ExampleWebApiApplication.csproj",
    "msbuildVersion": "14.0"
  },
  "deploy": {
    "type": "iis",
    "port": "53971",
    "siteName": "ExampleWebApiApplication",
    "appPool": "ExampleWebApiApplication",
    "content": "src/ExampleWebApiApplication/"
  },
  "dependencies": [
    { "type": "component", "name": "ExampleComp2" },
    { "type": "runtimeResource", "name": "incoming" }
  ]
}
```  
### Runtime Resources
The root JSON object of manifest file can have a `runtimeResources` array field.
Each object in this array can be used to describe a Runtime Resource.
* `name` - A mandatory unique name of the runtime resource.
* `type` - A mandatory field describing the type of the runtime resource. e.g. file, queue, environment-variable, database, etc
* `provider` - An option field for the technology provider for the resource.
* `format` - An option field for the type of transport format. e.g. XML, JSON.
* `context` - The context that the component relates to.
```
{
  "runtimeResources": [{
    "type": "queue",
    "name": "incoming",
    "provider": "ActiveMQ",
    "format": "XML",
    "context": "context1"
  }, {
    "type": "environment-variable",
    "name": "queue-host",
    "context": "context1"
  }]
}
```
### Build
A JSON object that can fall under a Component to describe how to build the component.
* `type` - A mandatory field describing the type of build action supported by Dewey. e.g. msbuild
* `target` - A mandatory field giving the location of a file the build action will try use.
### Deploy
A JSON object that can fall under a Component to describe how to deploy the component.
* `type` - A mandatory field describing the type of deployment action supported by Dewey. e.g. iis
### Dependency
A JSON object that can fall under a Component to describe what dependency it has at runtime.
* `type` - A mandatory field describing the type of dependency, either another `component` or a `runtimeResource`.
* `name` - A mandatory unique name of the dependency that should already be described in a manifest file.
