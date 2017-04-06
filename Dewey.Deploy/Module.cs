using Dewey.Messaging;
using SimpleInjector;

namespace Dewey.Deploy
{
    public class Module : IModule
    {
        public Module(Container container, ICommandProcessor commandProcessor)
        {
            var writer = container.GetInstance<DeployCommandWriter>();
            var deployCommandHandlerFactory = container.GetInstance<DeployCommandHandlerFactory>();

            commandProcessor.RegisterHandlerFactory<DeployCommand, DeployCommandHandlerFactory>(deployCommandHandlerFactory);
        }
    }
}
