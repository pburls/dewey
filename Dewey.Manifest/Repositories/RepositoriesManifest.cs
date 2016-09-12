using Dewey.File;
using System.Collections.Generic;
using System.Linq;

namespace Dewey.Manifest.Repositories
{
    public class RepositoriesManifest
    {
        public string DirectoryName { get; private set; }

        public string FileName { get; private set; }

        public IEnumerable<RepositoryItem> RepositoryItems { get; private set; }

        private RepositoriesManifest(string directoryName, string fileName)
        {
            DirectoryName = directoryName;
            FileName = fileName;
        }

        public static RepositoriesManifestLoadResult LoadRepositoriesManifestFile(IManifestFileReader repositoriesManifestFile)
        {
            if (!repositoriesManifestFile.FileExists) return RepositoriesManifestLoadResult.CreateFileNotFoundResult(repositoriesManifestFile);

            var repositories = repositoriesManifestFile.Load();

            var repositoriesManifest = new RepositoriesManifest(repositoriesManifestFile.DirectoryName, repositoriesManifestFile.FileName);

            var repositoryElements = repositories.Elements().Where(x => x.Name.LocalName == "repository").ToList();

            var loadRepositoryElementResults = new List<LoadRepositoryElementResult>();
            foreach (var repoElement in repositoryElements)
            {
                loadRepositoryElementResults.Add(RepositoryItem.LoadRepositoryElement(repoElement, repositoriesManifest));
            }
            repositoriesManifest.RepositoryItems = loadRepositoryElementResults.Where(x => x.RepositoryItem != null).Select(x => x.RepositoryItem);

            return RepositoriesManifestLoadResult.CreateSuccessfulResult(repositoriesManifestFile, repositoriesManifest, loadRepositoryElementResults);
        }
    }
}
