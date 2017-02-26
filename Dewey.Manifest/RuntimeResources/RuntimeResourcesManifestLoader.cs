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

            var runtimeResourcesManifest = new RuntimeResourcesManifest(nameAtt.Value, manifestFile, null);

            var runtimeResourceItemLoadResults = new List<RuntimeResourceItemLoadResult>();
            var runtimeResourceElements = rootElement.Elements().Where(x => x.Name.LocalName == "runtimeResource");
            foreach (var runtimeResourceElement in runtimeResourceElements)
            {
                runtimeResourceItemLoadResults.Add(LoadRuntimeResourceItemElement(runtimeResourceElement, runtimeResourcesManifest));
            }

            var runtimeResourceItems = runtimeResourceItemLoadResults.Where(x => x.IsSuccessful).Select(x => x.RuntimeResourceItem).ToList();
            runtimeResourcesManifest = runtimeResourcesManifest.WithRuntimeResourceItems(runtimeResourceItems);

            return RuntimeResourcesManifestLoadResult.CreateSuccessfulResult(repositoryManifest, manifestFile, rootElement, runtimeResourcesManifest, runtimeResourceItemLoadResults);
        }


        public static RuntimeResourceItemLoadResult LoadRuntimeResourceItemElement(XElement element, RuntimeResourcesManifest runtimeResourcesManifest)
        {
            var missingAttributes = new List<string>();

            var nameAtt = element.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
            if (nameAtt == null || string.IsNullOrWhiteSpace(nameAtt.Value))
            {
                missingAttributes.Add("name");
            }

            var typeAtt = element.Attributes().FirstOrDefault(x => x.Name.LocalName == "type");
            if (typeAtt == null || string.IsNullOrWhiteSpace(typeAtt.Value))
            {
                missingAttributes.Add("type");
            }

            var providerAtt = element.Attributes().FirstOrDefault(x => x.Name.LocalName == "provider");
            var contextAtt = element.Attributes().FirstOrDefault(x => x.Name.LocalName == "context");
            var formattAtt = element.Attributes().FirstOrDefault(x => x.Name.LocalName == "format");

            if (missingAttributes.Any())
            {
                return RuntimeResourceItemLoadResult.CreateMissingAttributesResult(runtimeResourcesManifest, element, missingAttributes);
            }

            var runtimeResourceItem = new RuntimeResourceItem(nameAtt.Value, typeAtt.Value, providerAtt != null ? providerAtt.Value : string.Empty, formattAtt?.Value, contextAtt != null ? contextAtt.Value : string.Empty, runtimeResourcesManifest, element);

            return RuntimeResourceItemLoadResult.CreateSuccessfulResult(runtimeResourcesManifest, element, runtimeResourceItem);
        }
    }
}
