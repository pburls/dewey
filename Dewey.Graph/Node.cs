namespace Dewey.Graph
{
    class Node
    {
        public const string COMPONENT_NODE_TYPE = "component";

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }

        public Node(int id, string name, string type)
        {
            Id = id;
            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            return string.Format("{{id: {0}, label: '{1}', group: '{2}'}}", Id, Name, Type);
        }
    }
}
