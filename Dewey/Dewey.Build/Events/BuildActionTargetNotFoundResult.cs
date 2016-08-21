using Dewey.Manifest.Component;

namespace Dewey.Build.Events
{
    public class BuildActionTargetNotFoundResult : BuildActionEvent
    {
        public string Target { get; private set; }

        public BuildActionTargetNotFoundResult(ComponentManifest componentManifest, string buildType, string fileName) : base(componentManifest, buildType)
        {
            Target = fileName;
        }

        public override bool Equals(object obj)
        {
            BuildActionTargetNotFoundResult other = obj as BuildActionTargetNotFoundResult;
            if (other == null)
            {
                return false;
            }

            return base.Equals(obj) && Target == other.Target;
        }

        public override int GetHashCode()
        {
            return Target.GetHashCode() ^ base.GetHashCode();
        }

        public static bool operator ==(BuildActionTargetNotFoundResult a, BuildActionTargetNotFoundResult b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ComponentManifest == b.ComponentManifest && a.BuildType == b.BuildType && a.Target == b.Target;
        }

        public static bool operator !=(BuildActionTargetNotFoundResult a, BuildActionTargetNotFoundResult b)
        {
            return !(a == b);
        }
    }
}
