using Dewey.Manifest.Component;
using Dewey.Messaging;
using System.Collections.Generic;
using System.Linq;
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
        public ComponentManifest Parent { get; private set; }

        public DependencyElementResult(ComponentManifest componentMandifest, XElement dependencyElement, string type, string name, ComponentManifest parent)
        {
            ComponentManifest = componentMandifest;
            DependencyElement = dependencyElement;
            Type = type;
            Name = name;
            Parent = parent;
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