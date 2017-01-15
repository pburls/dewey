using System.Xml.Linq;

namespace Dewey.Manifest.RuntimeResources
{
    public class RuntimeResourceItem
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Provider { get; private set; }
        public string Context { get; private set; }
        public XElement Element { get; private set; }
        public RuntimeResourcesManifest RuntimeResourcesManifest { get; private set; }

        public RuntimeResourceItem(string name, string type, string provider, string context, RuntimeResourcesManifest runtimeResourcesManifest, XElement element)
        {
            Name = name;
            Type = type;
            Provider = provider;
            Context = context;
            RuntimeResourcesManifest = runtimeResourcesManifest;
            Element = element;
        }
    }
}
