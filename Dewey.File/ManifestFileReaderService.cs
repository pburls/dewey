using System;

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

        public IManifestFileReader ReadRuntimeResourcesManifestFile(params string[] paths)
        {
            return new RuntimeResourcesManifestFileReader(paths);
        }

        public IManifestFileReader FindManifestFileInCurrentDirectory()
        {
            //Todo: Change to strategy pattern.
            if (System.IO.File.Exists(RepositoriesManifestFileReader.DEFAULT_REPOSITORIES_FILE_NAME))
            {
                return new RepositoriesManifestFileReader();
            }

            if (System.IO.File.Exists(RepositoryManifestFileReader.DEFAULT_REPOSITORY_FILE_NAME))
            {
                return new RepositoryManifestFileReader();
            }

            if (System.IO.File.Exists(ComponentManifestFileReader.DEFAULT_COMPONENT_FILE_NAME))
            {
                return new ComponentManifestFileReader();
            }

            return null;
        }
    }
}
