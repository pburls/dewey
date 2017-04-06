using Dewey.Messaging;
using SimpleInjector;

namespace Dewey.Build
{
    public class Module : IModule
    {
        public Module(Container container, ICommandProcessor commandProcessor)
        {
            var writer = container.GetInstance<BuildCommandWriter>();
            var buildCommandHandlerFactory = container.GetInstance<BuildCommandHandlerFactory>();

            commandProcessor.RegisterHandlerFactory<BuildCommand, BuildCommandHandlerFactory>(buildCommandHandlerFactory);
        }
    }
}
