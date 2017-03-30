using Dewey.Manifest.Models;

namespace Dewey.Build.Events
{
    public class JsonBuildManifestInvalidType : JsonBuildEvent
    {
        public JsonBuildManifestInvalidType(Component component, Models.Build build) : base(component, build) { }
    }
}
