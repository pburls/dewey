using Dewey.File;

namespace Dewey.Manifest.Events
{
    public class ManifestFileNotFound : ManifestFileEvent
    {
        public ManifestFileNotFound(IManifestFileReader manifestFile) : base(manifestFile) { }
    }
}
