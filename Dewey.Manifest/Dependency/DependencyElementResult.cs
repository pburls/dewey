using Dewey.Manifest.Component;
using System.Xml.Linq;

namespace Dewey.Manifest.Dependency
{
    public class DependencyElementResult : DependencyElementEvent
    {
        public const string COMPONENT_DEPENDENCY_TYPE = "component";

        public ComponentManifest ComponentManifest { get; private set; }
        public XElement DependencyElement { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }

        public DependencyElementResult(ComponentManifest componentMandifest, XElement dependencyElement, string type, string name)
        {
            ComponentManifest = componentMandifest;
            DependencyElement = dependencyElement;
            Type = type;
            Name = name;
        }

        public DependencyElementResult WithType(string type)
        {
            return new DependencyElementResult(ComponentManifest, DependencyElement, type, Name);
        }

        public DependencyElementResult WithComponentManifest(ComponentManifest componentMandifest)
        {
            return new DependencyElementResult(componentMandifest, DependencyElement, Type, Name);
        }
    }
}