using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.CLI.Repositories
{
    public class RepositoriesManifest
    {
        public IEnumerable<LoadRepositoryElementResult> LoadRepositoryElementResults { get; private set; }

        private RepositoriesManifest(IEnumerable<LoadRepositoryElementResult> loadRepositoryElementResults)
        {
            LoadRepositoryElementResults = loadRepositoryElementResults;
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

            var repositoriesManifest = new RepositoriesManifest(loadRepositoryElementResults);

            return LoadRepositoriesManifestResult.CreateSuccessfulResult(repositoriesManifestFile, repositoriesManifest);
        }
    }
}
