using Dewey.Messaging;

namespace Dewey.Manifest.Events
{
    public class ManifestFilesFound : IEvent
    {
        public string FileName { get; private set; }

        public ManifestFilesFound(string fileName)
        {
            FileName = fileName;
        }
    }
}
