using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Build
{
    public class MSBuildProcess : IMSBuildProcess
    {
        const string MS_BUILD_PATH = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";

        public void Execute(string arguments)
        {
            //read msbuild version options from registry.
            //choose version preference from app settings.

            var msBuildStartInfo = new ProcessStartInfo(MS_BUILD_PATH, arguments);
            msBuildStartInfo.UseShellExecute = false;
            var msBuildProcess = Process.Start(msBuildStartInfo);

            msBuildProcess.WaitForExit();
        }
    }
}
