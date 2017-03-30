using Dewey.Manifest.Models;

namespace Dewey.Deploy.Events
{
    public class JsonDeploymentActionFailed : JsonDeployEvent
    {
        public string Reason { get; private set; }

        public JsonDeploymentActionFailed(Component componentManifest, Models.Deploy deploy, string reason) : base(componentManifest, deploy)
        {
            Reason = reason;
        }
    }
}
