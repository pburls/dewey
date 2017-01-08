using System.Collections.Generic;

namespace Dewey.Graph
{
    public class Layer
    {
        public IEnumerable<int> NodeIds { get; private set; }

        public Layer(IEnumerable<int> nodeIds)
        {
            NodeIds = nodeIds;
        }
    }
}
