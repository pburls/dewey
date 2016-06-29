using System.IO;

namespace Dewey.File
{
    class RepositoryManifestFileReader : ManifestFileReader
    {
        public const string DEFAULT_REPOSITORY_FILE_NAME = "repository.xml";

        public RepositoryManifestFileReader(params string[] paths)
        {
            var directoryInfo = new DirectoryInfo(Path.Combine(paths));
            SetDirectoryInfo(directoryInfo);

            if (directoryInfo.Exists)
            {
                var fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, DEFAULT_REPOSITORY_FILE_NAME));
                SetFileInfo(fileInfo);
            }
        }
    }
}
