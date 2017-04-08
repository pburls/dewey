using Dewey.File;
using Ark3.Event;

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
