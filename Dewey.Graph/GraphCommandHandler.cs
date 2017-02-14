using Dewey.Graph.DOT;
using Dewey.Graph.Writers;
using Dewey.Manifest.Dependency;
using Dewey.Messaging;
using Dewey.State;
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
        IEventHandler<GetRuntimeResourcesResult>,
        IEventHandler<DependencyElementResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;
        readonly IDependencyElementLoader _dependencyElementLoader;
        readonly IGraphGenerator _graphGenerator;
        readonly IGraphWriterFactory _graphWriterFactory;


        readonly List<DependencyElementResult> _dependencies = new List<DependencyElementResult>();

        readonly string[] LAYER_COMPONENT_TYPES = { QueueDependency.QUEUE_DEPENDENCY_TYPE, DependencyElementResult.FILE_DEPENDENCY_TYPE, DependencyElementResult.ENVIRONMENT_VARIABLE_DEPENDENCY_TYPE, DatabaseDependency.DATABASE_DEPENDENCY_TYPE };

        GraphCommand _command;
        IEnumerable<Component> _components;
        IReadOnlyDictionary<string, RuntimeResource> _runtimeResources;

        public GraphCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator, IDependencyElementLoader dependencyElementLoader, IGraphGenerator graphGenerator, IGraphWriterFactory graphWriterFactory)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;
            _dependencyElementLoader = dependencyElementLoader;
            _graphGenerator = graphGenerator;
            _graphWriterFactory = graphWriterFactory;

            eventAggregator.Subscribe<GetComponentsResult>(this);
            eventAggregator.Subscribe<GetRuntimeResourcesResult>(this);
            eventAggregator.Subscribe<DependencyElementResult>(this);
        }

        public void Execute(GraphCommand command)
        {
            _command = command;
            _eventAggregator.PublishEvent(new GenerateGraphStarted());

            var stopwatch = Stopwatch.StartNew();
            var result = Execute();
            stopwatch.Stop();
            
            _eventAggregator.PublishEvent(GenerateGraphResult.Create(command, stopwatch.Elapsed, result));
        }

        private WriteGraphResult Execute()
        {
            _commandProcessor.Execute(new GetComponents());
            _commandProcessor.Execute(new GetRuntimeResources());

            var nodeDictionary = new Dictionary<string, Node>();
            int nodeId = 1;
            var layerDictionary = new Dictionary<string, Cluster>();

            foreach (var component in _components)
            {
                string type;
                if (string.IsNullOrWhiteSpace(component.ComponentManifest.SubType))
                    type = string.Join("-", Node.COMPONENT_NODE_TYPE, component.ComponentManifest.Type);
                else
                    type = string.Join("-", Node.COMPONENT_NODE_TYPE, component.ComponentManifest.Type, component.ComponentManifest.SubType);

                nodeDictionary.Add(component.ComponentManifest.Name, new Node(nodeId++, component.ComponentManifest.Name, type));

                _dependencyElementLoader.LoadFromComponentManifest(component.ComponentManifest, component.ComponentElement);
            }

            foreach (var runtimeResource in _runtimeResources.Values)
            {
                var name = !string.IsNullOrWhiteSpace(runtimeResource.RuntimeResourceItem.Provider) ? string.Format("{0}\n{1}", runtimeResource.RuntimeResourceItem.Name, runtimeResource.RuntimeResourceItem.Provider) : runtimeResource.RuntimeResourceItem.Name;
                var node = new Node(nodeId++, name, runtimeResource.RuntimeResourceItem.Type);
                nodeDictionary.Add(runtimeResource.RuntimeResourceItem.Name, node);

                var layerName = runtimeResource.RuntimeResourceItem.Context;
                if (!string.IsNullOrWhiteSpace(layerName))
                {
                    Cluster layer = null;
                    if (!layerDictionary.TryGetValue(layerName, out layer))
                    {
                        layer = new Cluster(layerName);
                        layerDictionary.Add(layerName, layer);
                    }

                    layer.AddNodeId(node.Id);
                }
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
                else if (dependecy is RuntimeResourceDependency)
                {
                    var runtimeResourceDependency = dependecy as RuntimeResourceDependency;
                    Node node1, node2;
                    RuntimeResource runtimeResource;
                    if (nodeDictionary.TryGetValue(dependecy.ComponentManifest.Name, out node1) && nodeDictionary.TryGetValue(dependecy.Name, out node2))
                    {
                        string format = null;
                        if (_runtimeResources.TryGetValue(dependecy.Name, out runtimeResource))
                        {
                            format = runtimeResource.RuntimeResourceItem.Format;
                        }
                        edgeList.Add(new Edge(node1.Id, node2.Id, format));
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

            var graphDOTtext = _graphGenerator.GenerateDOTGraph(nodeDictionary.Values, edgeList, layerDictionary.Values);

            var graphWriter = _graphWriterFactory.CreateWriter(_command);

            return graphWriter.Write(graphDOTtext);
        }

        public void Handle(GetComponentsResult getComponentsResult)
        {
            _components = getComponentsResult.Components;
        }

        public void Handle(DependencyElementResult dependencyElementResult)
        {
            _dependencies.Add(dependencyElementResult);
        }

        public void Handle(GetRuntimeResourcesResult getRuntimeResourcesResult)
        {
            _runtimeResources = getRuntimeResourcesResult.RuntimeResources;
        }
    }
}
