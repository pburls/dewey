using Dewey.Manifest.Component;
using Dewey.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System;

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

                DependencyElementResult dependencyElementResult = LoadDependencyElementForType(dependencyTypeAtt.Value, dependencyNameAtt.Value, componentMandifest, dependencyElement, missingAttributes);
                if (dependencyElementResult == null)
                {
                    _eventAggregator.PublishEvent(new DependencyElementMissingAttributesResult(componentElement, missingAttributes));
                    continue;
                }

                _eventAggregator.PublishEvent(dependencyElementResult);
            }
        }

        private DependencyElementResult LoadDependencyElementForType(string type, string name, ComponentManifest componentMandifest, XElement dependencyElement, List<string> missingAttributes)
        {
            switch (type)
            {
                case ComponentDependency.COMPONENT_DEPENDENCY_TYPE:
                    return ComponentDependency.Load(type, name, componentMandifest, dependencyElement, missingAttributes);
                case QueueDependency.QUEUE_DEPENDENCY_TYPE:
                    return QueueDependency.Load(type, name, componentMandifest, dependencyElement, missingAttributes);
                case DatabaseDependency.DATABASE_DEPENDENCY_TYPE:
                    return DatabaseDependency.Load(type, name, componentMandifest, dependencyElement, missingAttributes);
                default:
                    return new DependencyElementResult(componentMandifest, dependencyElement, type, name);
            }
        }
    }
}
