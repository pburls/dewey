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

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            BuildActionEvent other = obj as BuildActionEvent;
            if (other == null)
            {
                return false;
            }

            return ComponentManifest == other.ComponentManifest && BuildType == other.BuildType;
        }

        public override int GetHashCode()
        {
            return ComponentManifest.GetHashCode();
        }

        public static bool operator ==(BuildActionEvent a, BuildActionEvent b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ComponentManifest == b.ComponentManifest && a.BuildType == b.BuildType;
        }

        public static bool operator !=(BuildActionEvent a, BuildActionEvent b)
        {
            return !(a == b);
        }
    }
}
