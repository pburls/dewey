using System.Xml.Linq;

namespace Dewey.Deploy.Events
{
    public class NoDeploymentElementsFoundResult : DeployCommandEvent
    {
        public XElement ComponentElement { get; private set; }

        public NoDeploymentElementsFoundResult(DeployCommand command, XElement componentElement) : base(command)
        {
            ComponentElement = componentElement;
        }
    }
}
