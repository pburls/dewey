using System.IO;

namespace Dewey.File
{
    class RepositoriesManifestFileReader : ManifestFileReader
    {
        const string DEFAULT_REPOSITORIES_FILE_NAME = "repositories.xml";

        public RepositoriesManifestFileReader()
        {
            var fileInfo = new FileInfo(DEFAULT_REPOSITORIES_FILE_NAME);
            SetDirectoryInfo(fileInfo.Directory);
            SetFileInfo(fileInfo);
        }
    }
}
