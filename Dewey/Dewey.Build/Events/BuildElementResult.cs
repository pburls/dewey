using System.Xml.Linq;

namespace Dewey.Build.Events
{
    public class BuildElementResult : BuildCommandEvent
    {
        public XElement BuildElement { get; private set; }
        public string BuildType { get; private set; }

        public BuildElementResult(BuildCommand command, XElement buildElement, string buildType) : base(command)
        {
            BuildElement = buildElement;
            BuildType = buildType;
        }
    }
}
