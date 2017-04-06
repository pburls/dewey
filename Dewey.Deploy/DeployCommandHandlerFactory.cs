using Dewey.Messaging;
using SimpleInjector;

namespace Dewey.Deploy
{
    class DeployCommandHandlerFactory : ICommandHandlerFactory<DeployCommand>
    {
        readonly Container _container;

        public DeployCommandHandlerFactory(Container container)
        {
            _container = container;
        }

        public ICommandHandler<DeployCommand> CreateHandler()
        {
            return _container.GetInstance<DeployCommandHandler>();
        }
    }
}
