using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Graph
{
    class Edge
    {
        public int From { get; private set; }
        public int To { get; private set; }
        public string Label { get; private set; }

        public Edge(int from, int to, string label)
        {
            From = from;
            To = to;
            Label = label;
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Label))
            {
                return string.Format("{{from: {0}, to: {1}, label:'{2}', font: {{align: 'horizontal'}}}}", From, To, Label);
            }
            return string.Format("{{from: {0}, to: {1}}}", From, To);
        }
    }
}
