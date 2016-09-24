using Dewey.Manifest.Component;
using System.Xml.Linq;

namespace Dewey.State
{
    public class Component
    {
        public ComponentManifest ComponentManifest { get; private set; }

        public XElement ComponentElement { get; private set; }

        public Component(ComponentManifest componentManifest, XElement componentElement)
        {
            ComponentManifest = componentManifest;
            ComponentElement = componentElement;
        }
    }
}
