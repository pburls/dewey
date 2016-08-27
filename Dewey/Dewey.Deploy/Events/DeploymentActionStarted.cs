using Dewey.Manifest.Component;

namespace Dewey.Deploy.Events
{
    public class DeploymentActionStarted : DeploymentActionEvent
    {
        public object DeploymentArgs { get; private set; }

        public DeploymentActionStarted(ComponentManifest componentManifest, string deployType, object deploymentArgs) : base(componentManifest, deployType)
        {
            DeploymentArgs = deploymentArgs;
        }
    }
}
