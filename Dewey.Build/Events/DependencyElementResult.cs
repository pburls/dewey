using System.Xml.Linq;

namespace Dewey.Build.Events
{
    public class DependencyElementResult : DependencyElementEvent
    {
        public XElement DependencyElement { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }

        public DependencyElementResult(XElement dependencyElement, string type, string name)
        {
            DependencyElement = dependencyElement;
            Type = type;
            Name = name;
        }
    }
}