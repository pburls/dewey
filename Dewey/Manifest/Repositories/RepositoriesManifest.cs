using Dewey.Manfiest;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Repositories
{
    public class RepositoriesManifest
    {

        public IEnumerable<RepositoryItem> RepositoryItems { get; private set; }

        private RepositoriesManifest() { }

        public static LoadRepositoriesManifestResult LoadRepositoriesManifestFile()
        {
            var repositoriesManifestFile = new RepositoriesManifestFileReader();

            if (!repositoriesManifestFile.FileExists) return LoadRepositoriesManifestResult.CreateFileNotFoundResult(repositoriesManifestFile);

            var repositories = repositoriesManifestFile.Load();
            var repositoryElements = repositories.Elements().Where(x => x.Name.LocalName == "repository").ToList();

            var loadRepositoryElementResults = new List<LoadRepositoryElementResult>();
            foreach (var repoElement in repositoryElements)
            {
                loadRepositoryElementResults.Add(RepositoryItem.LoadRepositoryElement(repoElement, repositoriesManifestFile.DirectoryName));
            }

            var repositoriesManifest = new RepositoriesManifest();
            repositoriesManifest.RepositoryItems = loadRepositoryElementResults.Where(x => x.RepositoryItem != null).Select(x => x.RepositoryItem);

            return LoadRepositoriesManifestResult.CreateSuccessfulResult(repositoriesManifestFile, repositoriesManifest, loadRepositoryElementResults);
        }
    }
}
