using Dewey.Messaging;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Dewey.Manifest.Dependency
{
    public class DependencyElementMissingAttributesResult : DependencyElementEvent
    {
        public XElement DependencyElement { get; private set; }
        public IEnumerable<string> AttributeNames { get; private set; }

        public DependencyElementMissingAttributesResult(XElement dependencyElement, IEnumerable<string> attributeNames)
        {
            DependencyElement = dependencyElement;
            AttributeNames = attributeNames;
        }
    }
}