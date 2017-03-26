using Dewey.Manifest.Component;
using Dewey.Manifest.Models;

namespace Dewey.Deploy.Events
{
    public class JsonDeploymentActionStarted : JsonDeployEvent
    {
        public JsonDeploymentActionStarted(Component componentManifest, Models.Deploy deploy) : base(componentManifest, deploy) { }
    }

    public class DeploymentActionStarted : DeploymentActionEvent
    {
        public object DeploymentArgs { get; private set; }

        public DeploymentActionStarted(ComponentManifest componentManifest, string deployType, object deploymentArgs) : base(componentManifest, deployType)
        {
            DeploymentArgs = deploymentArgs;
        }
    }
}
