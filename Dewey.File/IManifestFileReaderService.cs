namespace Dewey.File
{
    public interface IManifestFileReaderService
    {
        IManifestFileReader ReadDeweyManifestFile(params string[] paths);
        IManifestFileReader FindManifestFileInCurrentDirectory();
    }
}
