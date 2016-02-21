using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.CLI.Deployments
{
    class DeploymentActionFactory
    {
        public static IDeploymentAction CreateDeploymentAction(string deploymentType)
        {
            switch (deploymentType)
            {
                case "iis":
                    return new IISDeployment();
                default:
                    throw new ArgumentOutOfRangeException("deploymentType", deploymentType, string.Format("Unknown deployment type {0}.", deploymentType));
            }
        }
    }
}
