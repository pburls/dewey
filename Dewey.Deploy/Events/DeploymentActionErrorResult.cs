using Dewey.Manifest.Models;
using System;

namespace Dewey.Deploy.Events
{
    public class JsonDeploymentActionErrorResult : JsonDeployEvent
    {
        public Exception Exception { get; private set; }

        public JsonDeploymentActionErrorResult(Component componentManifest, Models.Deploy deploy, Exception exception) : base(componentManifest, deploy)
        {
            Exception = exception;
        }
    }
}
