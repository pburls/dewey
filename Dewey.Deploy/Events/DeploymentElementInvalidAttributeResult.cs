using Dewey.Manifest.Component;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Dewey.Deploy.Events
{
    public class DeploymentElementInvalidAttributeResult : DeploymentActionEvent
    {
        public XElement DeploymentElement { get; private set; }

        public IEnumerable<string> AttributeNames { get; private set; }

        public DeploymentElementInvalidAttributeResult(ComponentManifest componentManifest, string deployType, XElement deploymentElement, IEnumerable<string> attributeNames) : base(componentManifest, deployType)
        {
            DeploymentElement = deploymentElement;
            AttributeNames = attributeNames;
        }
    }
}
