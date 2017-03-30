namespace Dewey.File
{
    public class ManifestFileReaderService : IManifestFileReaderService
    {
        public IManifestFileReader ReadDeweyManifestFile(params string[] paths)
        {
            return new DeweyManifestFileReader(paths);
        }

        public IManifestFileReader FindManifestFileInCurrentDirectory()
        {
            //Todo: Change to strategy pattern.
            if (System.IO.File.Exists(DeweyManifestFileReader.DEFAULT_FILE_NAME))
            {
                return new DeweyManifestFileReader();
            }

            return null;
        }
    }
}
