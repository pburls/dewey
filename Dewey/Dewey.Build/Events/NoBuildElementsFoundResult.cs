using System.Xml.Linq;

namespace Dewey.Build.Events
{
    public class NoBuildElementsFoundResult : BuildCommandEvent
    {
        public XElement ComponentElement { get; private set; }

        public NoBuildElementsFoundResult(BuildCommand command, XElement componentElement) : base(command)
        {
            ComponentElement = componentElement;
        }

        public override bool Equals(object obj)
        {
            NoBuildElementsFoundResult other = obj as NoBuildElementsFoundResult;
            if (other == null)
            {
                return false;
            }

            return base.Equals(obj) && ComponentElement == other.ComponentElement;
        }

        public override int GetHashCode()
        {
            return ComponentElement.GetHashCode() ^ base.GetHashCode();
        }

        public static bool operator ==(NoBuildElementsFoundResult a, NoBuildElementsFoundResult b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ComponentName == b.ComponentName && a.ComponentElement == b.ComponentElement;
        }

        public static bool operator !=(NoBuildElementsFoundResult a, NoBuildElementsFoundResult b)
        {
            return !(a == b);
        }
    }
}
