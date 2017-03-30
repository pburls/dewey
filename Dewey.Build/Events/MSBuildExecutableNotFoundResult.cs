using Dewey.Manifest.Models;

namespace Dewey.Build.Events
{
    public class JsonMSBuildExecutableNotFoundResult : JsonBuildEvent
    {
        public string MSBuildVersion { get; private set; }

        public JsonMSBuildExecutableNotFoundResult(Component componentManifest, Models.Build build, string msbuildVersion) : base(componentManifest, build)
        {
            MSBuildVersion = msbuildVersion;
        }
    }
}
