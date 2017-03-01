using Dewey.File;

namespace Dewey.Manifest.Events
{
    public class InvalidManifestFile : ManifestFileResult
    {
        public InvalidManifestFile(IManifestFileReader manifestFile) : base(manifestFile) { }
    }
}
