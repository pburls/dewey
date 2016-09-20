using Dewey.Messaging;
using System.Xml.Linq;

namespace Dewey.Manifest.Dependency
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