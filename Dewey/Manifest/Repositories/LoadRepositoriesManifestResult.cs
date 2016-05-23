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

        private LoadRepositoriesManifestResult()
        {

        }

        public static LoadRepositoriesManifestResult CreateFileNotFoundResult(FileInfo repositoriesManifestFile)
        {
            var result = new LoadRepositoriesManifestResult();
            result.RepositoriesManifestFile = repositoriesManifestFile;
            return result;
        }

        public static LoadRepositoriesManifestResult CreateSuccessfulResult(FileInfo repositoriesManifestFile, RepositoriesManifest repositoriesManifest, IEnumerable<LoadRepositoryElementResult> loadRepositoryElementResult)
        {
            var result = new LoadRepositoriesManifestResult();
            result.RepositoriesManifestFile = repositoriesManifestFile;
            result.RepositoriesManifest = repositoriesManifest;
            result.LoadRepositoryElementResults = loadRepositoryElementResult;
            return result;
        }

        private IEnumerable<string> GetErrorMessages()
        {
            if (!RepositoriesManifestFile.Exists) return new string[] { string.Format("Manifest file '{0}' not found.", RepositoriesManifestFile.FullName) };

            return LoadRepositoryElementResults.Where(x => x.ErrorMessage != null).Select(x => x.ErrorMessage);
        }
    }
}
