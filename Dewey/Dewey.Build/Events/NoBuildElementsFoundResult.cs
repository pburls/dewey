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
    }
}
