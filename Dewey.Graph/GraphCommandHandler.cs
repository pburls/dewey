using Dewey.Graph.DOT;
using Dewey.Graph.Models;
using Dewey.Graph.Writers;
using Dewey.Manifest.Messages;
using Dewey.Manifest.Models;
using Dewey.Messaging;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dewey.Graph
{
    public class GraphCommandHandler :
        ICommandHandler<GraphCommand>,
        IEventHandler<GetComponentsResult>,
        IEventHandler<GetRuntimeResourcesResult>
    {
        readonly ICommandProcessor _commandProcessor;
        readonly IEventAggregator _eventAggregator;
        readonly IGraphGenerator _graphGenerator;
        readonly IGraphWriterFactory _graphWriterFactory;

        GraphCommand _command;
        IEnumerable<Component> _components;
        IReadOnlyDictionary<string, RuntimeResource> _runtimeResources;

        public GraphCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator, IGraphGenerator graphGenerator, IGraphWriterFactory graphWriterFactory)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;
            _graphGenerator = graphGenerator;
            _graphWriterFactory = graphWriterFactory;

            eventAggregator.SubscribeAll(this);
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
            var clusterDictionary = new Dictionary<string, Cluster>();
            var unclusteredNodes = new List<Node>();

            foreach (var component in _components)
            {
                string type;
                if (string.IsNullOrWhiteSpace(component.subtype))
                    type = string.Join("-", Node.COMPONENT_NODE_TYPE, component.type);
                else
                    type = string.Join("-", Node.COMPONENT_NODE_TYPE, component.type, component.subtype);

                var node = new Node(nodeId++, component.name, type);
                nodeDictionary.Add(component.name, node);

                var clusterName = component.context;
                if (!string.IsNullOrWhiteSpace(clusterName))
                {
                    Cluster layer = null;
                    if (!clusterDictionary.TryGetValue(clusterName, out layer))
                    {
                        layer = new Cluster(clusterName);
                        clusterDictionary.Add(clusterName, layer);
                    }

                    layer.AddNode(node);
                }
                else
                {
                    unclusteredNodes.Add(node);
                }
            }

            foreach (var runtimeResource in _runtimeResources.Values)
            {
                var name = !string.IsNullOrWhiteSpace(runtimeResource.provider) ? string.Format("{0}\n{1}", runtimeResource.name, runtimeResource.provider) : runtimeResource.name;
                var node = new Node(nodeId++, name, runtimeResource.type);
                nodeDictionary.Add(runtimeResource.name, node);

                var clusterName = runtimeResource.context;
                if (!string.IsNullOrWhiteSpace(clusterName))
                {
                    Cluster layer = null;
                    if (!clusterDictionary.TryGetValue(clusterName, out layer))
                    {
                        layer = new Cluster(clusterName);
                        clusterDictionary.Add(clusterName, layer);
                    }

                    layer.AddNode(node);
                }
                else
                {
                    unclusteredNodes.Add(node);
                }
            }

            var edgeList = new List<Edge>();
            foreach (var component in _components)
            {
                foreach (var dependecy in component.dependencies)
                {
                    if (dependecy.IsComponentDependency())
                    {
                        var componentDependency = new ComponentDependency(dependecy);
                        Node node1, node2;
                        if (nodeDictionary.TryGetValue(component.name, out node1) && nodeDictionary.TryGetValue(componentDependency.name, out node2))
                        {
                            edgeList.Add(new Edge(node1.Id, node2.Id, componentDependency.protocol));
                        }
                    }
                    else if (dependecy.IsRuntimeResourceDependency())
                    {
                        Node node1, node2;
                        RuntimeResource runtimeResource;
                        if (nodeDictionary.TryGetValue(component.name, out node1) && nodeDictionary.TryGetValue(dependecy.name, out node2))
                        {
                            string format = null;
                            if (_runtimeResources.TryGetValue(dependecy.name, out runtimeResource))
                            {
                                format = runtimeResource.format;
                            }
                            edgeList.Add(new Edge(node1.Id, node2.Id, format));
                        }
                    }
                    else
                    {
                        Node componentNode, node;
                        if (nodeDictionary.TryGetValue(component.name, out componentNode))
                        {
                            if (!nodeDictionary.TryGetValue(dependecy.name, out node))
                            {
                                node = new Node(nodeId++, dependecy.name, dependecy.type);
                                nodeDictionary.Add(node.Name, node);
                            }

                            edgeList.Add(new Edge(componentNode.Id, node.Id));
                        }
                    }
                }
            }

            var graphDOTtext = _graphGenerator.GenerateDOTGraph(unclusteredNodes, edgeList, clusterDictionary.Values);

            var graphWriter = _graphWriterFactory.CreateWriter(_command);

            return graphWriter.Write(graphDOTtext);
        }

        public void Handle(GetComponentsResult getComponentsResult)
        {
            _components = getComponentsResult.Components;
        }

        public void Handle(GetRuntimeResourcesResult getRuntimeResourcesResult)
        {
            _runtimeResources = getRuntimeResourcesResult.RuntimeResources;
        }
    }
}
