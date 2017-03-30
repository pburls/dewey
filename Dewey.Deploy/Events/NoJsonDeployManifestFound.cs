using Dewey.Manifest.Models;

namespace Dewey.Deploy.Events
{
    public class NoJsonDeployManifestFound : JsonDeployManifestLoadEvent
    {
        public NoJsonDeployManifestFound(Component component) : base(component) { }
    }
}
