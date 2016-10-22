using Dewey.Manifest.Component;
using Dewey.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Dependency
{
    public class DependencyElementLoader : IDependencyElementLoader
    {
        private readonly IEventAggregator _eventAggregator;

        public DependencyElementLoader(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void LoadFromComponentManifest(ComponentManifest componentMandifest, XElement componentElement)
        {
            var dependenciesElement = componentElement.Elements().FirstOrDefault(x => x.Name.LocalName == "dependencies");
            if (dependenciesElement == null)
            {
                _eventAggregator.PublishEvent(new DependenciesElementNotFoundResult(componentElement));
                return;
            }

            var dependencyElements = dependenciesElement.Elements().Where(x => x.Name.LocalName == "dependency").ToList();
            if (dependencyElements.Count == 0)
            {
                _eventAggregator.PublishEvent(new DependencyElementsNotFoundResult(componentElement));
                return;
            }

            foreach (var dependencyElement in dependencyElements)
            {
                var missingAttributes = new List<string>();
                var dependencyTypeAtt = dependencyElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "type");
                if (dependencyTypeAtt == null || string.IsNullOrWhiteSpace(dependencyTypeAtt.Value))
                {
                    missingAttributes.Add("type");
                }

                var dependencyNameAtt = dependencyElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
                if (dependencyNameAtt == null || string.IsNullOrWhiteSpace(dependencyNameAtt.Value))
                {
                    missingAttributes.Add("name");
                }

                if (missingAttributes.Any())
                {
                    _eventAggregator.PublishEvent(new DependencyElementMissingAttributesResult(componentElement, missingAttributes));
                    continue;
                }

                _eventAggregator.PublishEvent(new DependencyElementResult(componentMandifest, dependencyElement, dependencyTypeAtt.Value, dependencyNameAtt.Value));
            }
        }
    }
}
