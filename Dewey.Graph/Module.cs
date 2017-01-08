using Dewey.Messaging;
using SimpleInjector;

namespace Dewey.Graph
{
    public class Module : IModule
    {
        public Module(Container container, ICommandProcessor commandProcessor)
        {
            var writer = container.GetInstance<GraphCommandWriter>();

            commandProcessor.RegisterHandler<GraphCommand, GraphCommandHandler>();
        }
    }
}
