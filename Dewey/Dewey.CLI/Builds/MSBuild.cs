using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.CLI.Builds
{
    class MSBuild : IBuildAction
    {
        string msbuildPath = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";

        public void Build(string target)
        {
            if (!File.Exists(target))
            {
                throw new ArgumentException(string.Format("MSBuild target '{0}' not found.", target), "target");
            }

            //read msbuild version options from registry.
            //choose version preference from app settings.

            var msBuildStartInfo = new ProcessStartInfo(msbuildPath, target);
            msBuildStartInfo.UseShellExecute = false;
            var msBuildProcess = Process.Start(msBuildStartInfo);

            msBuildProcess.WaitForExit();
        }
    }
}
