using Dewey.Manifest.Dependency;
using Dewey.Messaging;
using Dewey.State.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dewey.Graph
{
    public class GraphCommandHandler :
        ICommandHandler<GraphCommand>,
        IEventHandler<GetComponentsResult>,
        IEventHandler<DependencyElementResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;
        readonly IDependencyElementLoader _dependencyElementLoader;
        readonly IGraphGenerator _graphGenerator;

        readonly List<DependencyElementResult> _dependencies = new List<DependencyElementResult>();

        readonly string[] LAYER_COMPONENT_TYPES = { QueueDependency.QUEUE_DEPENDENCY_TYPE, DependencyElementResult.FILE_DEPENDENCY_TYPE, DependencyElementResult.ENVIRONMENT_VARIABLE_DEPENDENCY_TYPE, DatabaseDependency.DATABASE_DEPENDENCY_TYPE };

        public GraphCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator, IDependencyElementLoader dependencyElementLoader, IGraphGenerator graphGenerator)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;
            _dependencyElementLoader = dependencyElementLoader;
            _graphGenerator = graphGenerator;

            eventAggregator.Subscribe<GetComponentsResult>(this);
            eventAggregator.Subscribe<DependencyElementResult>(this);
        }

        public void Execute(GraphCommand command)
        {
            _commandProcessor.Execute(new GetComponents());
        }

        public void Handle(GetComponentsResult getComponentsResult)
        {
            _eventAggregator.PublishEvent(new GenerateGraphStarted());

            var nodeDictionary = new Dictionary<string, Node>();
            int nodeId = 1;
            foreach (var component in getComponentsResult.Components)
            {
                string type;
                if (string.IsNullOrWhiteSpace(component.ComponentManifest.SubType))
                    type = string.Join("-", Node.COMPONENT_NODE_TYPE, component.ComponentManifest.Type);
                else
                    type = string.Join("-", Node.COMPONENT_NODE_TYPE, component.ComponentManifest.Type, component.ComponentManifest.SubType);

                nodeDictionary.Add(component.ComponentManifest.Name, new Node(nodeId++, component.ComponentManifest.Name, type));

                _dependencyElementLoader.LoadFromComponentManifest(component.ComponentManifest, component.ComponentElement);
            }

            var edgeList = new List<Edge>();
            foreach (var dependecy in _dependencies)
            {
                if (dependecy is ComponentDependency)
                {
                    var componentDependency = dependecy as ComponentDependency;
                    Node node1, node2;
                    if (nodeDictionary.TryGetValue(dependecy.ComponentManifest.Name, out node1) && nodeDictionary.TryGetValue(dependecy.Name, out node2))
                    {
                        edgeList.Add(new Edge(node1.Id, node2.Id, componentDependency.Protocol));
                    }
                }
                else if (dependecy is QueueDependency)
                {
                    var queueDepenedency = dependecy as QueueDependency;
                    Node componentNode, queueNode;
                    if (nodeDictionary.TryGetValue(dependecy.ComponentManifest.Name, out componentNode))
                    {
                        var name = string.Format("{0}\n{1}", queueDepenedency.Name, queueDepenedency.Provider);
                        if (!nodeDictionary.TryGetValue(name, out queueNode))
                        {
                            queueNode = new Node(nodeId++, name, dependecy.Type);
                            nodeDictionary.Add(queueNode.Name, queueNode);
                        }

                        edgeList.Add(new Edge(componentNode.Id, queueNode.Id, queueDepenedency.Format));
                    }
                }
                else if (dependecy is DatabaseDependency)
                {
                    var databaseDepenedency = dependecy as DatabaseDependency;
                    Node componentNode, databaseNode;
                    if (nodeDictionary.TryGetValue(dependecy.ComponentManifest.Name, out componentNode))
                    {
                        var name = string.Format("{0}\n{1}", databaseDepenedency.Name, databaseDepenedency.Provider);
                        if (!nodeDictionary.TryGetValue(name, out databaseNode))
                        {
                            databaseNode = new Node(nodeId++, name, dependecy.Type);
                            nodeDictionary.Add(databaseNode.Name, databaseNode);
                        }

                        edgeList.Add(new Edge(componentNode.Id, databaseNode.Id));
                    }
                }
                else
                {
                    Node componentNode, node;
                    if (nodeDictionary.TryGetValue(dependecy.ComponentManifest.Name, out componentNode))
                    {
                        if (!nodeDictionary.TryGetValue(dependecy.Name, out node))
                        {
                            node = new Node(nodeId++, dependecy.Name, dependecy.Type);
                            nodeDictionary.Add(node.Name, node);
                        }

                        edgeList.Add(new Edge(componentNode.Id, node.Id));
                    }
                }
            }

            var layerList = new List<Layer>();
            foreach (var layerType in LAYER_COMPONENT_TYPES)
            {
                var nodesOfType = nodeDictionary.Values.Where(x => x.Type == layerType);
                if (nodesOfType.Any())
                {
                    layerList.Add(new Layer(nodesOfType.Select(x => x.Id)));
                }
            }

            var result = _graphGenerator.GenerateGraph(nodeDictionary.Values, edgeList, layerList);

            _eventAggregator.PublishEvent(result);
        }

        public void Handle(DependencyElementResult dependencyElementResult)
        {
            _dependencies.Add(dependencyElementResult);
        }
    }
}
