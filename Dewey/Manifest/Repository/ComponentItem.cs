using Dewey.Manfiest;
using Dewey.Manifest.Component;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Repository
{
    public class ComponentItem
    {
        public string Name { get; private set; }
        public string RelativeLocation { get; private set; }

        public ComponentItem(string name, string relativeLocation)
        {
            Name = name;
            RelativeLocation = relativeLocation;
        }

        public static LoadComponentElementResult LoadComponentElement(XElement componentElement, string repositoryRoot)
        {
            var missingAttributes = new List<string>();

            var nameAtt = componentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
            if (nameAtt == null || string.IsNullOrWhiteSpace(nameAtt.Value))
            {
                missingAttributes.Add("name");
            }

            var locationAtt = componentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "location");
            if (locationAtt == null || string.IsNullOrWhiteSpace(locationAtt.Value))
            {
                missingAttributes.Add("location");
            }

            if (missingAttributes.Any())
            {
                return LoadComponentElementResult.CreateMissingAttributesResult(componentElement, missingAttributes);
            }

            var componentItem = new ComponentItem(nameAtt.Value, locationAtt.Value);

            //var loadComponentItemResult = ComponentManifest.LoadComponentItem(componentItem, repositoryRoot, manifestFileReaderService);

            return LoadComponentElementResult.CreateSuccessfulResult(componentElement, componentItem/*, loadComponentItemResult*/);
        }
    }
}
