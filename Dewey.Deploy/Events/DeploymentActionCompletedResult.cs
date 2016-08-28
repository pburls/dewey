using Dewey.Manifest.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Deploy.Events
{
    public class DeploymentActionCompletedResult : DeploymentActionEvent
    {
        public object DeploymentArgs { get; private set; }

        public DeploymentActionCompletedResult(ComponentManifest componentManifest, string deployType, object deploymentArgs) : base(componentManifest, deployType)
        {
            DeploymentArgs = deploymentArgs;
        }
    }
}
