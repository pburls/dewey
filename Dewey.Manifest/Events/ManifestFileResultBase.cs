using Dewey.File;
using Dewey.Messaging;

namespace Dewey.Manifest.Events
{
    public abstract class ManifestFileResultBase : IEvent
    {
        public IManifestFileReader ManifestFile { get; private set; }

        public ManifestFileResultBase(IManifestFileReader manifestFile)
        {
            ManifestFile = manifestFile;
        }
    }
}
