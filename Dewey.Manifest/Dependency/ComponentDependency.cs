using Dewey.Manifest.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dewey.Manifest.Dependency
{
    public class ComponentDependency : DependencyElementResult
    {
        public const string COMPONENT_DEPENDENCY_TYPE = "component";

        public string Protocol { get; private set; }

        public override string Description
        {
            get
            {
                return Protocol;
            }
        }

        public ComponentDependency(ComponentManifest componentMandifest, XElement dependencyElement, string type, string name, string protocol) :
            base(componentMandifest, dependencyElement, type, name)
        {
            Protocol = protocol;
        }

        public static ComponentDependency Load(string type, string name, ComponentManifest componentMandifest, XElement dependencyElement, List<string> missingAttributes)
        {
            var protocolNameAtt = dependencyElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "protocol");
            string protocol = protocolNameAtt != null ? protocolNameAtt.Value : string.Empty;
            return new ComponentDependency(componentMandifest, dependencyElement, type, name, protocol);
        }
    }
}
