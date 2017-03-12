using Dewey.Messaging;
using SimpleInjector;

namespace Dewey.Manifest
{
    public class Module : IModule
    {
        readonly Store _store;
        readonly LoadManifestFilesWriter _writer;

        public Module(IEventAggregator eventAggregator, ICommandProcessor commandProcessor, Store store)
        {
            _writer = new LoadManifestFilesWriter(eventAggregator);
            _store = store;

            commandProcessor.RegisterHandler<LoadManifestFiles, ManifestLoadHandler>();
        }
    }
}
