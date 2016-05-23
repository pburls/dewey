using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Repositories
{
    public class RepositoriesManifest
    {
        const string DEFAULT_REPOSITORIES_FILE_NAME = "repositories.xml";

        public IEnumerable<RepositoryItem> RepositoryItems { get; private set; }

        private RepositoriesManifest() { }

        public static LoadRepositoriesManifestResult LoadRepositoriesManifestFile()
        {
            return LoadRepositoriesManifestFile(DEFAULT_REPOSITORIES_FILE_NAME);
        }

        public static LoadRepositoriesManifestResult LoadRepositoriesManifestFile(string repositoryFileName)
        {
            var repositoriesManifestFile = new FileInfo(repositoryFileName);

            if (!repositoriesManifestFile.Exists) return LoadRepositoriesManifestResult.CreateFileNotFoundResult(repositoriesManifestFile);

            var repositories = XElement.Load(repositoriesManifestFile.FullName);
            var repositoryElements = repositories.Elements().Where(x => x.Name.LocalName == "repository").ToList();

            var loadRepositoryElementResults = new List<LoadRepositoryElementResult>();
            foreach (var repoElement in repositoryElements)
            {
                loadRepositoryElementResults.Add(RepositoryItem.LoadRepositoryElement(repoElement));
            }

            var repositoriesManifest = new RepositoriesManifest();
            repositoriesManifest.RepositoryItems = loadRepositoryElementResults.Select(x => x.RepositoryItem);

            return LoadRepositoriesManifestResult.CreateSuccessfulResult(repositoriesManifestFile, repositoriesManifest, loadRepositoryElementResults);
        }
    }
}
