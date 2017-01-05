using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Graph
{
    class Layer
    {
        public IEnumerable<int> NodeIds { get; private set; }

        public Layer(IEnumerable<int> nodeIds)
        {
            NodeIds = nodeIds;
        }

        public override string ToString()
        {
            var nodeIdList = string.Join(" ", NodeIds);
            return string.Format("{{rank=same; {0}}}", nodeIdList);
        }
    }
}
