using Dewey.File;

namespace Dewey.Manifest.Events
{
    public class EmptyManifestFile : ManifestFileResult
    {
        public EmptyManifestFile(IManifestFileReader manifestFile) : base(manifestFile) { }
    }
}
