using Dewey.File;
using Dewey.Messaging;

namespace Dewey.Manifest.Events
{
    public abstract class ManifestFileEvent : IEvent
    {
        public IManifestFileReader ManifestFile { get; private set; }

        public ManifestFileEvent(IManifestFileReader manifestFile)
        {
            ManifestFile = manifestFile;
        }
    }
}
