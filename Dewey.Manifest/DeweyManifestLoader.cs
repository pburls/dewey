using Dewey.File;
using Dewey.Manifest.Events;
using Dewey.Messaging;
using System.Linq;

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
            var manifestFilesElement = childElements.FirstOrDefault(x => x.Name == "manifestFiles");
            var componentsElement = childElements.FirstOrDefault(x => x.Name == "components");
            var runtimeResourcesElement = childElements.FirstOrDefault(x => x.Name == "runtimeResources");
            if (manifestFilesElement != null || componentsElement != null || runtimeResourcesElement != null)
            {
                return new ManifestFileResult(manifestFile, manifestFilesElement, componentsElement, runtimeResourcesElement);
            }

            return new EmptyManifestFile(manifestFile);
        }
    }
}
