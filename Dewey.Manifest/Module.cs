using Dewey.File;
using Dewey.Manifest.Messages;
using Dewey.Messaging;
using SimpleInjector;

namespace Dewey.Manifest
{
    public class Module : IModule
    {
        readonly LoadManifestFilesWriter _writer;

        public Module(IEventAggregator eventAggregator, ICommandProcessor commandProcessor, Store store, IManifestFileReaderService manifestFileReaderService)
        {
            _writer = new LoadManifestFilesWriter(eventAggregator);

            var loadManifestFilesCommandHandlerFactory = new LoadManifestFilesCommandHandlerFactory(eventAggregator, manifestFileReaderService);
            commandProcessor.RegisterHandlerFactory<LoadManifestFiles, LoadManifestFilesCommandHandlerFactory>(loadManifestFilesCommandHandlerFactory);

            //todo: should be able to make a register all.
            var storeCommandHandlerFactory = new StoreCommandHandlerFactory(store);
            commandProcessor.RegisterHandlerFactory<GetComponent, StoreCommandHandlerFactory>(storeCommandHandlerFactory);
            commandProcessor.RegisterHandlerFactory<GetRuntimeResources, StoreCommandHandlerFactory>(storeCommandHandlerFactory);
            commandProcessor.RegisterHandlerFactory<GetComponents, StoreCommandHandlerFactory>(storeCommandHandlerFactory);
        }
    }
}
