using System;
using System.Collections.Generic;
using System.IO;
using Dewey.Manifest.Repository;
using System.Linq;
using System.Xml.Linq;
using Dewey.Manfiest;

namespace Dewey.Manifest.Component
{
    public class LoadComponentItemResult
    {
        public ComponentItem ComponentItem { get; private set; }
        
        public XmlFileLoader ComponentManifestFile { get; private set; }

        public ComponentManifest ComponentManifest { get; private set; }

        public XElement ComponentElement { get; private set; }

        public IEnumerable<string> MissingAttributes { get; private set; }

        public string ErrorMessage { get; private set; }

        private LoadComponentItemResult(ComponentItem componentItem, XmlFileLoader componentManifestFile, XElement componentElement, IEnumerable<string> missingAttributes, ComponentManifest componentManifest)
        {
            ComponentItem = componentItem;
            ComponentManifestFile = componentManifestFile;
            ComponentElement = componentElement;
            MissingAttributes = missingAttributes;
            ComponentManifest = componentManifest;
            ErrorMessage = GetErrorMessage();
        }

        internal static LoadComponentItemResult CreateFileNotFoundResult(ComponentItem componentItem, XmlFileLoader componentManifestFile)
        {
            return new LoadComponentItemResult(componentItem, componentManifestFile, null, null, null);
        }

        internal static LoadComponentItemResult CreateMissingAttributesResult(ComponentItem componentItem, XmlFileLoader componentManifestFile, XElement componentElement, List<string> missingAttributes)
        {
            return new LoadComponentItemResult(componentItem, componentManifestFile, componentElement, missingAttributes, null);
        }

        internal static LoadComponentItemResult CreateSuccessfulResult(ComponentItem componentItem, XmlFileLoader componentManifestFile, ComponentManifest componentManifest)
        {
            return new LoadComponentItemResult(componentItem, componentManifestFile, null, null, componentManifest);
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