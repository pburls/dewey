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
        IEventHandler<DeploymentActionFailed>,
        IEventHandler<NoJsonDeployManifestFound>,
        IEventHandler<JsonDeployManifestInvalidType>,
        IEventHandler<JsonDeploymentActionErrorResult>,
        IEventHandler<JsonDeploymentMissingAttributesResult>,
        IEventHandler<JsonDeploymentActionContentNotFoundResult>,
        IEventHandler<JsonDeploymentActionFailed>,
        IEventHandler<JsonDeploymentActionStarted>,
        IEventHandler<JsonDeploymentActionOutputMessage>,
        IEventHandler<JsonDeploymentActionCompletedResult>
    {
        public DeployCommandWriter(IEventAggregator eventAggregator)
        {
            eventAggregator.SubscribeAll(this);
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

        public void Handle(NoJsonDeployManifestFound noJsonDeployManifestFound)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("No deployment found for component '{0}' in manifest: {1}",
                noJsonDeployManifestFound.Component.name,
                noJsonDeployManifestFound.Component.ToJson()));
        }

        public void Handle(JsonDeployManifestInvalidType jsonDeployManifestInvalidType)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Invalid or unknown deploy type '{0}' in component manifest '{1}'",
                jsonDeployManifestInvalidType.Deploy.type,
                jsonDeployManifestInvalidType.Component.name));
        }

        public void Handle(DeploymentElementMissingTypeAttributeResult deploymentElementMissingTypeAttributeResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Skipping deployment element of component '{0}' without a valid type: {1}", 
                deploymentElementMissingTypeAttributeResult.ComponentName, 
                deploymentElementMissingTypeAttributeResult.DeploymentElement.ToString()));
        }

        public void Handle(JsonDeploymentMissingAttributesResult jsonDeploymentMissingAttributesResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var attributes = string.Join(", ", jsonDeploymentMissingAttributesResult.AttributeNames);
            Console.WriteLine($"Deployment of type '{jsonDeploymentMissingAttributesResult.Deploy.type}' for component '{jsonDeploymentMissingAttributesResult.Component.name}' is missing attributes '{attributes}' requied for action.");
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
        

        public void Handle(JsonDeploymentActionContentNotFoundResult jsonDeploymentActionContentNotFoundResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Content path '{0}' does not exist for '{1}' deployment of component '{2}'.",
                jsonDeploymentActionContentNotFoundResult.ContentPath,
                jsonDeploymentActionContentNotFoundResult.Deploy.type,
                jsonDeploymentActionContentNotFoundResult.Component.name));
        }

        public void Handle(DeploymentActionStarted deploymentActionStarted)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Deployment action '{0}' of component '{1}' started with arguments: {2}",
                deploymentActionStarted.DeploymentType,
                deploymentActionStarted.ComponentManifest.Name,
                deploymentActionStarted.DeploymentArgs.ToString()));
        }

        public void Handle(JsonDeploymentActionStarted deploymentActionStarted)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Deployment action '{0}' of component '{1}' started.",
                deploymentActionStarted.Deploy.BackingData.ToString(Newtonsoft.Json.Formatting.None),
                deploymentActionStarted.Component.name));
        }

        public void Handle(DeploymentActionFailed deploymentActionFailed)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Deployment action '{0}' of component '{1}' failed with reason: {2}",
                deploymentActionFailed.DeploymentType,
                deploymentActionFailed.ComponentManifest.Name,
                deploymentActionFailed.Reason));
        }

        public void Handle(JsonDeploymentActionFailed deploymentActionFailed)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Deployment action '{0}' of component '{1}' failed with reason: {2}",
                deploymentActionFailed.Deploy.type,
                deploymentActionFailed.Component.name,
                deploymentActionFailed.Reason));
        }

        public void Handle(DeploymentActionOutputMessage deploymentActionOutputMessage)
        {
            Console.ResetColor();
            Console.WriteLine(deploymentActionOutputMessage.Message);
        }

        public void Handle(JsonDeploymentActionOutputMessage deploymentActionOutputMessage)
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

        public void Handle(JsonDeploymentActionCompletedResult deploymentActionCompletedResult)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Deployment action '{0}' of component '{1}' completed.",
                deploymentActionCompletedResult.Deploy.BackingData.ToString(Newtonsoft.Json.Formatting.None),
                deploymentActionCompletedResult.Component.name));
        }

        public void Handle(JsonDeploymentActionErrorResult deploymentActionErrorResult)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Deploy action '{0}' of component '{1}' threw exception: {2}",
                deploymentActionErrorResult.Deploy.type,
                deploymentActionErrorResult.Component.name,
                deploymentActionErrorResult.Exception));
        }
    }
}
