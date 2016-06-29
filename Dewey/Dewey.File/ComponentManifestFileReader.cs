using System.IO;

namespace Dewey.File
{
    class ComponentManifestFileReader : ManifestFileReader
    {
        public const string DEFAULT_COMPONENT_FILE_NAME = "component.xml";

        public ComponentManifestFileReader(params string[] paths)
        {
            var directoryInfo = new DirectoryInfo(Path.Combine(paths));
            SetDirectoryInfo(directoryInfo);

            if (directoryInfo.Exists)
            {
                var fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, DEFAULT_COMPONENT_FILE_NAME));
                SetFileInfo(fileInfo);
            }
        }
    }
}
