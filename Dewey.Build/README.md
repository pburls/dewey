Build
===
The `dewey build <component-name> [-d]` command accepts the following arguments:
* `<component-name>` - The unique name of the component you wish to build.
* `[-d]` - Optional flags.
  * d - Builds the component and all of its dependencies and its dependencies' dependencies, etc.

# Build Manifest
A JSON object that can fall under a Component to describe how to build the component.
* `type` - A mandatory field describing the type of build action supported by Dewey

# Supported Build actions
## msbuild
The `msbuild` action uses Microsoft's MSBuild build tool to build Visual Studio solution of project files.
* `target` - A mandatory field giving the location of a file the Visual Studio solution or project file to be built.
* `msbuildVersion` - A mandatory field giving the version of the MSBuild tool that should be used. This needs to be installed with the correct DotNet framework.

# Example
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
  }
}
```
