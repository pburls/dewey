using System;
using System.IO;
using System.Xml.Linq;

namespace Dewey.Manfiest
{
    public class XmlFileLoader
    {
        public bool DirectoryExists { get; protected set; }

        public string DirectoryName { get; protected set; }

        public bool FileExists { get; protected set; }

        public string FileName { get; protected set; }

        protected void SetDirectoryInfo(DirectoryInfo directoryInfo)
        {
            DirectoryExists = directoryInfo.Exists;
            DirectoryName = directoryInfo.FullName;
        }

        protected void SetFileInfo(FileInfo fileInfo)
        {
            FileExists = fileInfo.Exists;
            FileName = fileInfo.FullName;
        }

        public XElement Load()
        {
            return XElement.Load(FileName);
        }
    }

    public class RepositoriesManifestFileReader : XmlFileLoader
    {
        const string DEFAULT_REPOSITORIES_FILE_NAME = "repositories.xml";

        public RepositoriesManifestFileReader()
        {
            var fileInfo = new FileInfo(DEFAULT_REPOSITORIES_FILE_NAME);
            SetDirectoryInfo(fileInfo.Directory);
            SetFileInfo(fileInfo);
        }
    }

    public class ComponentManifestFileReader : XmlFileLoader
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

    public class RepositoryManifestFileReader : XmlFileLoader
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
