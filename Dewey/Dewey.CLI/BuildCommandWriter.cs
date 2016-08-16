using Dewey.Build.Events;
using Dewey.Messaging;
using System;

namespace Dewey.CLI
{
    class BuildCommandWriter : 
        IEventHandler<BuildCommandStarted>, 
        IEventHandler<ComponentNotFoundResult>, 
        IEventHandler<NoBuildElementsFoundResult>, 
        IEventHandler<BuildElementMissingTypeAttributeResult>
    {
        public BuildCommandWriter(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe<BuildCommandStarted>(this);
            eventAggregator.Subscribe<ComponentNotFoundResult>(this);
            eventAggregator.Subscribe<NoBuildElementsFoundResult>(this);
            eventAggregator.Subscribe<BuildElementMissingTypeAttributeResult>(this);
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
            Console.WriteLine(string.Format("No builds found for component '{0}'.", noBuildElementsFoundResult.ComponentName));
        }

        public void Handle(BuildElementMissingTypeAttributeResult buildElementMissingTypeAttributeResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Skipping build element without a valid type: {0}", buildElementMissingTypeAttributeResult.BuildElement.ToString()));
        }
    }
}
