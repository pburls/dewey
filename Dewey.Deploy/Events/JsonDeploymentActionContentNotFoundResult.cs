using Dewey.Manifest.Models;

namespace Dewey.Deploy.Events
{
    public class JsonDeploymentActionContentNotFoundResult : JsonDeployEvent
    {
        public string ContentPath { get; private set; }

        public JsonDeploymentActionContentNotFoundResult(Component componentManifest, Models.Deploy deploy, string content) : base(componentManifest, deploy)
        {
            ContentPath = content;
        }
    }
}
