using Dewey.Manifest.Component;

namespace Dewey.Deploy.Events
{
    public class DeploymentActionOutputMessage : DeploymentActionEvent
    {
        public string Message { get; private set; }

        public DeploymentActionOutputMessage(ComponentManifest componentManifest, string deployType, string message) : base(componentManifest, deployType)
        {
            Message = message;
        }
    }
}
