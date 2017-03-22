using Dewey.Manifest.Models;

namespace Dewey.Build.Events
{
    public abstract class JsonBuildEvent : JsonBuildManifestLoadEvent
    {
        public Models.Build Build { get; private set; }

        public JsonBuildEvent(Component component, Models.Build build) : base(component)
        {
            Build = build;
        }
    }
}
