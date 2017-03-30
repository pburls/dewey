using Dewey.Manifest.Models;
using Dewey.Messaging;

namespace Dewey.Deploy.Events
{
    public abstract class JsonDeployManifestLoadEvent : IEvent
    {
        public Component Component { get; private set; }

        public JsonDeployManifestLoadEvent(Component component)
        {
            Component = component;
        }
    }
}
