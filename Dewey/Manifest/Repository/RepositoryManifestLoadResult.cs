using Dewey.File;
using Dewey.Manifest.Repositories;
using Dewey.Messaging;
using System.Collections.Generic;

namespace Dewey.Manifest.Repository
{
    public class RepositoryManifestLoadResult : IEvent
    {
        public RepositoriesManifest RepositoriesManifest { get; private set; }

        public IManifestFileReader RepositoryManifestFile { get; private set; }

        public RepositoryManifest RepositoryManifest { get; private set; }

        public IEnumerable<LoadComponentElementResult> LoadComponentElementResults { get; private set; }

        public IEnumerable<string> MissingAttributes { get; private set; }

        public bool IsSuccessful { get; private set; }

        public string ErrorMessage { get; private set; }

        private RepositoryManifestLoadResult(bool isSuccessful, RepositoriesManifest repositoriesManifest, IManifestFileReader repositoryManifestFile, IEnumerable<string> missingAttributes, RepositoryManifest repositoryManifest, IEnumerable<LoadComponentElementResult> loadComponentElementResults)
        {
            IsSuccessful = isSuccessful;
            RepositoriesManifest = repositoriesManifest;
            RepositoryManifestFile = repositoryManifestFile;
            RepositoryManifest = repositoryManifest;
            LoadComponentElementResults = loadComponentElementResults ?? new List<LoadComponentElementResult>();
            MissingAttributes = missingAttributes;
            ErrorMessage = GetErrorMessage();
        }

        public static RepositoryManifestLoadResult CreateFileNotFoundResult(RepositoriesManifest repositoriesManifest, IManifestFileReader repositoryManifestFile)
        {
            return new RepositoryManifestLoadResult(false, repositoriesManifest, repositoryManifestFile, null, null, null);
        }

        internal static RepositoryManifestLoadResult CreateMissingAttributesResult(RepositoriesManifest repositoriesManifest, IManifestFileReader repositoryManifestFile, IEnumerable<string> missingAttributes)
        {
            return new RepositoryManifestLoadResult(false, repositoriesManifest, repositoryManifestFile, missingAttributes, null, null);
        }

        public static RepositoryManifestLoadResult CreateSuccessfulResult(RepositoriesManifest repositoriesManifest, IManifestFileReader repositoryManifestFile, RepositoryManifest repositoryManifest, IEnumerable<LoadComponentElementResult> loadComponentElementResults)
        {
            return new RepositoryManifestLoadResult(true, repositoriesManifest, repositoryManifestFile, null, repositoryManifest, loadComponentElementResults);
        }

        private string GetErrorMessage()
        {
            if (!RepositoryManifestFile.DirectoryExists) return string.Format("Respository directory '{0}' not found.", RepositoryManifestFile.DirectoryName);
            if (!RepositoryManifestFile.FileExists) return string.Format("Repository Manifest file '{0}' not found.", RepositoryManifestFile.FileName);

            return null;
        }
    }
}
