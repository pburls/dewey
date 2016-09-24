namespace Dewey.Build
{
    public interface IMSBuildProcess
    {
        string GetMSBuildExecutablePathForVersion(string version);
        void Execute(string msbuildExecutablePath, string arguments);
    }
}