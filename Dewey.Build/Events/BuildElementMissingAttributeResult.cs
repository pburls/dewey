using Dewey.Manifest.Component;
using System.Xml.Linq;

namespace Dewey.Build.Events
{
    public class BuildElementMissingAttributeResult : BuildActionEvent
    {
        public XElement BuildElement { get; private set; }

        public string AttributeName { get; private set; }

        public BuildElementMissingAttributeResult(ComponentManifest componentManifest, string buildType, XElement buildElement, string attributeName) : base(componentManifest, buildType)
        {
            BuildElement = buildElement;
            AttributeName = attributeName;
        }

        public override bool Equals(object obj)
        {
            BuildElementMissingAttributeResult other = obj as BuildElementMissingAttributeResult;
            if (other == null)
            {
                return false;
            }

            return base.Equals(obj) && BuildElement == other.BuildElement && AttributeName == other.AttributeName;
        }

        public override int GetHashCode()
        {
            return BuildElement.GetHashCode() ^ AttributeName.GetHashCode() ^ base.GetHashCode();
        }

        public static bool operator ==(BuildElementMissingAttributeResult a, BuildElementMissingAttributeResult b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ComponentManifest == b.ComponentManifest && a.BuildType == b.BuildType && a.BuildElement == b.BuildElement && a.AttributeName == b.AttributeName;
        }

        public static bool operator !=(BuildElementMissingAttributeResult a, BuildElementMissingAttributeResult b)
        {
            return !(a == b);
        }
    }
}
