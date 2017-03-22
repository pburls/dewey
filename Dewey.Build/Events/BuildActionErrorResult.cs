using Dewey.Manifest.Component;
using Dewey.Manifest.Models;
using System;

namespace Dewey.Build.Events
{
    public class JsonBuildActionErrorResult : JsonBuildEvent
    {
        public Exception Exception { get; private set; }

        public JsonBuildActionErrorResult(Component componentManifest, Models.Build build, Exception exception) : base(componentManifest, build)
        {
            Exception = exception;
        }
    }

    public class BuildActionErrorResult : BuildActionEvent
    {
        public Exception Exception { get; private set; }

        public BuildActionErrorResult(ComponentManifest componentManifest, string buildType, Exception exception) : base(componentManifest, buildType)
        {
            Exception = exception;
        }
    }
}
