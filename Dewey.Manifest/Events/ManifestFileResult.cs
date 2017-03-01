using Dewey.File;
using Dewey.Messaging;

namespace Dewey.Manifest.Events
{
    public abstract class ManifestFileResult : IEvent
    {
        public IManifestFileReader ManifestFile { get; private set; }

        public ManifestFileResult(IManifestFileReader manifestFile)
        {
            ManifestFile = manifestFile;
        }
    }
}
