using Dewey.Messaging;
using System.Linq;
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

        public static void LoadDeployActionsFromComponentMandifest(DeployCommand command, XElement componentElement, IEventAggregator eventAggregator)
        {
            var deploymentsElements = componentElement.Elements().FirstOrDefault(x => x.Name.LocalName == "deployments");
            if (deploymentsElements == null)
            {
                eventAggregator.PublishEvent(new NoDeploymentElementsFoundResult(command, componentElement));
                return;
            }

            var deploymentElements = deploymentsElements.Elements().Where(x => x.Name.LocalName == "deployment").ToList();
            if (deploymentElements.Count == 0)
            {
                eventAggregator.PublishEvent(new NoDeploymentElementsFoundResult(command, componentElement));
                return;
            }

            foreach (var deploymentElement in deploymentElements)
            {
                var deploymentTypeAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "type");
                if (deploymentTypeAtt == null || string.IsNullOrWhiteSpace(deploymentTypeAtt.Value))
                {
                    eventAggregator.PublishEvent(new DeploymentElementMissingTypeAttributeResult(command, deploymentElement));
                    continue;
                }

                eventAggregator.PublishEvent(new DeploymentElementResult(command, deploymentElement, deploymentTypeAtt.Value));
            }
        }
    }
}
