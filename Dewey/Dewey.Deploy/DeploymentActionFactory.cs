using Dewey.Messaging;
using System;

namespace Dewey.Deploy
{
    class DeploymentActionFactory
    {
        public static IDeploymentAction CreateDeploymentAction(string deploymentType, IEventAggregator eventAggregator)
        {
            switch (deploymentType)
            {
                case IISDeployment.DEPLOYMENT_TYPE:
                    return new IISDeployment(eventAggregator);
                default:
                    throw new ArgumentOutOfRangeException("deploymentType", deploymentType, string.Format("Unknown deployment type {0}.", deploymentType));
            }
        }
    }
}
