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
}
