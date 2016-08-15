using Dewey.File;
using Dewey.Manifest.Repository;
using Dewey.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Component
{
    public class ComponentManifestLoadResult : IEvent
    {
        public ComponentManifest ComponentManifest { get; private set; }

        public RepositoryManifest RepositoryManifest { get; private set; }
        
        public IManifestFileReader ComponentManifestFile { get; private set; }

        public XElement ComponentElement { get; private set; }

        public IEnumerable<string> MissingAttributes { get; private set; }

        public bool IsSuccessful { get; private set; }

        public string ErrorMessage { get; private set; }

        private ComponentManifestLoadResult(bool isSuccessful, RepositoryManifest repositoryManifest, IManifestFileReader componentManifestFile, XElement componentElement, IEnumerable<string> missingAttributes, ComponentManifest componentManifest)
        {
            IsSuccessful = isSuccessful;
            RepositoryManifest = repositoryManifest;
            ComponentManifestFile = componentManifestFile;
            ComponentElement = componentElement;
            MissingAttributes = missingAttributes;
            ComponentManifest = componentManifest;
            ErrorMessage = GetErrorMessage();
        }

        internal static ComponentManifestLoadResult CreateFileNotFoundResult(RepositoryManifest repositoryManifest, IManifestFileReader componentManifestFile)
        {
            return new ComponentManifestLoadResult(false, repositoryManifest, componentManifestFile, null, null, null);
        }

        internal static ComponentManifestLoadResult CreateMissingAttributesResult(RepositoryManifest repositoryManifest, IManifestFileReader componentManifestFile, XElement componentElement, List<string> missingAttributes)
        {
            return new ComponentManifestLoadResult(false, repositoryManifest, componentManifestFile, componentElement, missingAttributes, null);
        }

        internal static ComponentManifestLoadResult CreateSuccessfulResult(RepositoryManifest repositoryManifest, IManifestFileReader componentManifestFile, ComponentManifest componentManifest)
        {
            return new ComponentManifestLoadResult(true, repositoryManifest, componentManifestFile, null, null, componentManifest);
        }

        private string GetErrorMessage()
        {
            if (!ComponentManifestFile.DirectoryExists) return string.Format("Component directory '{0}' not found.", ComponentManifestFile.DirectoryName);
            if (!ComponentManifestFile.FileExists) return string.Format("Component Manifest file '{0}' not found.", ComponentManifestFile.FileName);

            if (MissingAttributes != null && MissingAttributes.Any())
            {
                return string.Format("Component element '{0}' is missing the following attributes: {1}", ComponentElement, string.Join(", ", MissingAttributes));
            }

            return null;
        }
    }
}