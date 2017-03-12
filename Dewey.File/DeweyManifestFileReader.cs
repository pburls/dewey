using System;
using System.IO;

namespace Dewey.File
{
    class DeweyManifestFileReader : ManifestFileReader
    {
        public const string DEFAULT_FILE_NAME = "dewey.json";

        public override ManifestFileType MandifestFileType
        {
            get
            {
                return ManifestFileType.Dewey;
            }
        }

        public DeweyManifestFileReader(params string[] paths)
        {
            var directoryPath = paths.Length == 0 ? "." : Path.Combine(paths);
            var directoryInfo = new DirectoryInfo(directoryPath);
            SetDirectoryInfo(directoryInfo);

            if (directoryInfo.Exists)
            {
                var fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, DEFAULT_FILE_NAME));
                SetFileInfo(fileInfo);
            }
        }
    }
}
