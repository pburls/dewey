using Dewey.Messaging;
using System.Xml.Linq;

namespace Dewey.Build.Events
{
    public class DependenciesElementNotFoundResult : DependencyElementEvent
    {
        public XElement ComponentElement { get; private set; }

        public DependenciesElementNotFoundResult(XElement componentElement)
        {
            ComponentElement = componentElement;
        }
    }
}