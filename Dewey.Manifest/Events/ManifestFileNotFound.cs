using Dewey.File;

namespace Dewey.Manifest.Events
{
    public class ManifestFileNotFound : ManifestFileResultBase
    {
        public ManifestFileNotFound(IManifestFileReader manifestFile) : base(manifestFile) { }
    }
}
