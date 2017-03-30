using Dewey.Manifest.Models;

namespace Dewey.Build.Events
{
    public class NoJsonBuildManifestFound : JsonBuildManifestLoadEvent
    {
        public NoJsonBuildManifestFound(Component component) : base(component) { }
    }
}
