using Dewey.File;
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

        public string SubType { get; private set; }

        public string Context { get; private set; }

        public IManifestFileReader File { get; private set; }

        public ComponentManifest(string name, string type, string subType, string context, IManifestFileReader file)
        {
            Name = name;
            Type = type;
            File = file;
            SubType = subType;
            Context = context;
        }

        public ComponentManifest WithName(string name)
        {
            return new ComponentManifest(name, Type, SubType, Context, File);
        }

        public static ComponentManifestLoadResult LoadComponentItem(ComponentItem componentItem, IManifestFileReaderService manifestFileReaderService)
        {
            var componentManifestFile = manifestFileReaderService.ReadComponentManifestFile(componentItem.RepositoryManifest.DirectoryName, componentItem.RelativeLocation);
            return LoadComponentManifestFile(componentManifestFile, componentItem.RepositoryManifest);
        }

        public static ComponentManifestLoadResult LoadComponentManifestFile(IManifestFileReader componentManifestFile, RepositoryManifest repositoryManifest)
        {
            if (!componentManifestFile.DirectoryExists || !componentManifestFile.FileExists) return ComponentManifestLoadResult.CreateFileNotFoundResult(repositoryManifest, componentManifestFile);

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

            var subTypeAtt = rootElement.Attribute(XName.Get("sub-type"));
            var contextAtt = rootElement.Attribute(XName.Get("context"));

            if (missingAttributes.Any())
            {
                return ComponentManifestLoadResult.CreateMissingAttributesResult(repositoryManifest, componentManifestFile, rootElement, missingAttributes);
            }

            var componentManifest = new ComponentManifest(nameAtt.Value, typeAtt.Value, subTypeAtt?.Value ?? string.Empty, contextAtt?.Value ?? string.Empty, componentManifestFile);

            return ComponentManifestLoadResult.CreateSuccessfulResult(repositoryManifest, componentManifestFile, rootElement, componentManifest);
        }
    }
}
