using Dewey.Manifest.Component;
using Dewey.Manifest.Models;
using System;

namespace Dewey.Deploy.Events
{
    public class DeploymentActionErrorResult : DeploymentActionEvent
    {
        public Exception Exception { get; private set; }

        public DeploymentActionErrorResult(ComponentManifest componentManifest, string deploymentType, Exception exception) : base(componentManifest, deploymentType)
        {
            Exception = exception;
        }
    }
    public class JsonDeploymentActionErrorResult : JsonDeployEvent
    {
        public Exception Exception { get; private set; }

        public JsonDeploymentActionErrorResult(Component componentManifest, Models.Deploy deploy, Exception exception) : base(componentManifest, deploy)
        {
            Exception = exception;
        }
    }
}
