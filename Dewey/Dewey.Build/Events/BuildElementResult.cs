using System.Xml.Linq;

namespace Dewey.Build.Events
{
    public class BuildElementResult : BuildCommandEventBase
    {
        public XElement BuildElement { get; private set; }
        public string BuildType { get; private set; }

        public BuildElementResult(string componentName, XElement buildElement, string buildType) : base(componentName)
        {
            BuildElement = buildElement;
            BuildType = buildType;
        }
    }
}
