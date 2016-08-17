using Dewey.Manifest.Component;

namespace Dewey.Build.Events
{
    public class BuildActionTargetNotFoundResult : BuildActionResult
    {
        public string FileName { get; private set; }

        public BuildActionTargetNotFoundResult(ComponentManifest componentManifest, string buildType, string fileName) : base(componentManifest, buildType)
        {
            FileName = fileName;
        }
    }
}
