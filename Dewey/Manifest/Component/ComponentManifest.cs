using Dewey.Manfiest;
using Dewey.Manifest.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Component
{
    public class ComponentManifest
    {
        public const string DEFAULT_COMPONENT_FILE_NAME = "component.xml";

        public string Name { get; private set; }

        public string Type { get; private set; }

        private ComponentManifest(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public static LoadComponentItemResult LoadComponentItem(ComponentItem componentItem, string rootLocation)
        {
            var componentManifestFile = new ComponentManifestFileReader(rootLocation, componentItem.RelativeLocation);
            if (!componentManifestFile.DirectoryExists || !componentManifestFile.FileExists) return LoadComponentItemResult.CreateFileNotFoundResult(componentItem, componentManifestFile);

            var rootElement = componentManifestFile.Load();

            var missingAttributes = new List<string>();

            var nameAtt = rootElement.Attribute(XName.Get("name"));
            if (nameAtt == null || string.IsNullOrWhiteSpace(nameAtt.Value))
            {
                missingAttributes.Add("name");
            }

            var typeAtt = rootElement.Attribute(XName.Get("type"));
            if (typeAtt == null || string.IsNullOrWhiteSpace(typeAtt.Value))
            {
                missingAttributes.Add("type");
            }

            if (missingAttributes.Any())
            {
                return LoadComponentItemResult.CreateMissingAttributesResult(componentItem, componentManifestFile, rootElement, missingAttributes);
            }

            var componentManifest = new ComponentManifest(nameAtt.Value, typeAtt.Value);

            return LoadComponentItemResult.CreateSuccessfulResult(componentItem, componentManifestFile, componentManifest);
        }
    }
}
