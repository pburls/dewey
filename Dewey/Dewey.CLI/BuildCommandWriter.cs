using Dewey.Build.Events;
using Dewey.Messaging;
using System;

namespace Dewey.CLI
{
    class BuildCommandWriter : 
        IEventHandler<BuildCommandStarted>, 
        IEventHandler<ComponentNotFoundResult>, 
        IEventHandler<NoBuildElementsFoundResult>, 
        IEventHandler<BuildElementMissingTypeAttributeResult>,
        IEventHandler<BuildElementMissingAttributeResult>,
        IEventHandler<BuildActionTargetNotFoundResult>,
        IEventHandler<BuildActionStarted>,
        IEventHandler<BuildActionCompletedResult>,
        IEventHandler<BuildActionErrorResult>
    {
        public BuildCommandWriter(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe<BuildCommandStarted>(this);
            eventAggregator.Subscribe<ComponentNotFoundResult>(this);
            eventAggregator.Subscribe<NoBuildElementsFoundResult>(this);
            eventAggregator.Subscribe<BuildElementMissingTypeAttributeResult>(this);
            eventAggregator.Subscribe<BuildElementMissingAttributeResult>(this);
            eventAggregator.Subscribe<BuildActionTargetNotFoundResult>(this);
            eventAggregator.Subscribe<BuildActionStarted>(this);
            eventAggregator.Subscribe<BuildActionCompletedResult>(this);
            eventAggregator.Subscribe<BuildActionErrorResult>(this);
        }

        public void Handle(BuildCommandStarted buildCommandStarted)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Building component '{0}'.", buildCommandStarted.ComponentName));
        }

        public void Handle(ComponentNotFoundResult componentNotFoundResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("No component manifest file found for component with name '{0}'.", componentNotFoundResult.ComponentName));
        }

        public void Handle(NoBuildElementsFoundResult noBuildElementsFoundResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("No builds found for component '{0}' in manifest: {1}", noBuildElementsFoundResult.ComponentName, noBuildElementsFoundResult.ComponentElement.ToString()));
        }

        public void Handle(BuildElementMissingTypeAttributeResult buildElementMissingTypeAttributeResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Skipping build element of component '{0}' without a valid type: {1}", buildElementMissingTypeAttributeResult.ComponentName, buildElementMissingTypeAttributeResult.BuildElement.ToString()));
        }

        public void Handle(BuildElementMissingAttributeResult buildElementMissingAttributeResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Skipping build element of component '{0}' with invalid attribute '{1}' requied for action {2}: {3}", buildElementMissingAttributeResult.ComponentManifest.Name, buildElementMissingAttributeResult.AttributeName, buildElementMissingAttributeResult.BuildType, buildElementMissingAttributeResult.BuildElement.ToString()));
        }

        public void Handle(BuildActionTargetNotFoundResult buildTargetNotFoundResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Target file '{0}' not found for '{1}' action of component '{2}'.", buildTargetNotFoundResult.FileName, buildTargetNotFoundResult.BuildType, buildTargetNotFoundResult.ComponentManifest.Name));
        }

        public void Handle(BuildActionStarted buildActionStartedResult)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Build action '{0}' of component '{1}' started with target: {2}", buildActionStartedResult.BuildType, buildActionStartedResult.ComponentManifest.Name, buildActionStartedResult.Target));
        }

        public void Handle(BuildActionCompletedResult buildActionCompletedResult)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Build action '{0}' of component '{1}' completed with target: {2}", buildActionCompletedResult.BuildType, buildActionCompletedResult.ComponentManifest.Name, buildActionCompletedResult.Target));
        }

        public void Handle(BuildActionErrorResult buildActionErrorResult)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Build action '{0}' of component '{1}' threw exception: {2}", buildActionErrorResult.BuildType, buildActionErrorResult.ComponentManifest.Name, buildActionErrorResult.Exception));
        }
    }
}
