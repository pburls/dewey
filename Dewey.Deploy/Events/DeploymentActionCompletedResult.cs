using Dewey.Manifest.Models;

namespace Dewey.Deploy.Events
{
    public class JsonDeploymentActionCompletedResult : JsonDeployEvent
    {
        public JsonDeploymentActionCompletedResult(Component componentManifest, Models.Deploy deploy) : base(componentManifest, deploy) { }
    }
}
