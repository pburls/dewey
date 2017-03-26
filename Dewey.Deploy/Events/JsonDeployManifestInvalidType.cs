using Dewey.Manifest.Models;

namespace Dewey.Deploy.Events
{
    public class JsonDeployManifestInvalidType : JsonDeployEvent
    {
        public JsonDeployManifestInvalidType(Component component, Models.Deploy deploy) : base(component, deploy) { }
    }
}
