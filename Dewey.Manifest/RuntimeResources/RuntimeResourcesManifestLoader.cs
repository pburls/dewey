using Dewey.File;
using Dewey.Manifest.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.RuntimeResources
{
    public static class RuntimeResourcesManifestLoader
    {
        public static RuntimeResourcesManifestLoadResult LoadManifestFileItem(ManifestFileItem manifestFileItem, IManifestFileReaderService manifestFileReaderService)
        {
            var componentManifestFile = manifestFileReaderService.ReadRuntimeResourcesManifestFile(manifestFileItem.RepositoryManifest.DirectoryName, manifestFileItem.RelativeLocation);
            return LoadRuntimeResourcesManifestFile(componentManifestFile, manifestFileItem.RepositoryManifest);
        }

        public static RuntimeResourcesManifestLoadResult LoadRuntimeResourcesManifestFile(IManifestFileReader manifestFile, RepositoryManifest repositoryManifest)
        {
            if (!manifestFile.DirectoryExists || !manifestFile.FileExists) return RuntimeResourcesManifestLoadResult.CreateFileNotFoundResult(repositoryManifest, manifestFile);

            var rootElement = manifestFile.Load();

            var missingAttributes = new List<string>();

            var nameAtt = rootElement.Attribute(XName.Get("name"));
            if (nameAtt == null || string.IsNullOrWhiteSpace(nameAtt.Value))
            {
                missingAttributes.Add("name");
            }

            if (missingAttributes.Any())
            {
                return RuntimeResourcesManifestLoadResult.CreateMissingAttributesResult(repositoryManifest, manifestFile, rootElement, missingAttributes);
            }

            var runtimeResourcesManifest = new RuntimeResourcesManifest(nameAtt.Value, manifestFile);

            return RuntimeResourcesManifestLoadResult.CreateSuccessfulResult(repositoryManifest, manifestFile, rootElement, runtimeResourcesManifest);
        }
    }
}
