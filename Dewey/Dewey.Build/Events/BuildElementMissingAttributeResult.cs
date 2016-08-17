using Dewey.Manifest.Component;
using System.Xml.Linq;

namespace Dewey.Build.Events
{
    public class BuildElementMissingAttributeResult : BuildActionResult
    {
        public XElement BuildElement { get; private set; }

        public string AttributeName { get; private set; }

        public BuildElementMissingAttributeResult(ComponentManifest componentManifest, string buildType, XElement buildElement, string attributeName) : base(componentManifest, buildType)
        {
            AttributeName = attributeName;
        }
    }
}
