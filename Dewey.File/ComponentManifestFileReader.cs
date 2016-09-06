using System;
using System.IO;

namespace Dewey.File
{
    class ComponentManifestFileReader : ManifestFileReader
    {
        public const string DEFAULT_COMPONENT_FILE_NAME = "component.xml";

        public override ManifestFileType MandifestFileType
        {
            get
            {
                return ManifestFileType.Component;
            }
        }

        public ComponentManifestFileReader(params string[] paths)
        {
            var directoryPath = paths.Length == 0 ? "." : Path.Combine(paths);
            var directoryInfo = new DirectoryInfo(directoryPath);
            SetDirectoryInfo(directoryInfo);

            if (directoryInfo.Exists)
            {
                var fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, DEFAULT_COMPONENT_FILE_NAME));
                SetFileInfo(fileInfo);
            }
        }
    }
}
