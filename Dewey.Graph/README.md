Dewey Component Dependency Graph
===
The `dewey graph` command loads all the components and runtime resources that are discovered during start up.
## Generating a Graph
The graph is described using the [DOT graph description language](https://en.m.wikipedia.org/wiki/DOT_%28graph_description_language%29) and saved into the working directory as `graph.gv`.  
Given the `-r` command argument, dewey will attempt to render the graph using the GraphViz `dot.exe` dependency.
## Graph Elements
### Nodes
From the dependency information described in each component manifest, a dependency graph is built up.
Each node can be on of the following:
- A component.
- A runtime resources.

The type and sub-type of the component or runtime resource is used render an appropriate image in the node from a set of predefined icons listed in the [icons](icons) folder.

### Contexts
The context value set on components or runtime resources is used to group nodes into sub graphs.
