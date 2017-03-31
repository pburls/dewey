Manifest File
===
Dewey manifest files `dewey.json` are used to capture the location of components in a repository and information required to perform different tasks.  
The following root information can be captured:
- Location of other manifest files.
- Details about components.
- Details about runtime resources used by components.

# Locations
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

# Component
The root JSON object of manifest file can have a `components` array field.
Each component object in this array can be used to describe a component.
* `name` - A mandatory unique name of the component.
* `type` - A mandatory field describing the type of the component. e.g. executable, web.
* `subtype` - An option field describing in more detail the type of the component. e.g. serivce, worker.
* `context` - The context that the component relates to.
* `build` - A Build object describing how to build the component. More information can be found [here](Dewey.Build).
* `deploy` - A Deploy object describing how to deploy the component. More information can be found [here](Dewey.Deploy).
* `dependencies` - An array of Dependency objects.
```
{
"components": [{
  "name": "ExampleWebApiApplication",
  "type": "web",
  "subtype": "service",
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

# Runtime Resources
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

# Dependency
A JSON object that can fall under a Component to describe what dependency it has at runtime.
* `type` - A mandatory field describing the type of dependency, either another `component` or a `runtimeResource`.
* `name` - A mandatory unique name of the dependency that should already be described in a manifest file.
