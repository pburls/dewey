using Dewey.Manifest.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var componentDirectory = new DirectoryInfo(Path.Combine(rootLocation, componentItem.RelativeLocation));
            if (!componentDirectory.Exists) return LoadComponentItemResult.CreateDirectoryNotFoundResult(componentItem, componentDirectory);

            var componentManifestFile = new FileInfo(Path.Combine(componentDirectory.FullName, DEFAULT_COMPONENT_FILE_NAME));
            if (!componentManifestFile.Exists) return LoadComponentItemResult.CreateFileNotFoundResult(componentItem, componentDirectory, componentManifestFile);

            var rootElement = XElement.Load(componentManifestFile.FullName);

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
                return LoadComponentItemResult.CreateMissingAttributesResult(componentItem, componentDirectory, componentManifestFile, rootElement, missingAttributes);
            }

            var componentManifest = new ComponentManifest(nameAtt.Value, typeAtt.Value);

            return LoadComponentItemResult.CreateSuccessfulResult(componentItem, componentDirectory, componentManifestFile, componentManifest);
        }
    }
}
