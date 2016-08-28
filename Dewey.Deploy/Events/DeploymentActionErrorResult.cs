using Dewey.Manifest.Component;
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
}
