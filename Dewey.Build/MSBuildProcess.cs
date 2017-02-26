using Microsoft.Build.Utilities;
using System.Diagnostics;

namespace Dewey.Build
{
    public class MSBuildProcess : IMSBuildProcess
    {
        //const string MS_BUILD_PATH = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";
        public string GetMSBuildExecutablePathForVersion(string version)
        {
            return ToolLocationHelper.GetPathToBuildToolsFile("msbuild.exe", version, DotNetFrameworkArchitecture.Current);
        }

        public bool Execute(string msbuildExecutablePath, string arguments)
        {
            var msBuildStartInfo = new ProcessStartInfo(msbuildExecutablePath, arguments);
            msBuildStartInfo.UseShellExecute = false;
            var msBuildProcess = Process.Start(msBuildStartInfo);

            msBuildProcess.WaitForExit();

            return msBuildProcess.ExitCode == 0;
        }
    }
}
