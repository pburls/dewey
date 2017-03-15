using Dewey.File;
using Dewey.Manifest.Events;
using Newtonsoft.Json;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest
{
    public static class DeweyManifestLoader
    {
        public static ManifestFileEvent LoadDeweyManifest(IManifestFileReader manifestFile)
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
                return new ManifestLoadResult(manifestFile, manifestFilesElement, componentsElement, runtimeResourcesElement);
            }

            return new EmptyManifestFile(manifestFile);
        }

        public static void LoadManifestFilesElement(XElement manifestFilesElement)
        {

        }

        public static ManifestFileEvent LoadJsonDeweyManifest(IManifestFileReader manifestFile)
        {
            if (!manifestFile.DirectoryExists || !manifestFile.FileExists) return new ManifestFileNotFound(manifestFile);

            try
            {
                var manifest = Models.Manifest.FromJson(manifestFile.LoadText());
                return new JsonManifestLoadResult(manifestFile, manifest);
            }
            catch (System.Exception ex)
            {
                return new InvalidJsonManifestFile(manifestFile, ex);
            }
        }
    }


}
