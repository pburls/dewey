using Ark3.Command;
using SimpleInjector;

namespace Dewey.Build
{
    class BuildCommandHandlerFactory : ICommandHandlerFactory<BuildCommand>
    {
        readonly Container _container;

        public BuildCommandHandlerFactory(Container container)
        {
            _container = container;
        }

        public ICommandHandler<BuildCommand> CreateHandler()
        {
            return _container.GetInstance<BuildCommandHandler>();
        }
    }
}
