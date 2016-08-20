using Dewey.Manifest.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Deploy.Events
{
    public class DeploymentActionContentNotFoundResult : DeploymentActionEvent
    {
        public string ContentPath { get; private set; }

        public DeploymentActionContentNotFoundResult(ComponentManifest componentManifest, string deployType, string contentPath) : base(componentManifest, deployType)
        {
            ContentPath = contentPath;
        }
    }
}
