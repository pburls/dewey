using Dewey.File;
using Dewey.Manifest.Component;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
using Dewey.Messaging;
using System.Linq;

namespace Dewey.Manifest
{
    public class ManifestLoadHandler : IEventHandler<RepositoriesManifestLoadResult>, IEventHandler<RepositoryManifestLoadResult>, IEventHandler<ComponentManifestLoadResult>, ICommandHandler<LoadManifestFiles>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IManifestFileReaderService _manifestFileReaderService;

        public ManifestLoadHandler(ICommandProcessor commandProcessor, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _manifestFileReaderService = new ManifestFileReaderService();

            _eventAggregator.Subscribe<RepositoriesManifestLoadResult>(this);
            _eventAggregator.Subscribe<ComponentManifestLoadResult>(this);
            _eventAggregator.Subscribe<RepositoryManifestLoadResult>(this);
        }

        public void Execute(LoadManifestFiles command)
        {
            _eventAggregator.PublishEvent(new LoadManifestFilesStarted());

            var loadRepositoriesManifestFileResult = RepositoriesManifest.LoadRepositoriesManifestFile(_manifestFileReaderService);
            _eventAggregator.PublishEvent(loadRepositoriesManifestFileResult);
        }

        public void Handle(RepositoriesManifestLoadResult @event)
        {
            if (@event.IsSuccessful)
            {
                var repositoryManifestLoadResults = @event.RepositoriesManifest.RepositoryItems.Select(x => RepositoryManifest.LoadRepositoryItem(x, _manifestFileReaderService));

                foreach (var repositoryManifestLoadResult in repositoryManifestLoadResults)
                {
                    _eventAggregator.PublishEvent(repositoryManifestLoadResult);
                }
            }
        }

        public void Handle(RepositoryManifestLoadResult @event)
        {
            if (@event.IsSuccessful)
            {
                var repositoryManifestLoadResults = @event.RepositoryManifest.ComponentItems.Select(x => ComponentManifest.LoadComponentItem(x, _manifestFileReaderService));

                foreach (var repositoryManifestLoadResult in repositoryManifestLoadResults)
                {
                    _eventAggregator.PublishEvent(repositoryManifestLoadResult);
                }
            }
        }

        public void Handle(ComponentManifestLoadResult @event)
        {
        }
    }
}
