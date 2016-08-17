using Dewey.Manifest.Component;
using Dewey.Messaging;

namespace Dewey.Build.Events
{
    public abstract class BuildActionResult : IEvent
    {
        public ComponentManifest ComponentManifest { get; private set; }

        public string BuildType { get; private set; }

        public BuildActionResult(ComponentManifest componentManifest, string buildType)
        {
            ComponentManifest = componentManifest;
            BuildType = buildType;
        }
    }
}
