using Dewey.Manifest.Component;
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

    public class DeploymentActionFailed : DeploymentActionEvent
    {
        public string Reason { get; private set; }

        public DeploymentActionFailed(ComponentManifest componentManifest, string deployType, string reason) : base(componentManifest, deployType)
        {
            Reason = reason;
        }
    }
}
