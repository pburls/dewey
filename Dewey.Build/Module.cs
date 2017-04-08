using Dewey.Messaging;
using Ark3.Command;
using SimpleInjector;

namespace Dewey.Build
{
    public class Module : IModule
    {
        public Module(Container container, ICommandProcessor commandProcessor)
        {
            var writer = container.GetInstance<BuildCommandWriter>();
            var buildCommandHandlerFactory = container.GetInstance<BuildCommandHandlerFactory>();

            commandProcessor.RegisterHandlerFactory(buildCommandHandlerFactory);
        }
    }
}
