using Dewey.Manfiest;

namespace Dewey.File
{
    public class ManifestFileReaderService : IManifestFileReaderService
    {
        public IManifestFileReader ReadComponentManifestFile(params string[] paths)
        {
            return new ComponentManifestFileReader(paths);
        }

        public IManifestFileReader ReadRepositoriesManifestFile()
        {
            return new RepositoriesManifestFileReader();
        }

        public IManifestFileReader ReadRepositoryManifestFile(params string[] paths)
        {
            return new RepositoryManifestFileReader(paths);
        }
    }
}
