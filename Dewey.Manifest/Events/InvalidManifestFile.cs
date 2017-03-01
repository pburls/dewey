using Dewey.File;

namespace Dewey.Manifest.Events
{
    public class InvalidManifestFile : ManifestFileResultBase
    {
        public InvalidManifestFile(IManifestFileReader manifestFile) : base(manifestFile) { }
    }
}
