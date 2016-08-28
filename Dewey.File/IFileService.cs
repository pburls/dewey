namespace Dewey.File
{
    public interface IFileService
    {
        bool FileExists(string path);

        bool DirectoryExists(string path);

        string CombinePaths(params string[] paths);
    }
}
