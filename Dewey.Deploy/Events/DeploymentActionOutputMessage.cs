using Dewey.Manifest.Models;

namespace Dewey.Deploy.Events
{
    public class JsonDeploymentActionOutputMessage : JsonDeployEvent
    {
        public string Message { get; private set; }

        public JsonDeploymentActionOutputMessage(Component componentManifest, Models.Deploy deploy, string message) : base(componentManifest, deploy)
        {
            Message = message;
        }
    }
}
