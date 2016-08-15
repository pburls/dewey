using Dewey.Manfiest;
using Dewey.Messaging;
using System;
using System.Collections.Generic;

namespace Dewey.Manifest.Repositories
{
    public class RepositoriesManifestLoadResult : IEvent
    {
        public IManifestFileReader RepositoriesManifestFile { get; private set; }

        public RepositoriesManifest RepositoriesManifest { get; private set; }

        public IEnumerable<LoadRepositoryElementResult> LoadRepositoryElementResults { get; private set; }

        public bool IsSuccessful { get; private set; }

        public string ErrorMessage { get; private set; }

        private RepositoriesManifestLoadResult(bool isSuccessful, IManifestFileReader repositoriesManifestFile, RepositoriesManifest repositoriesManifest, IEnumerable<LoadRepositoryElementResult> loadRepositoryElementResult)
        {
            if (repositoriesManifestFile == null)
            {
                throw new ArgumentNullException("repositoriesManifestFile");
            }

            IsSuccessful = isSuccessful;
            RepositoriesManifestFile = repositoriesManifestFile;
            RepositoriesManifest = repositoriesManifest;
            LoadRepositoryElementResults = loadRepositoryElementResult;
            ErrorMessage = GetErrorMessage();
        }

        public static RepositoriesManifestLoadResult CreateFileNotFoundResult(IManifestFileReader repositoriesManifestFile)
        {
            return new RepositoriesManifestLoadResult(false, repositoriesManifestFile, null, null);
        }

        public static RepositoriesManifestLoadResult CreateSuccessfulResult(IManifestFileReader repositoriesManifestFile, RepositoriesManifest repositoriesManifest, IEnumerable<LoadRepositoryElementResult> loadRepositoryElementResult)
        {
            return new RepositoriesManifestLoadResult(true, repositoriesManifestFile, repositoriesManifest, loadRepositoryElementResult);
        }

        private string GetErrorMessage()
        {
            if (!RepositoriesManifestFile.FileExists) return string.Format("Manifest file '{0}' not found.", RepositoriesManifestFile.FileName);

            return null;
        }
    }
}
