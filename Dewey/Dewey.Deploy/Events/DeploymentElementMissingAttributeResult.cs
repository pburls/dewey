using Dewey.Manifest.Component;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Dewey.Deploy.Events
{
    public class DeploymentElementMissingAttributeResult : DeploymentActionEvent
    {
        public XElement DeploymentElement { get; private set; }

        public IEnumerable<string> AttributeNames { get; private set; }

        public DeploymentElementMissingAttributeResult(ComponentManifest componentManifest, string deployType, XElement deploymentElement, IEnumerable<string> attributeNames) : base(componentManifest, deployType)
        {
            DeploymentElement = deploymentElement;
            AttributeNames = attributeNames;
        }
    }
}
