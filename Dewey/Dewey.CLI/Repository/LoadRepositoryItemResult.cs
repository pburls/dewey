using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dewey.CLI.Repository
{
    public class LoadRepositoryItemResult
    {
        public DirectoryInfo RepositoryDirectory { get; private set; }

        public FileInfo RepositoryManifestFile { get; private set; }

        public RepositoryManifest RepositoryManifest { get; private set; }

        public IEnumerable<LoadComponentElementResult> LoadComponentElementResults { get; private set; }

        private IEnumerable<string> _errorMessages;
        public IEnumerable<string> ErrorMessages
        {
            get
            {
                if (_errorMessages == null)
                {
                    _errorMessages = GetErrorMessages();
                }

                return _errorMessages;
            }
        }

        private LoadRepositoryItemResult() { }

        public static LoadRepositoryItemResult CreateDirectoryNotFoundResult(DirectoryInfo repositoryDirectory)
        {
            var result = new LoadRepositoryItemResult();
            result.RepositoryDirectory = repositoryDirectory;
            return result;
        }

        public static LoadRepositoryItemResult CreateFileNotFoundResult(DirectoryInfo repositoryDirectory, FileInfo repositoryManifestFile)
        {
            var result = new LoadRepositoryItemResult();
            result.RepositoryDirectory = repositoryDirectory;
            result.RepositoryManifestFile = repositoryManifestFile;
            return result;
        }

        public static LoadRepositoryItemResult CreateSuccessfulResult(DirectoryInfo repositoryDirectory, FileInfo repositoryManifestFile, RepositoryManifest repositoryManifest, IEnumerable<LoadComponentElementResult> loadComponentElementResults)
        {
            var result = new LoadRepositoryItemResult();
            result.RepositoryDirectory = repositoryDirectory;
            result.RepositoryManifestFile = repositoryManifestFile;
            result.RepositoryManifest = repositoryManifest;
            result.LoadComponentElementResults = loadComponentElementResults;
            return result;
        }

        private IEnumerable<string> GetErrorMessages()
        {
            if (!RepositoryDirectory.Exists) return new string[] { string.Format("Respository directory '{0}' not found.", RepositoryDirectory.FullName) };
            if (!RepositoryManifestFile.Exists) return new string[] { string.Format("Repository Manifest file '{0}' not found.", RepositoryManifestFile.FullName) };

            return LoadComponentElementResults.Where(x => x.ErrorMessage != null).Select(x => x.ErrorMessage);
        }
    }
}
