using System.Collections.Generic;

namespace Dewey.Graph
{
    public class Layer
    {
        private List<int> _nodeIdList = new List<int>();

        public string Name { get; private set; }

        public IEnumerable<int> NodeIds
        {
            get
            {
                return _nodeIdList;
            }
        }

        public Layer(string name)
        {
            Name = name;

            _nodeIdList = new List<int>();
        }

        public void AddNodeId(int nodeId)
        {
            _nodeIdList.Add(nodeId);
        }
    }
}
