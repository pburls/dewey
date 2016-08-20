using Dewey.Manifest.Component;
using Dewey.Messaging;

namespace Dewey.Deploy.Events
{
    public abstract class DeploymentActionEvent : IEvent
    {
        public ComponentManifest ComponentManifest { get; private set; }

        public string DeploymentType { get; private set; }

        public DeploymentActionEvent(ComponentManifest componentManifest, string deploymentType)
        {
            ComponentManifest = componentManifest;
            DeploymentType = deploymentType;
        }
    }
}
