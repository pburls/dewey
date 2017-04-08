using Ark3.Command;
using Ark3.Event;
using Dewey.File;
using Dewey.Manifest.Messages;
using Dewey.Messaging;

namespace Dewey.Manifest
{
    public class Module : IModule
    {
        readonly LoadManifestFilesWriter _writer;

        public Module(IEventAggregator eventAggregator, ICommandProcessor commandProcessor, Store store, IManifestFileReaderService manifestFileReaderService)
        {
            _writer = new LoadManifestFilesWriter(eventAggregator);

            var loadManifestFilesCommandHandlerFactory = new LoadManifestFilesCommandHandlerFactory(eventAggregator, manifestFileReaderService);
            commandProcessor.RegisterHandlerFactory(loadManifestFilesCommandHandlerFactory);

            //todo: should be able to make a register all.
            var storeCommandHandlerFactory = new StoreCommandHandlerFactory(store);
            commandProcessor.RegisterHandlerFactory<GetComponent>(storeCommandHandlerFactory);
            commandProcessor.RegisterHandlerFactory<GetRuntimeResources>(storeCommandHandlerFactory);
            commandProcessor.RegisterHandlerFactory<GetComponents>(storeCommandHandlerFactory);
        }
    }
}
