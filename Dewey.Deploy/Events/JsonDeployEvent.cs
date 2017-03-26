using Dewey.Manifest.Models;

namespace Dewey.Deploy.Events
{
    public abstract class JsonDeployEvent : JsonDeployManifestLoadEvent
    {
        public Models.Deploy Deploy { get; private set; }

        public JsonDeployEvent(Component component, Models.Deploy deploy) : base(component)
        {
            Deploy = deploy;
        }
    }
}
