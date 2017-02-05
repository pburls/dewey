using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dewey.Graph
{
    public class GraphViz : IGraphGenerator
    {
        const string GRAPH_VIZ_PATH = @"C:\Program Files (x86)\Graphviz2.38\bin\dot.exe";

        private string _iconsPath;

        public GraphViz()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var assemblyPath = Path.GetDirectoryName(path);
            _iconsPath = Path.Combine(assemblyPath, "icons");
        }

        public string GenerateDOTGraph(IEnumerable<Node> nodes, IEnumerable<Edge> edges, IEnumerable<Layer> layers)
        {
            string graphText = @"digraph G {
	$layers$
	$nodes$
	$edges$
}";

            string layerData = string.Join("\r\n\t", layers.Select(WriteLayer));
            string nodeData = string.Join("\r\n\t", nodes.Select(WriteNode));
            string edgeData = string.Join("\r\n\t", edges.Select(WriteEdge));

            graphText = graphText.Replace("$layers$", layerData);
            graphText = graphText.Replace("$nodes$", nodeData);
            graphText = graphText.Replace("$edges$", edgeData);

            return graphText;
        }

        public WriteGraphResult WritePNGGraph(string dotGraph)
        {
            if (!File.Exists(GRAPH_VIZ_PATH))
            {
                return new WriteGraphResult(null, string.Format("GraphViz not found at path '{0}'.", GRAPH_VIZ_PATH));
            }

            var graphFileName = "graph.png";
            
            ProcessStartInfo startInfo = new ProcessStartInfo(GRAPH_VIZ_PATH);
            startInfo.Arguments = "-Tpng -o " + graphFileName;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;

            var process = Process.Start(startInfo);

            using (var stdIn = process.StandardInput)
            {
                stdIn.WriteLine(dotGraph);
            }

            process.WaitForExit();

            var graphFileInfo = new FileInfo(graphFileName);
            if (process.ExitCode == 0)
            {
                return new WriteGraphResult(graphFileInfo.FullName, null);
            }

            return new WriteGraphResult(graphFileInfo.FullName, string.Format("GraphViz process exited with existed with error code '{0}'.", process.ExitCode));
        }

        public WriteGraphResult WriteDOTGraph(string dotGraph)
        {
            var graphFileName = "graph.gv";
            File.WriteAllText(graphFileName, dotGraph);
            var graphFileInfo = new FileInfo(graphFileName);
            return new WriteGraphResult(graphFileInfo.FullName, null);
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

        private string WriteLayer(Layer layer)
        {
            return string.Format("{{rank=same; {0}}}", string.Join(" ", layer.NodeIds));
        }
    }
}
