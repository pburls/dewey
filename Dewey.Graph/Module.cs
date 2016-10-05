using Dewey.Messaging;

namespace Dewey.Graph
{
    public class Module : IModule
    {
        public Module(ICommandProcessor commandProcessor)
        {
            commandProcessor.RegisterHandler<GraphCommand, GraphCommandHandler>();
        }
    }
}
