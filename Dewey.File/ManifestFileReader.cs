using System.IO;

namespace Dewey.File
{
    abstract class ManifestFileReader : IManifestFileReader
    {
        public bool DirectoryExists { get; protected set; }

        public string DirectoryName { get; protected set; }

        public bool FileExists { get; protected set; }

        public string FileName { get; protected set; }

        public abstract ManifestFileType MandifestFileType { get; }

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

        public string LoadText()
        {
            using (StreamReader file = System.IO.File.OpenText(FileName))
            {
                return file.ReadToEnd();
            }
        }
    }
}
