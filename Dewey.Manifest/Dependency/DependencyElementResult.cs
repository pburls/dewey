using System.Xml.Linq;

namespace Dewey.Manifest.Dependency
{
    public class DependencyElementResult : DependencyElementEvent
    {
        public const string COMPONENT_DEPENDENCY_TYPE = "component";

        public XElement DependencyElement { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }

        public DependencyElementResult(XElement dependencyElement, string type, string name)
        {
            DependencyElement = dependencyElement;
            Type = type;
            Name = name;
        }

        public DependencyElementResult WithType(string type)
        {
            return new DependencyElementResult(DependencyElement, type, Name);
        }
    }
}