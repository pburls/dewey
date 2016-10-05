namespace Dewey.Build
{
    public interface IMSBuildProcess
    {
        string GetMSBuildExecutablePathForVersion(string version);
        bool Execute(string msbuildExecutablePath, string arguments);
    }
}