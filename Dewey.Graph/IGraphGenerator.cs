using Dewey.Graph.Writers;
using System.Collections.Generic;

namespace Dewey.Graph
{
    public interface IGraphGenerator
    {
        string GenerateDOTGraph(IEnumerable<Node> nodes, IEnumerable<Edge> edges, IEnumerable<Layer> layers);

        WriteGraphResult WritePNGGraph(string dotGraph);

        WriteGraphResult WriteDOTGraph(string dotGraph);
    }
}