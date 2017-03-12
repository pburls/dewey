using Dewey.File;

namespace Dewey.Manifest.Events
{
    public class JsonManifestLoadResult : ManifestFileEvent
    {
        public Models.Manifest Manifest { get; private set; }

        public JsonManifestLoadResult(IManifestFileReader manifestFile, Models.Manifest manifest) : base(manifestFile)
        {
            Manifest = manifest;
        }
    }
}
