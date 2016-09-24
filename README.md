Dewey
===

[![Build status](https://ci.appveyor.com/api/projects/status/ac9jreo07s3eb405?svg=true)](https://ci.appveyor.com/project/pburls/dewey)

- Introducing a standard for cataloguing software throughout multiple repositories using simple xml manifest files.
- A simple command line application for reading manifests and performing simple actions described by them. Such as building and deploying.

# Command Line application
The current trend in software design is to break up large, complicated software systems into a collection of many smaller, simpler components. e.g. micro services.
The 'dewey' command line tool is designed to help alleviate some of the problems of working on a large system.
By reading the necessary information from the appropriate set of manifest files, dewey can help perform more complex tasks in a simple instruction. Such as:
- Build a component.
- Deploy a component.

## Getting Started
Dewey is available as a [chocolatey](https://chocolatey.org/) package (latest package can be found [here](https://chocolatey.org/packages/dewey/)) and can be installed in a single command.
```
$ choco install dewey
```
The installation can be simply tested by typing 'dewey'. If successful, the version number and a list of available commands will be displayed.
```
$ dewey
Dewey Development Tool. v0.0.13
Usage: dewey <command>
Commands:
 - list
 - build
 - deploy
```

# Manifests
The following xml manifests files can be used to catalogue the components that form part of a software system:
- Component Manifest - Used to describe a software component that can be built and deployed.
- Repository Manifest - A manifest containing a listing of components inside a repository.
- Repositories Manifest - A manifest containing a list of repositories.

## Examples
### Repositories
A 'repositories.xml' file can be placed in a directory that is a common ancestor to the repositories it references.
As shown in the example below, the name and location of each required repository can be listed in the xml manifest file as a 'repository' xml element.
```
<?xml version="1.0" encoding="UTF-8"?>
<repositories>
	<repository name="repo1" location="repo/" />
	<repository name="repo2" location="repo2/" />
</repositories>
```
### Repository
A 'repository.xml' file can be placed in the root directory of a repository.
As shown in the example below, the name and location of each component stored in the repository can be listed in the xml manifest file as a 'component' xml element.
```
<?xml version="1.0" encoding="UTF-8"?>
<repository name="repo1">
	<components>
		<component name="ExampleComp1" location="ExampleComp1/" />
		<component name="ExampleComp2" location="ExampleComp2/" />
	</components>
</repository>
```
### Component
A 'component.xml' file can be placed in the root directory of a component.
An example manifest is shown below.
- The name and type of the component is set as attributes of the root 'componentManifest' element.
- Build actions for the component can be described using 'build' elements.
- Deployment actions for the component can be described using 'deployment' elements.
- Dependencies on other components can be described using 'dependency' elements.
```
<?xml version="1.0" encoding="UTF-8"?>
<componentManifest name="ExampleComp1" type="web">
	<builds>
		<build type="msbuild" target="src/ExampleComp1/ExampleComp1.csproj" msbuildVersion="14.0" />
	</builds>
	<deployments>
		<deployment type="iis" port="53971" siteName="ExampleApplication" appPool="ExampleApplication" content="src/ExampleApplication" />
	</deployments>
	<dependencies>
		<dependency type="component" name="ExampleComp2" />
	</dependencies>
</componentManifest>
```
