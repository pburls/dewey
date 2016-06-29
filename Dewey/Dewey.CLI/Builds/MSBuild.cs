using Dewey.Manifest.Component;
using Dewey.Manifest.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dewey.CLI.Builds
{
    class MSBuild : IBuildAction
    {
        string msbuildPath = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";

        public void Build(ComponentItem repoComponent, ComponentManifest componentManifest, XElement buildElement)
        {
            var buildTargetAtt = buildElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "target");
            if (buildTargetAtt == null || string.IsNullOrWhiteSpace(buildTargetAtt.Value))
            {
                throw new ArgumentException(string.Format("MSBuild element without a valid target: {0}", buildElement.ToString()), "buildElement");
            }

            string buildTargetPath = Path.Combine(repoComponent.RelativeLocation, buildTargetAtt.Value);
            if (!System.IO.File.Exists(buildTargetPath))
            {
                throw new ArgumentException(string.Format("MSBuild target '{0}' not found.", buildTargetAtt.Value), "buildElement");
            }

            //read msbuild version options from registry.
            //choose version preference from app settings.

            var msBuildStartInfo = new ProcessStartInfo(msbuildPath, buildTargetPath);
            msBuildStartInfo.UseShellExecute = false;
            var msBuildProcess = Process.Start(msBuildStartInfo);

            msBuildProcess.WaitForExit();
        }
    }
}
