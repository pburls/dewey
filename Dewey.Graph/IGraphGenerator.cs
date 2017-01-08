using System.Collections.Generic;

namespace Dewey.Graph
{
    public interface IGraphGenerator
    {
        GenerateGraphResult GenerateGraph(IEnumerable<Node> nodes, IEnumerable<Edge> edges, IEnumerable<Layer> layers);
    }
}