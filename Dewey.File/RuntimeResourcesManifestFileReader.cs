using System;
using System.IO;

namespace Dewey.File
{
    class RuntimeResourcesManifestFileReader : ManifestFileReader
    {
        public const string DEFAULT_RUNTIME_RESOURCES_FILE_NAME = "runtime-resources.xml";

        public override ManifestFileType MandifestFileType
        {
            get
            {
                return ManifestFileType.RuntimeResources;
            }
        }

        public RuntimeResourcesManifestFileReader(params string[] paths)
        {
            var directoryPath = paths.Length == 0 ? "." : Path.Combine(paths);
            var directoryInfo = new DirectoryInfo(directoryPath);
            SetDirectoryInfo(directoryInfo);

            if (directoryInfo.Exists)
            {
                var fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, DEFAULT_RUNTIME_RESOURCES_FILE_NAME));
                SetFileInfo(fileInfo);
            }
        }
    }
}
