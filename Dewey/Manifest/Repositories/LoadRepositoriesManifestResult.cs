using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dewey.Manifest.Repositories
{
    public class LoadRepositoriesManifestResult
    {
        public FileInfo RepositoriesManifestFile { get; private set; }

        public RepositoriesManifest RepositoriesManifest { get; private set; }

        public IEnumerable<LoadRepositoryElementResult> LoadRepositoryElementResults { get; private set; }

        public string ErrorMessage { get; private set; }

        private LoadRepositoriesManifestResult(FileInfo repositoriesManifestFile, RepositoriesManifest repositoriesManifest, IEnumerable<LoadRepositoryElementResult> loadRepositoryElementResult)
        {
            if (repositoriesManifestFile == null)
            {
                throw new ArgumentNullException("repositoriesManifestFile");
            }

            RepositoriesManifestFile = repositoriesManifestFile;
            RepositoriesManifest = repositoriesManifest;
            LoadRepositoryElementResults = loadRepositoryElementResult;
            ErrorMessage = GetErrorMessage();
        }

        public static LoadRepositoriesManifestResult CreateFileNotFoundResult(FileInfo repositoriesManifestFile)
        {
            return new LoadRepositoriesManifestResult(repositoriesManifestFile, null, null);
        }

        public static LoadRepositoriesManifestResult CreateSuccessfulResult(FileInfo repositoriesManifestFile, RepositoriesManifest repositoriesManifest, IEnumerable<LoadRepositoryElementResult> loadRepositoryElementResult)
        {
            return new LoadRepositoriesManifestResult(repositoriesManifestFile, repositoriesManifest, loadRepositoryElementResult);
        }

        private string GetErrorMessage()
        {
            if (!RepositoriesManifestFile.Exists) return string.Format("Manifest file '{0}' not found.", RepositoriesManifestFile.FullName);

            return null;
        }
    }
}
