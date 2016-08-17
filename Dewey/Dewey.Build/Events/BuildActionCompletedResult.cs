using Dewey.Manifest.Component;

namespace Dewey.Build.Events
{
    public class BuildActionCompletedResult : BuildActionResult
    {
        public string Target { get; private set; }

        public BuildActionCompletedResult(ComponentManifest componentManifest, string buildType, string target) : base(componentManifest, buildType)
        {
            Target = target;
        }
    }
}
