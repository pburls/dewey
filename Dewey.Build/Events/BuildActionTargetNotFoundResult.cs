using Dewey.Manifest.Models;

namespace Dewey.Build.Events
{
    public class JsonBuildActionTargetNotFoundResult : JsonBuildEvent
    {
        public string Target { get; private set; }

        public JsonBuildActionTargetNotFoundResult(Component componentManifest, Models.Build build, string fileName) : base(componentManifest, build)
        {
            Target = fileName;
        }
    }
}
