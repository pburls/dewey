using Dewey.Manifest.Component;
using Dewey.Manifest.Models;

namespace Dewey.Deploy.Events
{
    public class JsonDeploymentActionCompletedResult : JsonDeployEvent
    {
        public JsonDeploymentActionCompletedResult(Component componentManifest, Models.Deploy deploy) : base(componentManifest, deploy) { }
    }

    public class DeploymentActionCompletedResult : DeploymentActionEvent
    {
        public object DeploymentArgs { get; private set; }

        public DeploymentActionCompletedResult(ComponentManifest componentManifest, string deployType, object deploymentArgs) : base(componentManifest, deployType)
        {
            DeploymentArgs = deploymentArgs;
        }
    }
}
