using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dewey.Graph.DOT
{
    public class GraphViz : IGraphGenerator
    {
        private string _iconsPath;

        public GraphViz()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var assemblyPath = Path.GetDirectoryName(path);
            _iconsPath = Path.Combine(assemblyPath, "icons");
        }

        public string GenerateDOTGraph(IEnumerable<Node> nodes, IEnumerable<Edge> edges, IEnumerable<Cluster> clusters)
        {
            string graphText = @"digraph G {
	$clusters$
	$nodes$
	$edges$
}";

            string clusterText = string.Join("\r\n\t", clusters.Select(WriteCluster));
            string nodeData = string.Join("\r\n\t", nodes.Select(WriteNode));
            string edgeData = string.Join("\r\n\t", edges.Select(WriteEdge));

            graphText = graphText.Replace("$clusters$", clusterText);
            graphText = graphText.Replace("$nodes$", nodeData);
            graphText = graphText.Replace("$edges$", edgeData);

            return graphText;
        }

        private string WriteNode(Node node)
        {
            var imageFileName = string.Format("{0}.png", node.Type);
            var imagePath = Path.Combine(_iconsPath, imageFileName);
            return string.Format("{0} [label=\"{1}\",image=\"{2}\",labelloc=\"b\",shape=box];", node.Id, node.Name, imagePath);
        }

        private string WriteEdge(Edge edge)
        {
            var attributes = new List<string>();
            if (!string.IsNullOrWhiteSpace(edge.Label))
            {
                attributes.Add(string.Format("label=\"{0}\"", edge.Label));
            }

            var text = string.Format("{0} -> {1}", edge.From, edge.To);

            if (attributes.Count > 0)
            {
                text = string.Format("{0} [{1}]", text, string.Join(",", attributes));
            }

            return text + ";";
        }

        private string WriteCluster(Cluster cluster)
        {
            string newline = "\r\n\t\t";
            var nodeText = string.Join(newline, cluster.Nodes.Select(WriteNode));
            return $"subgraph cluster_{cluster.Name} {{{newline}label=\"{cluster.Name}\"{newline}{nodeText}{newline}}}";
        }
    }
}
