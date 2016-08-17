using Dewey.Manifest.Component;
using Dewey.Messaging;

namespace Dewey.Build.Events
{
    public abstract class BuildActionEvent : IEvent
    {
        public ComponentManifest ComponentManifest { get; private set; }

        public string BuildType { get; private set; }

        public BuildActionEvent(ComponentManifest componentManifest, string buildType)
        {
            ComponentManifest = componentManifest;
            BuildType = buildType;
        }
    }
}
