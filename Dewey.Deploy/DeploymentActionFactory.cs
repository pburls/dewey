using Dewey.Messaging;
using SimpleInjector;
using System;
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

            IDeploymentAction deployAction;
            if (!deploymentActions.TryGetValue(deploymentType, out deployAction))
            {
                throw new ArgumentOutOfRangeException("deploymentType", deploymentType, string.Format("Unknown deployment type {0}.", deploymentType));
            }

            return deployAction;
        }
    }
}
