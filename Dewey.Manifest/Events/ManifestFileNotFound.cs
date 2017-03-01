using Dewey.File;

namespace Dewey.Manifest.Events
{
    public class ManifestFileNotFound : ManifestFileResult
    {
        public ManifestFileNotFound(IManifestFileReader manifestFile) : base(manifestFile) { }
    }
}
