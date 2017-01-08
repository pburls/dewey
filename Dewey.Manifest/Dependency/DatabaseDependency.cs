using Dewey.Manifest.Component;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Dependency
{
    public class DatabaseDependency : DependencyElementResult
    {
        public const string DATABASE_DEPENDENCY_TYPE = "database";

        public string Provider { get; private set; }

        public override string Description
        {
            get
            {
                return Provider;
            }
        }

        public DatabaseDependency(ComponentManifest componentMandifest, XElement dependencyElement, string type, string name, string provider) :
            base(componentMandifest, dependencyElement, type, name)
        {
            Provider = provider;
        }

        public static DatabaseDependency Load(string type, string name, ComponentManifest componentMandifest, XElement dependencyElement, List<string> missingAttributes)
        {
            var providerNameAtt = dependencyElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "provider");
            if (providerNameAtt == null || string.IsNullOrWhiteSpace(providerNameAtt.Value))
            {
                missingAttributes.Add("provider");
                return null;
            }

            return new DatabaseDependency(componentMandifest, dependencyElement, type, name, providerNameAtt.Value);
        }
    }
}
