using Dewey.Messaging;
using System.Linq;
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

        public static void LoadBuildElementsFromComponentManifest(BuildCommand command, XElement componentElement, IEventAggregator eventAggregator)
        {
            var buildsElement = componentElement.Elements().FirstOrDefault(x => x.Name.LocalName == "builds");
            if (buildsElement == null)
            {
                eventAggregator.PublishEvent(new NoBuildElementsFoundResult(command, componentElement));
                return;
            }

            var buildElements = buildsElement.Elements().Where(x => x.Name.LocalName == "build").ToList();
            if (buildElements.Count == 0)
            {
                eventAggregator.PublishEvent(new NoBuildElementsFoundResult(command, componentElement));
                return;
            }

            foreach (var buildElement in buildElements)
            {
                var buildTypeAtt = buildElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "type");
                if (buildTypeAtt == null || string.IsNullOrWhiteSpace(buildTypeAtt.Value))
                {
                    eventAggregator.PublishEvent(new BuildElementMissingTypeAttributeResult(command, buildElement));
                    continue;
                }

                eventAggregator.PublishEvent(new BuildElementResult(command, buildElement, buildTypeAtt.Value));
            }
        }

        public override bool Equals(object obj)
        {
            BuildElementResult other = obj as BuildElementResult;
            if (other == null)
            {
                return false;
            }

            return base.Equals(obj) && BuildElement == other.BuildElement && BuildType == other.BuildType;
        }

        public override int GetHashCode()
        {
            return BuildElement.GetHashCode() ^ BuildType.GetHashCode() ^ base.GetHashCode();
        }

        public static bool operator ==(BuildElementResult a, BuildElementResult b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ComponentName == b.ComponentName && a.BuildElement == b.BuildElement && a.BuildType == b.BuildType;
        }

        public static bool operator !=(BuildElementResult a, BuildElementResult b)
        {
            return !(a == b);
        }
    }
}
