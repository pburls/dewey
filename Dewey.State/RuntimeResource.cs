using Dewey.Manifest.RuntimeResources;
using System.Xml.Linq;

namespace Dewey.State
{
    public class RuntimeResource
    {
        public RuntimeResourceItem RuntimeResourceItem { get; private set; }

        public XElement RuntimeResourceElement { get; private set; }

        public RuntimeResource(RuntimeResourceItem runtimeResourceItem, XElement runtimeResourceElement)
        {
            RuntimeResourceItem = runtimeResourceItem;
            RuntimeResourceElement = runtimeResourceElement;
        }
    }
}
