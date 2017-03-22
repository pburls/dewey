using Dewey.Manifest.Component;
using Dewey.Manifest.Models;

namespace Dewey.Build.Events
{
    public class JsonBuildActionStarted : JsonBuildEvent
    {
        public JsonBuildActionStarted(Component componentManifest, Models.Build build) : base(componentManifest, build) { }
    }

    public class BuildActionStarted : BuildActionEvent
    {
        public MSBuildArgs Arguments { get; private set; }

        public BuildActionStarted(ComponentManifest componentManifest, string buildType, MSBuildArgs arguments) : base(componentManifest, buildType)
        {
            Arguments = arguments;
        }

        public override bool Equals(object obj)
        {
            BuildActionStarted other = obj as BuildActionStarted;
            if (other == null)
            {
                return false;
            }

            return base.Equals(obj) && Arguments == other.Arguments;
        }

        public override int GetHashCode()
        {
            return Arguments.GetHashCode() ^ base.GetHashCode();
        }

        public static bool operator ==(BuildActionStarted a, BuildActionStarted b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ComponentManifest == b.ComponentManifest && a.BuildType == b.BuildType && a.Arguments == b.Arguments;
        }

        public static bool operator !=(BuildActionStarted a, BuildActionStarted b)
        {
            return !(a == b);
        }
    }
}
