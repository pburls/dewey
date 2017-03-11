using Dewey.File;

namespace Dewey.Manifest.Events
{
    public class EmptyManifestFile : ManifestFileEvent
    {
        public EmptyManifestFile(IManifestFileReader manifestFile) : base(manifestFile) { }
    }
}
