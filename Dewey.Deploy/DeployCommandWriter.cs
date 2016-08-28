using Dewey.Deploy.Events;
using Dewey.Messaging;
using System;

namespace Dewey.Deploy
{
    class DeployCommandWriter :
        IEventHandler<DeployCommandStarted>,
        IEventHandler<ComponentNotFoundResult>,
        IEventHandler<NoDeploymentElementsFoundResult>,
        IEventHandler<DeploymentElementMissingTypeAttributeResult>,
        IEventHandler<DeploymentActionErrorResult>,
        IEventHandler<DeploymentElementMissingAttributeResult>,
        IEventHandler<DeploymentElementInvalidAttributeResult>,
        IEventHandler<DeploymentActionContentNotFoundResult>,
        IEventHandler<DeploymentActionStarted>,
        IEventHandler<DeploymentActionOutputMessage>,
        IEventHandler<DeploymentActionCompletedResult>,
        IEventHandler<DeploymentActionFailed>
    {
        public DeployCommandWriter(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe<DeployCommandStarted>(this);
            eventAggregator.Subscribe<ComponentNotFoundResult>(this);
            eventAggregator.Subscribe<NoDeploymentElementsFoundResult>(this);
            eventAggregator.Subscribe<DeploymentElementMissingTypeAttributeResult>(this);
            eventAggregator.Subscribe<DeploymentActionErrorResult>(this);
            eventAggregator.Subscribe<DeploymentElementMissingAttributeResult>(this);
            eventAggregator.Subscribe<DeploymentElementInvalidAttributeResult>(this);
            eventAggregator.Subscribe<DeploymentActionContentNotFoundResult>(this);
            eventAggregator.Subscribe<DeploymentActionStarted>(this);
            eventAggregator.Subscribe<DeploymentActionOutputMessage>(this);
            eventAggregator.Subscribe<DeploymentActionCompletedResult>(this);
            eventAggregator.Subscribe<DeploymentActionFailed>(this);
        }

        public void Handle(DeployCommandStarted deployCommandStarted)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Deploying component '{0}'.", deployCommandStarted.ComponentName));
        }

        public void Handle(ComponentNotFoundResult componentNotFoundResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("No component manifest file found for component with name '{0}'.", componentNotFoundResult.ComponentName));
        }

        public void Handle(NoDeploymentElementsFoundResult noDeploymentElementsFoundResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("No deployments found for component '{0}' in manifest: {1}", 
                noDeploymentElementsFoundResult.ComponentName, 
                noDeploymentElementsFoundResult.ComponentElement.ToString()));
        }

        public void Handle(DeploymentElementMissingTypeAttributeResult deploymentElementMissingTypeAttributeResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Skipping deployment element of component '{0}' without a valid type: {1}", 
                deploymentElementMissingTypeAttributeResult.ComponentName, 
                deploymentElementMissingTypeAttributeResult.DeploymentElement.ToString()));
        }

        public void Handle(DeploymentActionErrorResult deploymentActionErrorResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Deployment action '{0}' of component '{1}' threw exception: {2}", 
                deploymentActionErrorResult.DeploymentType, 
                deploymentActionErrorResult.ComponentManifest.Name, 
                deploymentActionErrorResult.Exception));
        }

        public void Handle(DeploymentElementMissingAttributeResult deploymentElementMissingAttributeResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Skipping '{0}' deployment element of component '{1}' with a missing attributes: {2}",
                deploymentElementMissingAttributeResult.DeploymentType,
                deploymentElementMissingAttributeResult.ComponentManifest.Name,
                string.Join(",", deploymentElementMissingAttributeResult.AttributeNames)));
        }

        public void Handle(DeploymentElementInvalidAttributeResult deploymentElementInvalidAttributeResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Skipping '{0}' deployment element of component '{1}' with a invalid attributes: {2}", 
                deploymentElementInvalidAttributeResult.DeploymentType, 
                deploymentElementInvalidAttributeResult.ComponentManifest.Name, 
                string.Join(",", deploymentElementInvalidAttributeResult.AttributeNames)));
        }

        public void Handle(DeploymentActionContentNotFoundResult deploymentActionContentNotFoundResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Content path '{0}' does not exist for '{1}' deployment of component '{2}'.",
                deploymentActionContentNotFoundResult.ContentPath,
                deploymentActionContentNotFoundResult.DeploymentType,
                deploymentActionContentNotFoundResult.ComponentManifest.Name));
        }

        public void Handle(DeploymentActionStarted deploymentActionStarted)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Deployment action '{0}' of component '{1}' started with arguments: {2}",
                deploymentActionStarted.DeploymentType,
                deploymentActionStarted.ComponentManifest.Name,
                deploymentActionStarted.DeploymentArgs.ToString()));
        }

        public void Handle(DeploymentActionFailed deploymentActionFailed)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Deployment action '{0}' of component '{1}' failed with reason: {2}",
                deploymentActionFailed.DeploymentType,
                deploymentActionFailed.ComponentManifest.Name,
                deploymentActionFailed.Reason));
        }

        public void Handle(DeploymentActionOutputMessage deploymentActionOutputMessage)
        {
            Console.ResetColor();
            Console.WriteLine(deploymentActionOutputMessage.Message);
        }

        public void Handle(DeploymentActionCompletedResult deploymentActionCompletedResult)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Deployment action '{0}' of component '{1}' completed with arguments: {2}",
                deploymentActionCompletedResult.DeploymentType,
                deploymentActionCompletedResult.ComponentManifest.Name,
                deploymentActionCompletedResult.DeploymentArgs.ToString()));
        }
    }
}
