using Dewey.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Repository
{
    public class LoadManifestFileElementResult
    {
        public IEnumerable<string> MissingAttributes { get; private set; }
        public ManifestFileItem ManifestFileItem { get; private set; }
        public XElement ManifestFileElement { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool IsSuccessful { get; private set; }

        private LoadManifestFileElementResult(bool isSuccessful, XElement manifestFileElement, ManifestFileItem manifestFileItem, IEnumerable<string> missingAttributes)
        {
            IsSuccessful = isSuccessful;
            ManifestFileElement = manifestFileElement;
            ManifestFileItem = manifestFileItem;
            MissingAttributes = missingAttributes;
            ErrorMessage = GetErrorMessage();
        }

        public static LoadManifestFileElementResult LoadManifestFileElement(XElement manifestFileElement, RepositoryManifest repositoryManifest)
        {
            var missingAttributes = new List<string>();

            var nameAtt = manifestFileElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
            if (nameAtt == null || string.IsNullOrWhiteSpace(nameAtt.Value))
            {
                missingAttributes.Add("name");
            }

            var locationAtt = manifestFileElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "location");
            if (locationAtt == null || string.IsNullOrWhiteSpace(locationAtt.Value))
            {
                missingAttributes.Add("location");
            }

            if (missingAttributes.Any())
            {
                return CreateMissingAttributesResult(manifestFileElement, missingAttributes);
            }

            var manifestFileItem = new ManifestFileItem(nameAtt.Value, locationAtt.Value, repositoryManifest);

            return CreateSuccessfulResult(manifestFileElement, manifestFileItem);
        }

        public static LoadManifestFileElementResult CreateMissingAttributesResult(XElement manifestFileElement, IEnumerable<string> missingAttributes)
        {
            return new LoadManifestFileElementResult(false, manifestFileElement, null, missingAttributes);
        }

        internal static LoadManifestFileElementResult CreateSuccessfulResult(XElement manifestFileElement, ManifestFileItem manifestFileItem)
        {
            return new LoadManifestFileElementResult(true, manifestFileElement, manifestFileItem, null);
        }

        private string GetErrorMessage()
        {
            if (MissingAttributes != null && MissingAttributes.Any())
            {
                return string.Format("ManifestFile element '{0}' is missing the following attributes: {1}", ManifestFileElement, string.Join(", ", MissingAttributes));
            }

            return null;
        }
    }
}
