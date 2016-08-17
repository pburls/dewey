using Dewey.Manifest.Component;
using System;

namespace Dewey.Build.Events
{
    public class BuildActionErrorResult : BuildActionEvent
    {
        public Exception Exception { get; private set; }

        public BuildActionErrorResult(ComponentManifest componentManifest, string buildType, Exception exception) : base(componentManifest, buildType)
        {
            Exception = exception;
        }
    }
}
