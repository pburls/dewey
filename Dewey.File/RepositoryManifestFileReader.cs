using System.IO;

namespace Dewey.File
{
    class RepositoryManifestFileReader : ManifestFileReader
    {
        public const string DEFAULT_REPOSITORY_FILE_NAME = "repository.xml";

        public override ManifestFileType MandifestFileType
        {
            get
            {
                return ManifestFileType.Repository;
            }
        }

        public RepositoryManifestFileReader(params string[] paths)
        {
            var directoryPath = paths.Length == 0 ? "." : Path.Combine(paths);
            var directoryInfo = new DirectoryInfo(directoryPath);
            SetDirectoryInfo(directoryInfo);

            if (directoryInfo.Exists)
            {
                var fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, DEFAULT_REPOSITORY_FILE_NAME));
                SetFileInfo(fileInfo);
            }
        }
    }
}
