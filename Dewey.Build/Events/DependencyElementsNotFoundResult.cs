using Dewey.Messaging;
using System.Xml.Linq;

namespace Dewey.Build.Events
{
    public class DependencyElementsNotFoundResult : DependencyElementEvent
    {
        public XElement ComponentElement { get; private set; }

        public DependencyElementsNotFoundResult(XElement componentElement)
        {
            ComponentElement = componentElement;
        }
    }
}