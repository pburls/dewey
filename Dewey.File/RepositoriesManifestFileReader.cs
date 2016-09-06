using System.IO;

namespace Dewey.File
{
    class RepositoriesManifestFileReader : ManifestFileReader
    {
        public const string DEFAULT_REPOSITORIES_FILE_NAME = "repositories.xml";

        public override ManifestFileType MandifestFileType
        {
            get
            {
                return ManifestFileType.Repositories;
            }
        }

        public RepositoriesManifestFileReader()
        {
            var fileInfo = new FileInfo(DEFAULT_REPOSITORIES_FILE_NAME);
            SetDirectoryInfo(fileInfo.Directory);
            SetFileInfo(fileInfo);
        }
    }
}
