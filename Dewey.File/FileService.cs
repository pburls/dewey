using System;

namespace Dewey.File
{
    public class FileService : IFileService
    {
        public string CombinePaths(params string[] paths)
        {
            return System.IO.Path.Combine(paths);
        }

        public bool DirectoryExists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public bool FileExists(string path)
        {
            return System.IO.File.Exists(path);
        }
    }
}
