using Dewey.Messaging;
using SimpleInjector;

namespace Dewey.Build
{
    public class Module : IModule
    {
        public Module(Container container, ICommandProcessor commandProcessor)
        {
            var writer = container.GetInstance<BuildCommandWriter>();

            commandProcessor.RegisterHandler<BuildCommand, BuildCommandHandler>();
        }
    }
}
