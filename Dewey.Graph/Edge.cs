using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Graph
{
    class Edge
    {
        public int Id1 { get; private set; }
        public int Id2 { get; private set; }

        public Edge(int id1, int id2)
        {
            Id1 = id1;
            Id2 = id2;
        }

        public override string ToString()
        {
            return string.Format("{{from: {0}, to: {1}}}", Id1, Id2);
        }
    }
}
