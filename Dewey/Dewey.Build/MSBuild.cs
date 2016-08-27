using Dewey.Build.Events;
using Dewey.File;
using Dewey.Manifest.Component;
using Dewey.Messaging;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System;

namespace Dewey.Build
{
    public class MSBuild : IBuildAction
    {
        readonly IEventAggregator _eventAggregator;
        readonly IFileService _fileService;
        readonly IMSBuildProcess _msBuildProcess;

        public const string BUILD_TYPE = "msbuild";

        public string BuildType
        {
            get
            {
                return BUILD_TYPE;
            }
        }

        public MSBuild(IEventAggregator eventAggregator, IFileService fileService, IMSBuildProcess msBuildProcess)
        {
            _eventAggregator = eventAggregator;
            _fileService = fileService;
            _msBuildProcess = msBuildProcess;
        }

        public void Build(ComponentManifest componentManifest, XElement buildElement)
        {
            var buildArgs = MSBuildArgs.ParseMSBuildElement(buildElement);

            if (buildArgs.MissingAttributes.Any())
            {
                _eventAggregator.PublishEvent(new BuildElementMissingAttributeResult(componentManifest, BUILD_TYPE, buildElement, string.Join(", ", buildArgs.MissingAttributes)));
                return;
            }

            string buildTargetPath = _fileService.CombinePaths(componentManifest.File.DirectoryName, buildArgs.BuildTarget);
            if (!_fileService.FileExists(buildTargetPath))
            {
                _eventAggregator.PublishEvent(new BuildActionTargetNotFoundResult(componentManifest, BUILD_TYPE, buildTargetPath));
                return;
            }

            _eventAggregator.PublishEvent(new BuildActionStarted(componentManifest, BUILD_TYPE, buildArgs));

            _msBuildProcess.Execute(buildTargetPath);

            _eventAggregator.PublishEvent(new BuildActionCompletedResult(componentManifest, BUILD_TYPE, buildArgs));
        }
    }
}
