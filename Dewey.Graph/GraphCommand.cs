using Dewey.Messaging;

namespace Dewey.Graph
{
    public class GraphCommand : ICommand
    {
        public const string COMMAND_TEXT = "graph";

        public static GraphCommand Create(string[] args)
        {
            return new GraphCommand();
        }
    }
}
