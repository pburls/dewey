Deploy
===
The `dewey deploy <component-name> [-d]` command accepts the following arguments:
* `<component-name>` - The unique name of the component you wish to deploy.
* `[-d]` - Optional flags.
  * d - Deploys the component and all of its dependencies and its dependencies' dependencies, etc.

# Deployment Manifest
A JSON object that can fall under a Component to describe how to deploy the component.
* `type` - A mandatory field describing the type of deployment action supported by Dewey.

# Supported Deployment actions
## iis
The `iis` action deploys the component as a Web Application on the local instance of Microsoft's IIS WebHost. The following parameters are used:
* `port` - A mandatory field with an integer value for the port on which the Web Application is bound.
* `siteName` - A mandatory field giving the unique name the Web Application is hosted under in IIS.
* `appPool` - A mandatory field giving the unique name the application pool Web Application is hosted under in IIS.
* `content` - A mandatory field giving the location to the built Web Application files required to host the application.

# Example
```
{
"components": [{
  "name": "ExampleWebApiApplication",
  "type": "web",
  "subtype": "service",
  "context": "context1",
  "deploy": {
    "type": "iis",
    "port": "53971",
    "siteName": "ExampleWebApiApplication",
    "appPool": "ExampleWebApiApplication",
    "content": "src/ExampleWebApiApplication/"
  }
}
```
