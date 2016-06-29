using Dewey.Manfiest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dewey.Manifest.Repositories
{
    public class LoadRepositoriesManifestResult
    {
        public XmlFileLoader RepositoriesManifestFile { get; private set; }

        public RepositoriesManifest RepositoriesManifest { get; private set; }

        public IEnumerable<LoadRepositoryElementResult> LoadRepositoryElementResults { get; private set; }

        public string ErrorMessage { get; private set; }

        private LoadRepositoriesManifestResult(XmlFileLoader repositoriesManifestFile, RepositoriesManifest repositoriesManifest, IEnumerable<LoadRepositoryElementResult> loadRepositoryElementResult)
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

        public static LoadRepositoriesManifestResult CreateFileNotFoundResult(XmlFileLoader repositoriesManifestFile)
        {
            return new LoadRepositoriesManifestResult(repositoriesManifestFile, null, null);
        }

        public static LoadRepositoriesManifestResult CreateSuccessfulResult(XmlFileLoader repositoriesManifestFile, RepositoriesManifest repositoriesManifest, IEnumerable<LoadRepositoryElementResult> loadRepositoryElementResult)
        {
            return new LoadRepositoriesManifestResult(repositoriesManifestFile, repositoriesManifest, loadRepositoryElementResult);
        }

        private string GetErrorMessage()
        {
            if (!RepositoriesManifestFile.FileExists) return string.Format("Manifest file '{0}' not found.", RepositoriesManifestFile.FileName);

            return null;
        }
    }
}
