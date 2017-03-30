using Dewey.Manifest.Models;

namespace Dewey.Build.Events
{
    public class JsonBuildActionCompletedResult : JsonBuildEvent
    {
        public JsonBuildActionCompletedResult(Component componentManifest, Models.Build build) : base(componentManifest, build) { }
    }
}
