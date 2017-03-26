using SimpleInjector;
using System.Linq;

namespace Dewey.Deploy
{
    public class DeploymentActionFactory : IDeploymentActionFactory
    {
        private readonly Container _container;

        public DeploymentActionFactory(Container container)
        {
            _container = container;
        }

        public IDeploymentAction CreateDeploymentAction(string deploymentType)
        {
            var deploymentActions = _container.GetAllInstances<IDeploymentAction>().ToDictionary(x => x.Type);
            
            if (deploymentActions.ContainsKey(deploymentType))
            {
                return deploymentActions[deploymentType];
            }

            return null;
        }
    }
}
