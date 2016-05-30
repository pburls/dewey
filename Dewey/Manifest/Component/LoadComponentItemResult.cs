using System;
using System.Collections.Generic;
using System.IO;
using Dewey.Manifest.Repository;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Component
{
    public class LoadComponentItemResult
    {
        public ComponentItem ComponentItem { get; private set; }

        public DirectoryInfo ComponentDirectory { get; private set; }

        public FileInfo ComponentManifestFile { get; private set; }

        public ComponentManifest ComponentManifest { get; private set; }

        public XElement ComponentElement { get; private set; }

        public IEnumerable<string> MissingAttributes { get; private set; }

        public string ErrorMessage { get; private set; }

        private LoadComponentItemResult(ComponentItem componentItem, DirectoryInfo componentDirectory, FileInfo componentManifestFile, XElement componentElement, IEnumerable<string> missingAttributes, ComponentManifest componentManifest)
        {
            ComponentItem = componentItem;
            ComponentDirectory = componentDirectory;
            ComponentManifestFile = componentManifestFile;
            ComponentElement = componentElement;
            MissingAttributes = missingAttributes;
            ComponentManifest = componentManifest;
            ErrorMessage = GetErrorMessage();
        }

        internal static LoadComponentItemResult CreateDirectoryNotFoundResult(ComponentItem componentItem, DirectoryInfo componentDirectory)
        {
            return new LoadComponentItemResult(componentItem, componentDirectory, null, null, null, null);
        }

        internal static LoadComponentItemResult CreateFileNotFoundResult(ComponentItem componentItem, DirectoryInfo componentDirectory, FileInfo componentManifestFile)
        {
            return new LoadComponentItemResult(componentItem, componentDirectory, componentManifestFile, null, null, null);
        }

        internal static LoadComponentItemResult CreateMissingAttributesResult(ComponentItem componentItem, DirectoryInfo componentDirectory, FileInfo componentManifestFile, XElement componentElement, List<string> missingAttributes)
        {
            return new LoadComponentItemResult(componentItem, componentDirectory, componentManifestFile, componentElement, missingAttributes, null);
        }

        internal static LoadComponentItemResult CreateSuccessfulResult(ComponentItem componentItem, DirectoryInfo componentDirectory, FileInfo componentManifestFile, ComponentManifest componentManifest)
        {
            return new LoadComponentItemResult(componentItem, componentDirectory, componentManifestFile, null, null, componentManifest);
        }

        private string GetErrorMessage()
        {
            if (!ComponentDirectory.Exists) return string.Format("Component directory '{0}' not found.", ComponentDirectory.FullName);
            if (!ComponentManifestFile.Exists) return string.Format("Component Manifest file '{0}' not found.", ComponentManifestFile.FullName);

            if (MissingAttributes != null && MissingAttributes.Any())
            {
                return string.Format("Component element '{0}' is missing the following attributes: {1}", ComponentElement, string.Join(", ", MissingAttributes));
            }

            return null;
        }
    }
}