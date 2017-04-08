namespace Dewey.Test
{
    public class MockFileService : File.IFileService
    {
        public bool FileExistsReturns { get; set; }
        public bool DirectoryExistsReturns { get; set; }

        public MockFileService()
        {
            FileExistsReturns = true;
            DirectoryExistsReturns = true;
        }

        public string CombinePaths(params string[] paths)
        {
            return System.IO.Path.Combine(paths);
        }

        public bool DirectoryExists(string path)
        {
            return DirectoryExistsReturns;
        }

        public bool FileExists(string path)
        {
            return FileExistsReturns;
        }
    }
}
