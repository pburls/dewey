using Dewey.Manifest.Models;
using Ark3.Event;

namespace Dewey.Build.Events
{
    public abstract class JsonBuildManifestLoadEvent : IEvent
    {
        public Component Component { get; private set; }

        public JsonBuildManifestLoadEvent(Component component)
        {
            Component = component;
        }
    }
}
