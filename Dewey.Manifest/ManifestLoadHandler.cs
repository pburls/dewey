﻿using Ark3.Command;
using Ark3.Event;
using Dewey.File;
using Dewey.Manifest.Events;

namespace Dewey.Manifest
{
    public class ManifestLoadHandler :
        IEventHandler<JsonManifestLoadResult>,
        ICommandHandler<LoadManifestFiles>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IManifestFileReaderService _manifestFileReaderService;

        public ManifestLoadHandler(IEventAggregator eventAggregator, IManifestFileReaderService manifestFileReaderService)
        {
            _eventAggregator = eventAggregator;
            _manifestFileReaderService = manifestFileReaderService;
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

            _eventAggregator.SubscribeAll(this);
            _eventAggregator.PublishEvent(new ManifestFilesFound(manifestFileReader.FileName));

            switch (manifestFileReader.MandifestFileType)
            {
                case ManifestFileType.Dewey:
                    var loadDeweyManifestResult = DeweyManifestLoader.LoadJsonDeweyManifest(manifestFileReader);
                    _eventAggregator.PublishEvent(loadDeweyManifestResult);
                    break;
                case ManifestFileType.Unknown:
                default:
                    break;
            }

            _eventAggregator.PublishEvent(new LoadManifestFilesResult());
            _eventAggregator.UnsubscribeAll(this);
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
