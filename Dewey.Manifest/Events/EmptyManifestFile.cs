using Dewey.File;

namespace Dewey.Manifest.Events
{
    public class EmptyManifestFile : ManifestFileResultBase
    {
        public EmptyManifestFile(IManifestFileReader manifestFile) : base(manifestFile) { }
    }
}
