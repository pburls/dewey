using Dewey.File;

namespace Dewey.Manifest.Events
{
    public class InvalidManifestFile : ManifestFileEvent
    {
        public InvalidManifestFile(IManifestFileReader manifestFile) : base(manifestFile) { }
    }
}
