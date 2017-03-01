using Dewey.File;
using Dewey.Manifest.Events;
using Dewey.Messaging;

namespace Dewey.Manifest
{
    public static class DeweyManifestLoader
    {
        public static IEvent LoadDeweyManifest(IManifestFileReader manifestFile)
        {
            if (!manifestFile.DirectoryExists || !manifestFile.FileExists) return new ManifestFileNotFound(manifestFile);

            var rootElement = manifestFile.Load();
            if (rootElement.Name != "deweyManifest")
            {
                return new InvalidManifestFile(manifestFile);
            }

            var childElements = rootElement.Elements();

            return new EmptyManifestFile(manifestFile);
        }
    }
}
