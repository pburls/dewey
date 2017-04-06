using Dewey.Manifest.Messages;
using Dewey.Messaging;
using System;

namespace Dewey.Manifest
{
    class StoreCommandHandlerFactory : 
        ICommandHandlerFactory<GetComponent>,
        ICommandHandlerFactory<GetRuntimeResources>,
        ICommandHandlerFactory<GetComponents>
    {
        readonly Store _store;

        public StoreCommandHandlerFactory(Store store)
        {
            _store = store;
        }

        ICommandHandler<GetComponent> ICommandHandlerFactory<GetComponent>.CreateHandler()
        {
            return _store;
        }

        ICommandHandler<GetComponents> ICommandHandlerFactory<GetComponents>.CreateHandler()
        {
            return _store;
        }

        ICommandHandler<GetRuntimeResources> ICommandHandlerFactory<GetRuntimeResources>.CreateHandler()
        {
            return _store;
        }
    }
}
