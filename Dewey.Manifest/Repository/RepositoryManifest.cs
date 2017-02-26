using Dewey.File;
using Dewey.Manifest.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Repository
{
    public class RepositoryManifest
    {
        public string DirectoryName { get; private set; }

        public string FileName { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<ComponentItem> ComponentItems { get; private set; }

        public IEnumerable<ManifestFileItem> RuntimeResourcesItems { get; private set; }

        public RepositoryManifest (string name, string directoryName, string fileName)
        {
            Name = name;
            DirectoryName = directoryName;
            FileName = fileName;
        }

        public static RepositoryManifestLoadResult LoadRepositoryItem(RepositoryItem repositoryItem, IManifestFileReaderService manifestFileReaderService)
        {
            var repositoryManifestFile = manifestFileReaderService.ReadRepositoryManifestFile(repositoryItem.RepositoriesManifest.DirectoryName, repositoryItem.RelativeLocation);
            return LoadRepositoryManifestFile(repositoryManifestFile, repositoryItem.RepositoriesManifest);
        }

        public static RepositoryManifestLoadResult LoadRepositoryManifestFile(IManifestFileReader repositoryManifestFile, RepositoriesManifest repositoriesManifest)
        {
            if (!repositoryManifestFile.DirectoryExists || !repositoryManifestFile.FileExists) return RepositoryManifestLoadResult.CreateFileNotFoundResult(repositoriesManifest, repositoryManifestFile);

            var repository = repositoryManifestFile.Load();

            var nameAtt = repository.Attribute(XName.Get("name"));
            if (nameAtt == null || string.IsNullOrWhiteSpace(nameAtt.Value))
            {
                return RepositoryManifestLoadResult.CreateMissingAttributesResult(repositoriesManifest, repositoryManifestFile, new string[] { "name" });
            }

            var repositoryManifest = new RepositoryManifest(nameAtt.Value, repositoryManifestFile.DirectoryName, repositoryManifestFile.FileName);

            var componentsElement = repository.Elements().FirstOrDefault(x => x.Name.LocalName == "components");

            var componentItemResults = new List<LoadComponentElementResult>();
            if (componentsElement != null)
            {
                var componentElements = componentsElement.Elements().Where(x => x.Name.LocalName == "component");
                foreach (var componentElement in componentElements)
                {
                    componentItemResults.Add(ComponentItem.LoadComponentElement(componentElement, repositoryManifest));
                }
            }

            repositoryManifest.ComponentItems = componentItemResults.Where(x => x.IsSuccessful).Select(x => x.ComponentItem).ToList();

            var runtimeResourcesElement = repository.Elements().FirstOrDefault(x => x.Name.LocalName == "runtimeResources");

            var runtimeResourceLoadResults = new List<LoadManifestFileElementResult>();
            if (runtimeResourcesElement != null)
            {
                var manifestFileElements = runtimeResourcesElement.Elements().Where(x => x.Name.LocalName == "manifestFile");
                foreach (var manifestFileElement in manifestFileElements)
                {
                    runtimeResourceLoadResults.Add(LoadManifestFileElementResult.LoadManifestFileElement(manifestFileElement, repositoryManifest));
                }
            }

            repositoryManifest.RuntimeResourcesItems = runtimeResourceLoadResults.Where(x => x.IsSuccessful).Select(x => x.ManifestFileItem).ToList();

            return RepositoryManifestLoadResult.CreateSuccessfulResult(repositoriesManifest, repositoryManifestFile, repositoryManifest, componentItemResults);
        }
    }
}
