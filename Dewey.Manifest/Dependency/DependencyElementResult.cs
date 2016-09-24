using Dewey.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Dependency
{
    public class DependencyElementResult : DependencyElementEvent
    {
        public const string COMPONENT_DEPENDENCY_TYPE = "component";

        public XElement DependencyElement { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }

        public DependencyElementResult(XElement dependencyElement, string type, string name)
        {
            DependencyElement = dependencyElement;
            Type = type;
            Name = name;
        }

        public static void LoadDependencies(XElement componentElement, IEventAggregator eventAggregator)
        {
            var dependenciesElement = componentElement.Elements().FirstOrDefault(x => x.Name.LocalName == "dependencies");
            if (dependenciesElement == null)
            {
                eventAggregator.PublishEvent(new DependenciesElementNotFoundResult(componentElement));
                return;
            }

            var dependencyElements = dependenciesElement.Elements().Where(x => x.Name.LocalName == "dependency").ToList();
            if (dependencyElements.Count == 0)
            {
                eventAggregator.PublishEvent(new DependencyElementsNotFoundResult(componentElement));
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
                    eventAggregator.PublishEvent(new DependencyElementMissingAttributesResult(componentElement, missingAttributes));
                    continue;
                }

                eventAggregator.PublishEvent(new DependencyElementResult(dependencyElement, dependencyTypeAtt.Value, dependencyNameAtt.Value));
            }
        }
    }
}