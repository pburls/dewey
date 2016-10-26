using Dewey.Messaging;

namespace Dewey.Deploy
{
    public interface IDeploymentActionFactory
    {
        IDeploymentAction CreateDeploymentAction(string deploymentType);
    }
}