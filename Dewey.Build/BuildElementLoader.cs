using Dewey.Build.Events;
using Dewey.Messaging;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Build
{
    public class BuildElementLoader : IBuildElementLoader
    {
        private readonly IEventAggregator _eventAggregator;

        public BuildElementLoader(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void LoadFromComponentManifest(BuildCommand command, XElement componentElement)
        {
            var buildsElement = componentElement.Elements().FirstOrDefault(x => x.Name.LocalName == "builds");
            if (buildsElement == null)
            {
                _eventAggregator.PublishEvent(new NoBuildElementsFoundResult(command, componentElement));
                return;
            }

            var buildElements = buildsElement.Elements().Where(x => x.Name.LocalName == "build").ToList();
            if (buildElements.Count == 0)
            {
                _eventAggregator.PublishEvent(new NoBuildElementsFoundResult(command, componentElement));
                return;
            }

            foreach (var buildElement in buildElements)
            {
                var buildTypeAtt = buildElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "type");
                if (buildTypeAtt == null || string.IsNullOrWhiteSpace(buildTypeAtt.Value))
                {
                    _eventAggregator.PublishEvent(new BuildElementMissingTypeAttributeResult(command, buildElement));
                    continue;
                }

                _eventAggregator.PublishEvent(new BuildElementResult(command, buildElement, buildTypeAtt.Value));
            }
        }
    }
}
