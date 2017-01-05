﻿using Dewey.Manifest.Dependency;
using Dewey.Messaging;
using Dewey.State.Messages;
using System;
using System.Collections.Generic;
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

        readonly List<DependencyElementResult> _dependencies = new List<DependencyElementResult>();

        public GraphCommandHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator, IDependencyElementLoader dependencyElementLoader)
        {
            _commandProcessor = commandProcessor;
            _eventAggregator = eventAggregator;
            _dependencyElementLoader = dependencyElementLoader;

            eventAggregator.Subscribe<GetComponentsResult>(this);
            eventAggregator.Subscribe<DependencyElementResult>(this);
        }

        public void Execute(GraphCommand command)
        {
            _commandProcessor.Execute(new GetComponents());
        }

        public void Handle(GetComponentsResult getComponentsResult)
        {
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
                        var name = string.Format("{0}:{1}", queueDepenedency.Provider, queueDepenedency.Name);
                        if (!nodeDictionary.TryGetValue(name, out queueNode))
                        {
                            queueNode = new Node(nodeId++, name, dependecy.Type);
                            nodeDictionary.Add(queueNode.Name, queueNode);
                        }

                        edgeList.Add(new Edge(componentNode.Id, queueNode.Id, queueDepenedency.Format));
                    }
                }
                else if (dependecy.Type == DependencyElementResult.FILE_DEPENDENCY_TYPE)
                {
                    Node componentNode, fileNode;
                    if (nodeDictionary.TryGetValue(dependecy.ComponentManifest.Name, out componentNode))
                    {
                        if (!nodeDictionary.TryGetValue(dependecy.Name, out fileNode))
                        {
                            fileNode = new Node(nodeId++, dependecy.Name, dependecy.Type);
                            nodeDictionary.Add(fileNode.Name, fileNode);
                        }

                        edgeList.Add(new Edge(componentNode.Id, fileNode.Id));
                    }
                }
            }

            var indexFileName = WriteGraphFiles(nodeDictionary.Values, edgeList, GenerateLayers(nodeDictionary.Values));

            System.Diagnostics.Process.Start(indexFileName);
        }

        public void Handle(DependencyElementResult dependencyElementResult)
        {
            _dependencies.Add(dependencyElementResult);
        }

        private IEnumerable<Layer> GenerateLayers(IEnumerable<Node> nodes)
        {
            string[] layerTypes = { QueueDependency.QUEUE_DEPENDENCY_TYPE, DependencyElementResult.FILE_DEPENDENCY_TYPE };

            var layerList = new List<Layer>();
            foreach (var layerType in layerTypes)
            {
                var nodesOfType = nodes.Where(x => x.Type == layerType);
                if (nodesOfType.Any())
                {
                    layerList.Add(new Layer(nodesOfType.Select(x => x.Id)));
                }
            }

            return layerList;
        }

        private string WriteGraphFiles(IEnumerable<Node> nodes, IEnumerable<Edge> edges, IEnumerable<Layer> layers)
        {
            //string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            //UriBuilder uri = new UriBuilder(codeBase);
            //string path = Uri.UnescapeDataString(uri.Path);
            //var assemblyPath = Path.GetDirectoryName(path);
            //var webPath = Path.Combine(assemblyPath, "web");
            //var webDirectoryInfo = new DirectoryInfo(webPath);

            //var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            //var graphDirectoryInfo = Directory.CreateDirectory(string.Format("Graph_{0}", timestamp));

            //foreach (var fileInfo in webDirectoryInfo.GetFiles())
            //{
            //    var outputFileName = Path.Combine(graphDirectoryInfo.FullName, fileInfo.Name);
            //    fileInfo.CopyTo(outputFileName, true);
            //}

            //var indexFileName = Path.Combine(graphDirectoryInfo.FullName, "index.html");
            //string nodeData = string.Join(",\n", nodes);
            //string edgeData = string.Join(",\n", edges);
            //string src = File.ReadAllText(indexFileName);
            //src = src.Replace("$nodes$", nodeData);
            //src = src.Replace("$edges$", edgeData);
            //File.WriteAllText(indexFileName, src);

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var graphFileName = string.Format("graph_{0}.txt", timestamp);

            string graphText = @"digraph G {
	$layers$
	$nodes$
	$edges$
}";

            string layerData = string.Join("\r\n\t", layers);
            string nodeData = string.Join("\r\n\t", nodes);
            string edgeData = string.Join("\r\n\t", edges);

            graphText = graphText.Replace("$layers$", layerData);
            graphText = graphText.Replace("$nodes$", nodeData);
            graphText = graphText.Replace("$edges$", edgeData);

            File.WriteAllText(graphFileName, graphText);

            return graphFileName;
        }
    }
}
