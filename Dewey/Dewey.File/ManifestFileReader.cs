using Dewey.Manfiest;
using System.IO;
using System.Xml.Linq;

namespace Dewey.File
{
    abstract class ManifestFileReader : IManifestFileReader
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
}
