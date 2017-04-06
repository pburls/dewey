using Dewey.File;
using Dewey.Messaging;

namespace Dewey.Manifest
{
    class LoadManifestFilesCommandHandlerFactory : ICommandHandlerFactory<LoadManifestFiles>
    {
        readonly IEventAggregator _eventAggregator;
        readonly IManifestFileReaderService _manifestFileReaderService;

        public LoadManifestFilesCommandHandlerFactory(IEventAggregator eventAggregator, IManifestFileReaderService manifestFileReaderService)
        {
            _eventAggregator = eventAggregator;
            _manifestFileReaderService = manifestFileReaderService;
        }

        public ICommandHandler<LoadManifestFiles> CreateHandler()
        {
            return new ManifestLoadHandler(_eventAggregator, _manifestFileReaderService);
        }
    }
}
