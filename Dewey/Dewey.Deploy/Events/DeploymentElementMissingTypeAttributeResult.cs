using System.Xml.Linq;

namespace Dewey.Deploy.Events
{
    public class DeploymentElementMissingTypeAttributeResult : DeployCommandEvent
    {
        public XElement DeploymentElement { get; private set; }

        public DeploymentElementMissingTypeAttributeResult(DeployCommand command, XElement deploymentElement) : base(command)
        {
            DeploymentElement = deploymentElement;
        }
    }
}
