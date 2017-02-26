using Dewey.File;
using Dewey.Manifest.Repository;
using Dewey.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.RuntimeResources
{
    public class RuntimeResourcesManifestLoadResult : IEvent
    {
        public RuntimeResourcesManifest RuntimeResourcesManifest { get; private set; }

        public RepositoryManifest RepositoryManifest { get; private set; }
        
        public IManifestFileReader ManifestFile { get; private set; }

        public XElement Element { get; private set; }

        public IEnumerable<string> MissingAttributes { get; private set; }

        public bool IsSuccessful { get; private set; }

        public string ErrorMessage { get; private set; }

        public IEnumerable<RuntimeResourceItemLoadResult> RuntimeResourceItemLoadResults { get; private set; }

        private RuntimeResourcesManifestLoadResult(bool isSuccessful, RepositoryManifest repositoryManifest, IManifestFileReader manifestFile, XElement element, IEnumerable<string> missingAttributes, RuntimeResourcesManifest runtimeResourcesManifest, IEnumerable<RuntimeResourceItemLoadResult> runtimeResourceItemLoadResults)
        {
            IsSuccessful = isSuccessful;
            RepositoryManifest = repositoryManifest;
            ManifestFile = manifestFile;
            Element = element;
            MissingAttributes = missingAttributes;
            RuntimeResourcesManifest = runtimeResourcesManifest;
            RuntimeResourceItemLoadResults = runtimeResourceItemLoadResults ?? new List<RuntimeResourceItemLoadResult>();
            ErrorMessage = GetErrorMessage();
        }

        internal static RuntimeResourcesManifestLoadResult CreateFileNotFoundResult(RepositoryManifest repositoryManifest, IManifestFileReader manifestFile)
        {
            return new RuntimeResourcesManifestLoadResult(false, repositoryManifest, manifestFile, null, null, null, null);
        }

        internal static RuntimeResourcesManifestLoadResult CreateMissingAttributesResult(RepositoryManifest repositoryManifest, IManifestFileReader manifestFile, XElement element, List<string> missingAttributes)
        {
            return new RuntimeResourcesManifestLoadResult(false, repositoryManifest, manifestFile, element, missingAttributes, null, null);
        }

        internal static RuntimeResourcesManifestLoadResult CreateSuccessfulResult(RepositoryManifest repositoryManifest, IManifestFileReader manifestFile, XElement element, RuntimeResourcesManifest runtimeResourcesManifest, IEnumerable<RuntimeResourceItemLoadResult> runtimeResourceItemLoadResults)
        {
            return new RuntimeResourcesManifestLoadResult(true, repositoryManifest, manifestFile, element, null, runtimeResourcesManifest, runtimeResourceItemLoadResults);
        }

        private string GetErrorMessage()
        {
            if (!ManifestFile.DirectoryExists) return string.Format("Directory '{0}' not found.", ManifestFile.DirectoryName);
            if (!ManifestFile.FileExists) return string.Format("Manifest file '{0}' not found.", ManifestFile.FileName);

            if (MissingAttributes != null && MissingAttributes.Any())
            {
                return string.Format("Element '{0}' is missing the following attributes: {1}", Element, string.Join(", ", MissingAttributes));
            }

            return null;
        }
    }
}