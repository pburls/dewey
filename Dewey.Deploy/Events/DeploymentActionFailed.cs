using Dewey.Manifest.Component;

namespace Dewey.Deploy.Events
{
    public class DeploymentActionFailed : DeploymentActionEvent
    {
        public string Reason { get; private set; }

        public DeploymentActionFailed(ComponentManifest componentManifest, string deployType, string reason) : base(componentManifest, deployType)
        {
            Reason = reason;
        }
    }
}
