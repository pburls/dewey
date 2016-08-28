using Dewey.Manifest.Component;

namespace Dewey.Build.Events
{
    public class BuildActionCompletedResult : BuildActionEvent
    {
        public MSBuildArgs Arguments { get; private set; }

        public BuildActionCompletedResult(ComponentManifest componentManifest, string buildType, MSBuildArgs arguments) : base(componentManifest, buildType)
        {
            Arguments = arguments;
        }

        public override bool Equals(object obj)
        {
            BuildActionCompletedResult other = obj as BuildActionCompletedResult;
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

        public static bool operator ==(BuildActionCompletedResult a, BuildActionCompletedResult b)
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

        public static bool operator !=(BuildActionCompletedResult a, BuildActionCompletedResult b)
        {
            return !(a == b);
        }
    }
}
