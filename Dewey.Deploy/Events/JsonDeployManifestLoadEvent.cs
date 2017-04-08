using Dewey.Manifest.Models;
using Ark3.Event;

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
