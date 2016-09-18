using Dewey.Messaging;
using Dewey.State.Messages;

namespace Dewey.State
{
    public class Module : IModule
    {
        readonly Store _store;

        public Module(Store store, ICommandProcessor commandProcessor)
        {
            _store = store;
            commandProcessor.RegisterHandler<GetRepositoriesFiles, Store>();
            commandProcessor.RegisterHandler<GetRepositories, Store>();
            commandProcessor.RegisterHandler<GetComponents, Store>();
        }
    }
}
