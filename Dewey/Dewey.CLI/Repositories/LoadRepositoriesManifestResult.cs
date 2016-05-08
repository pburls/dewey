using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dewey.CLI.Repositories
{
    public class LoadRepositoriesManifestResult
    {
        public FileInfo RepositoriesManifestFile { get; private set; }

        public bool FileNotFound { get; private set; }

        public RepositoriesManifest RepositoriesManifest { get; private set; }

        public IEnumerable<string> ErrorMessages { get; private set; }

        private LoadRepositoriesManifestResult(FileInfo repositoriesManifestFile, bool fileNotFound, RepositoriesManifest repositoriesManifest)
        {
            RepositoriesManifestFile = repositoriesManifestFile;
            FileNotFound = fileNotFound;
            RepositoriesManifest = repositoriesManifest;
            ErrorMessages = GetErrorMessages();
        }

        public static LoadRepositoriesManifestResult CreateFileNotFoundResult(FileInfo repositoriesManifestFile)
        {
            return new LoadRepositoriesManifestResult(repositoriesManifestFile, true, null);
        }

        public static LoadRepositoriesManifestResult CreateSuccessfulResult(FileInfo repositoriesManifestFile, RepositoriesManifest repositoriesManifest)
        {
            return new LoadRepositoriesManifestResult(repositoriesManifestFile, false, repositoriesManifest);
        }

        private IEnumerable<string> GetErrorMessages()
        {
            if (FileNotFound) return new string[] { string.Format("Manifest file '{0}' not found.", RepositoriesManifestFile.FullName) };

            return RepositoriesManifest.LoadRepositoryElementResults.Where(x => x.ErrorMessage != null).Select(x => x.ErrorMessage);
        }
    }
}
