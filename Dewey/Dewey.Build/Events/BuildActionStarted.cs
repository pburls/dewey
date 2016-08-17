using Dewey.Manifest.Component;

namespace Dewey.Build.Events
{
    public class BuildActionStarted : BuildActionEvent
    {
        public string Target { get; private set; }

        public BuildActionStarted(ComponentManifest componentManifest, string buildType, string target) : base(componentManifest, buildType)
        {
            Target = target;
        }
    }
}
