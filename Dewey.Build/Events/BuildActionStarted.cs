using Dewey.Manifest.Models;

namespace Dewey.Build.Events
{
    public class JsonBuildActionStarted : JsonBuildEvent
    {
        public JsonBuildActionStarted(Component componentManifest, Models.Build build) : base(componentManifest, build) { }
    }
}
