using System.Xml.Linq;

namespace Dewey.File
{
    public enum ManifestFileType
    {
        Unknown,
        Component,
        Repository,
        Repositories
    }

    public interface IManifestFileReader
    {
        ManifestFileType MandifestFileType { get; }

        bool DirectoryExists { get; }
        string DirectoryName { get; }
        bool FileExists { get; }
        string FileName { get; }

        XElement Load();
    }
}