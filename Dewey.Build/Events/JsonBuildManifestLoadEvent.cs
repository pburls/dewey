using Dewey.Manifest.Models;
using Dewey.Messaging;

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
