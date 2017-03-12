using Dewey.File;
using Dewey.Manifest.Component;
using Dewey.Manifest.Events;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
using Dewey.Messaging;
using System.Linq;
using System;

namespace Dewey.Manifest
{
    public class ManifestLoadHandler :
        IEventHandler<RepositoriesManifestLoadResult>,
        IEventHandler<RepositoryManifestLoadResult>,
        IEventHandler<ComponentManifestLoadResult>,
        IEventHandler<ManifestLoadResult>,
        IEventHandler<JsonManifestLoadResult>,
        ICommandHandler<LoadManifestFiles>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IManifestFileReaderService _manifestFileReaderService;

        public ManifestLoadHandler(IEventAggregator eventAggregator, IManifestFileReaderService manifestFileReaderService)
        {
            _eventAggregator = eventAggregator;
            _manifestFileReaderService = manifestFileReaderService;

            _eventAggregator.SubscribeAll(this);
        }

        public void Execute(LoadManifestFiles command)
        {
            _eventAggregator.PublishEvent(new LoadManifestFilesStarted());

            var manifestFileReader = _manifestFileReaderService.FindManifestFileInCurrentDirectory();
            if (manifestFileReader == null)
            {
                _eventAggregator.PublishEvent(new NoManifestFileFoundResult());
                return;
            }

            _eventAggregator.PublishEvent(new ManifestFilesFound(manifestFileReader.FileName));

            switch (manifestFileReader.MandifestFileType)
            {
                case ManifestFileType.Component:
                    var loadComponentManifestFileResult = ComponentManifest.LoadComponentManifestFile(manifestFileReader, null);
                    _eventAggregator.PublishEvent(loadComponentManifestFileResult);
                    break;
                case ManifestFileType.Repository:
                    var loadRepositoryManifestFileResult = DeweyManifestLoader.LoadDeweyManifest(manifestFileReader);
                    _eventAggregator.PublishEvent(loadRepositoryManifestFileResult);
                    break;
                case ManifestFileType.Repositories:
                    var loadRepositoriesManifestFileResult = DeweyManifestLoader.LoadDeweyManifest(manifestFileReader);
                    _eventAggregator.PublishEvent(loadRepositoriesManifestFileResult);
                    break;
                case ManifestFileType.Dewey:
                    var loadDeweyManifestResult = DeweyManifestLoader.LoadJsonDeweyManifest(manifestFileReader);
                    _eventAggregator.PublishEvent(loadDeweyManifestResult);
                    break;
                case ManifestFileType.Unknown:
                default:
                    break;
            }

            _eventAggregator.PublishEvent(new LoadManifestFilesResult());
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
                var componentManifestLoadResults = @event.RepositoryManifest.ComponentItems.Select(x => ComponentManifest.LoadComponentItem(x, _manifestFileReaderService));

                foreach (var componentManifestLoadResult in componentManifestLoadResults)
                {
                    _eventAggregator.PublishEvent(componentManifestLoadResult);
                }

                var runtimeResourceManifestLoadResults = @event.RepositoryManifest.RuntimeResourcesItems.Select(x => RuntimeResources.RuntimeResourcesManifestLoader.LoadManifestFileItem(x, _manifestFileReaderService));

                foreach (var runtimeResourceManifestLoadResult in runtimeResourceManifestLoadResults)
                {
                    _eventAggregator.PublishEvent(runtimeResourceManifestLoadResult);
                }
            }
        }

        public void Handle(ComponentManifestLoadResult @event)
        {

        }

        public void Handle(ManifestLoadResult @event)
        {
            if (@event.ManifestFilesElement != null)
            {
                DeweyManifestLoader.LoadManifestFilesElement(@event.ManifestFilesElement);
            }
        }

        public void Handle(JsonManifestLoadResult loadResult)
        {
            if (loadResult.Manifest.manifestFiles != null)
            {
                foreach (var manifestFile in loadResult.Manifest.manifestFiles)
                {
                    var manifestFileReader = _manifestFileReaderService.ReadDeweyManifestFile(loadResult.ManifestFile.DirectoryName, manifestFile.location);
                    var loadDeweyManifestResult = DeweyManifestLoader.LoadJsonDeweyManifest(manifestFileReader);
                    _eventAggregator.PublishEvent(loadDeweyManifestResult);
                }
            }
        }
    }
}
