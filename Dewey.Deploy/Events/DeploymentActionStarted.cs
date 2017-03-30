using Dewey.Manifest.Models;

namespace Dewey.Deploy.Events
{
    public class JsonDeploymentActionStarted : JsonDeployEvent
    {
        public JsonDeploymentActionStarted(Component componentManifest, Models.Deploy deploy) : base(componentManifest, deploy) { }
    }
}
