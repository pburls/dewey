using System.Xml.Linq;

namespace Dewey.Deploy.Events
{
    public class DeploymentElementResult : DeployCommandEvent
    {
        public XElement DeploymentElement { get; private set; }
        public string DeploymentType { get; private set; }

        public DeploymentElementResult(DeployCommand command, XElement deploymentElement, string deployType) : base(command)
        {
            DeploymentElement = deploymentElement;
            DeploymentType = deployType;
        }
    }
}
