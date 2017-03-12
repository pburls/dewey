namespace Dewey.File
{
    public interface IManifestFileReaderService
    {
        IManifestFileReader ReadRepositoriesManifestFile();
        IManifestFileReader ReadRepositoryManifestFile(params string[] paths);
        IManifestFileReader ReadComponentManifestFile(params string[] paths);
        IManifestFileReader ReadRuntimeResourcesManifestFile(params string[] paths);
        IManifestFileReader ReadDeweyManifestFile(params string[] paths);
        IManifestFileReader FindManifestFileInCurrentDirectory();
    }
}
