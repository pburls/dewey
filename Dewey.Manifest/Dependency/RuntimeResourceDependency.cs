using Dewey.Manifest.Component;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Dewey.Manifest.Dependency
{
    public class RuntimeResourceDependency : DependencyElementResult
    {
        public const string RUNTIME_RESOURCE_DEPENDENCY_TYPE = "runtimeResource";

        public RuntimeResourceDependency(ComponentManifest componentMandifest, XElement dependencyElement, string type, string name) :
            base(componentMandifest, dependencyElement, type, name)
        {

        }

        public static RuntimeResourceDependency Load(string type, string name, ComponentManifest componentMandifest, XElement dependencyElement, List<string> missingAttributes)
        {
            return new RuntimeResourceDependency(componentMandifest, dependencyElement, type, name);
        }
    }
}
