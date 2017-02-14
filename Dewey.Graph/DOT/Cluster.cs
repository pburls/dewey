using System.Collections.Generic;

namespace Dewey.Graph.DOT
{
    public class Cluster
    {
        private List<Node> _nodeList;

        public string Name { get; private set; }

        public IEnumerable<Node> Nodes
        {
            get
            {
                return _nodeList;
            }
        }

        public Cluster(string name)
        {
            Name = name;

            _nodeList = new List<Node>();
        }

        public void AddNode(Node node)
        {
            _nodeList.Add(node);
        }
    }
}
