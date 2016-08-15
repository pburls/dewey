using System.Xml.Linq;

namespace Dewey.File
{
    public interface IManifestFileReaderService
    {
        IManifestFileReader ReadRepositoriesManifestFile();
        IManifestFileReader ReadRepositoryManifestFile(params string[] paths);
        IManifestFileReader ReadComponentManifestFile(params string[] paths);
    }

    public interface IManifestFileReader
    {
        bool DirectoryExists { get; }
        string DirectoryName { get; }
        bool FileExists { get; }
        string FileName { get; }

        XElement Load();
    }
}