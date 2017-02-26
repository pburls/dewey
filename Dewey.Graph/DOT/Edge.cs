namespace Dewey.Graph.DOT
{
    public class Edge
    {
        public int From { get; private set; }
        public int To { get; private set; }
        public string Label { get; private set; }

        public Edge(int from, int to) : 
            this(from, to, string.Empty)
        {

        }

        public Edge(int from, int to, string label)
        {
            From = from;
            To = to;
            Label = label;
        }
    }
}
