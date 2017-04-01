using Dewey.Build.Events;
using Dewey.Messaging;
using System;

namespace Dewey.Build
{
    class BuildCommandWriter : 
        IEventHandler<BuildCommandStarted>, 
        IEventHandler<ComponentNotFoundResult>, 
        IEventHandler<NoJsonBuildManifestFound>,
        IEventHandler<JsonBuildManifestInvalidType>,
        IEventHandler<JsonBuildMissingAttributesResult>,
        IEventHandler<JsonBuildActionTargetNotFoundResult>,
        IEventHandler<JsonMSBuildExecutableNotFoundResult>,
        IEventHandler<JsonBuildActionStarted>,
        IEventHandler<JsonBuildActionCompletedResult>,
        IEventHandler<JsonBuildActionErrorResult>,
        IEventHandler<BuildCommandSkipped>
    {
        public BuildCommandWriter(IEventAggregator eventAggregator)
        {
            eventAggregator.SubscribeAll(this);
        }

        public void Handle(BuildCommandSkipped @event)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Skipped building component '{0}'.", @event.ComponentName));
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

        public void Handle(NoJsonBuildManifestFound noJsonBuildManifestFound)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("No builds found for component '{0}' in manifest: {1}",
                noJsonBuildManifestFound.Component.name,
                noJsonBuildManifestFound.Component.ToJson()));
        }

        public void Handle(JsonBuildManifestInvalidType jsonBuildManifestInvalidType)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Invalid or unknown build type '{0}' in component manifest '{1}'",
                jsonBuildManifestInvalidType.Build.type,
                jsonBuildManifestInvalidType.Component.name));
        }

        public void Handle(JsonBuildMissingAttributesResult jsonBuildMissingAttributesResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var attributes = string.Join(", ", jsonBuildMissingAttributesResult.AttributeNames);
            Console.WriteLine($"Build of type '{jsonBuildMissingAttributesResult.Build.type}' for component '{jsonBuildMissingAttributesResult.Component.name}' is missing attributes '{attributes}' requied for action.");
        }

        public void Handle(JsonBuildActionTargetNotFoundResult jsonBuildActionTargetNotFoundResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Target file '{0}' not found for '{1}' action of component '{2}'.",
                jsonBuildActionTargetNotFoundResult.Target,
                jsonBuildActionTargetNotFoundResult.Build.type,
                jsonBuildActionTargetNotFoundResult.Component.name));
        }

        public void Handle(JsonMSBuildExecutableNotFoundResult jsonMSBuildExecutableNotFoundResult)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format("Fail to build component '{0}' because no msbuild executable version '{1}' can be found on this system.",
                jsonMSBuildExecutableNotFoundResult.Component.name,
                jsonMSBuildExecutableNotFoundResult.MSBuildVersion));
        }

        public void Handle(JsonBuildActionStarted buildActionStarted)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Build action '{0}' of component '{1}' started.",
                buildActionStarted.Build.BackingData.ToString(Newtonsoft.Json.Formatting.None),
                buildActionStarted.Component.name));
        }

        public void Handle(JsonBuildActionCompletedResult buildActionCompletedResult)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Build action '{0}' of component '{1}' completed.",
                buildActionCompletedResult.Build.BackingData.ToString(Newtonsoft.Json.Formatting.None),
                buildActionCompletedResult.Component.name));
        }

        public void Handle(JsonBuildActionErrorResult buildActionErrorResult)
        {
            Console.ResetColor();
            Console.WriteLine(string.Format("Build action '{0}' of component '{1}' threw exception: {2}",
                buildActionErrorResult.Build.type,
                buildActionErrorResult.Component.name,
                buildActionErrorResult.Exception));
        }
    }
}
