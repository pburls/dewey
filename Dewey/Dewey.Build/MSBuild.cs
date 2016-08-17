using Dewey.Build.Events;
using Dewey.Manifest.Component;
using Dewey.Messaging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Build
{
    class MSBuild : IBuildAction
    {
        readonly IEventAggregator _eventAggregator;

        const string msbuildPath = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";

        public const string BUILD_TYPE = "msbuild";

        public MSBuild(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Build(ComponentManifest componentManifest, XElement buildElement)
        {
            var buildTargetAtt = buildElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "target");
            if (buildTargetAtt == null || string.IsNullOrWhiteSpace(buildTargetAtt.Value))
            {
                _eventAggregator.PublishEvent(new BuildElementMissingAttributeResult(componentManifest, BUILD_TYPE, buildElement, "target"));
                return;
            }

            string buildTargetPath = Path.Combine(componentManifest.File.DirectoryName, buildTargetAtt.Value);
            if (!System.IO.File.Exists(buildTargetPath))
            {
                _eventAggregator.PublishEvent(new BuildActionTargetNotFoundResult(componentManifest, BUILD_TYPE, buildTargetPath));
                return;
            }

            _eventAggregator.PublishEvent(new BuildActionStarted(componentManifest, BUILD_TYPE, buildTargetPath));

            //read msbuild version options from registry.
            //choose version preference from app settings.

            var msBuildStartInfo = new ProcessStartInfo(msbuildPath, buildTargetPath);
            msBuildStartInfo.UseShellExecute = false;
            var msBuildProcess = Process.Start(msBuildStartInfo);

            msBuildProcess.WaitForExit();

            _eventAggregator.PublishEvent(new BuildActionCompletedResult(componentManifest, BUILD_TYPE, buildTargetPath));
        }
    }
}
