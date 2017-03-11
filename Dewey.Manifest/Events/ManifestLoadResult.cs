using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dewey.File;
using System.Xml.Linq;

namespace Dewey.Manifest.Events
{
    public class ManifestLoadResult : ManifestFileEvent
    {
        public XElement ManifestFilesElement { get; private set; }
        public XElement ComponentsElement { get; private set; }
        public XElement RuntimeResourcesElement { get; private set; }

        public ManifestLoadResult(IManifestFileReader manifestFile, XElement manifestFilesElement, XElement componentsElement, XElement runtimeResourcesElement) : base(manifestFile)
        {
            ManifestFilesElement = manifestFilesElement;
            ComponentsElement = componentsElement;
            RuntimeResourcesElement = runtimeResourcesElement;
        }
    }
}
