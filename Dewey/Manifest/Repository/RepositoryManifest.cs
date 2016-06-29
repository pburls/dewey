using Dewey.Manfiest;
using Dewey.Manifest.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Repository
{
    public class RepositoryManifest
    {
        public IEnumerable<ComponentItem> ComponentItems { get; private set; }

        private RepositoryManifest () { }

        public static LoadRepositoryItemResult LoadRepositoryItem(RepositoryItem repositoryItem, string rootLocation, IManifestFileReaderService manifestFileReaderService)
        {
            var repositoryManifestFile = manifestFileReaderService.ReadRepositoryManifestFile(rootLocation, repositoryItem.RelativeLocation);
            if (!repositoryManifestFile.DirectoryExists || !repositoryManifestFile.FileExists) return LoadRepositoryItemResult.CreateFileNotFoundResult(repositoryItem, repositoryManifestFile);

            var repository = repositoryManifestFile.Load();
            var componentsElement = repository.Elements().FirstOrDefault(x => x.Name.LocalName == "components");

            var componentItemResults = new List<LoadComponentElementResult>();
            if (componentsElement != null)
            {
                var componentElements = componentsElement.Elements().Where(x => x.Name.LocalName == "component");
                foreach (var componentElement in componentElements)
                {
                    componentItemResults.Add(ComponentItem.LoadComponentElement(componentElement, repositoryManifestFile.DirectoryName, manifestFileReaderService));
                }
            }

            var repositoryManifest = new RepositoryManifest();
            repositoryManifest.ComponentItems = componentItemResults.Select(x => x.ComponentItem);

            return LoadRepositoryItemResult.CreateSuccessfulResult(repositoryItem, repositoryManifestFile, repositoryManifest, componentItemResults);
        }
    }
}
