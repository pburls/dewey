using Dewey.Build.Events;
using Dewey.File;
using System.Linq;
using Dewey.Build.Models;
using Dewey.Manifest.Models;
using Ark3.Event;

namespace Dewey.Build
{
    public class MSBuildAction : IBuildAction
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

        public MSBuildAction(IEventAggregator eventAggregator, IFileService fileService, IMSBuildProcess msBuildProcess)
        {
            _eventAggregator = eventAggregator;
            _fileService = fileService;
            _msBuildProcess = msBuildProcess;
        }

        public bool Build(Component componentManifest, Models.Build build)
        {
            var msBuild = new MSBuild(build);
            var missingAttributes = msBuild.GetMissingAttributes();
            if (missingAttributes.Any())
            {
                _eventAggregator.PublishEvent(new JsonBuildMissingAttributesResult(componentManifest, build, missingAttributes));
                return false;
            }

            string buildTargetPath = _fileService.CombinePaths(componentManifest.File.DirectoryName, msBuild.target);
            if (!_fileService.FileExists(buildTargetPath))
            {
                _eventAggregator.PublishEvent(new JsonBuildActionTargetNotFoundResult(componentManifest, build, buildTargetPath));
                return false;
            }

            string msbuildExecutablePath = _msBuildProcess.GetMSBuildExecutablePathForVersion(msBuild.msbuildVersion);
            if (string.IsNullOrEmpty(msbuildExecutablePath))
            {
                _eventAggregator.PublishEvent(new JsonMSBuildExecutableNotFoundResult(componentManifest, build, msBuild.msbuildVersion));
                return false;
            }

            _eventAggregator.PublishEvent(new JsonBuildActionStarted(componentManifest, msBuild));

            var result = _msBuildProcess.Execute(msbuildExecutablePath, buildTargetPath);

            _eventAggregator.PublishEvent(new JsonBuildActionCompletedResult(componentManifest, msBuild));

            return result;
        }
    }
}
