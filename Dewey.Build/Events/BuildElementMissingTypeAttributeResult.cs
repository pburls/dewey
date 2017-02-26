using System.Xml.Linq;

namespace Dewey.Build.Events
{
    public class BuildElementMissingTypeAttributeResult : BuildCommandEvent
    {
        public XElement BuildElement { get; private set; }

        public BuildElementMissingTypeAttributeResult(BuildCommand command, XElement buildElement) : base(command)
        {
            BuildElement = buildElement;
        }

        public override bool Equals(object obj)
        {
            BuildElementMissingTypeAttributeResult other = obj as BuildElementMissingTypeAttributeResult;
            if (other == null)
            {
                return false;
            }

            return base.Equals(obj) && BuildElement == other.BuildElement;
        }

        public override int GetHashCode()
        {
            return BuildElement.GetHashCode() ^ base.GetHashCode();
        }

        public static bool operator ==(BuildElementMissingTypeAttributeResult a, BuildElementMissingTypeAttributeResult b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ComponentName == b.ComponentName && a.BuildElement == b.BuildElement;
        }

        public static bool operator !=(BuildElementMissingTypeAttributeResult a, BuildElementMissingTypeAttributeResult b)
        {
            return !(a == b);
        }
    }
}
