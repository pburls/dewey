using Dewey.Manifest.Component;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Dependency
{
    public class QueueDependency : DependencyElementResult
    {
        public const string QUEUE_DEPENDENCY_TYPE = "queue";

        public string Provider { get; private set; }
        public string Format { get; private set; }

        public override string Description
        {
            get
            {
                return Provider;
            }
        }

        public QueueDependency(ComponentManifest componentMandifest, XElement dependencyElement, string type, string name, string provider, string format) :
            base(componentMandifest, dependencyElement, type, name)
        {
            Provider = provider;
            Format = format;
        }

        public static QueueDependency Load(string type, string name, ComponentManifest componentMandifest, XElement dependencyElement, List<string> missingAttributes)
        {
            var providerNameAtt = dependencyElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "provider");
            if (providerNameAtt == null || string.IsNullOrWhiteSpace(providerNameAtt.Value))
            {
                missingAttributes.Add("provider");
                return null;
            }

            var formatNameAtt = dependencyElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "format");
            string format = formatNameAtt != null ? formatNameAtt.Value : string.Empty;

            return new QueueDependency(componentMandifest, dependencyElement, type, name, providerNameAtt.Value, format);
        }
    }
}
