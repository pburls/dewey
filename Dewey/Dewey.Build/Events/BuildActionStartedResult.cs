using Dewey.Manifest.Component;

namespace Dewey.Build.Events
{
    public class BuildActionStartedResult : BuildActionResult
    {
        public string Target { get; private set; }

        public BuildActionStartedResult(ComponentManifest componentManifest, string buildType, string target) : base(componentManifest, buildType)
        {
            Target = target;
        }
    }
}
