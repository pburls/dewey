using Dewey.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.RuntimeResources
{
    public class RuntimeResourceItemLoadResult : IEvent
    {
        public RuntimeResourcesManifest RuntimeResourcesManifest { get; private set; }

        public RuntimeResourceItem RuntimeResourceItem { get; private set; }

        public XElement Element { get; private set; }

        public IEnumerable<string> MissingAttributes { get; private set; }

        public bool IsSuccessful { get; private set; }

        public string ErrorMessage { get; private set; }

        private RuntimeResourceItemLoadResult(bool isSuccessful, RuntimeResourcesManifest runtimeResourcesManifest, XElement element, IEnumerable<string> missingAttributes, RuntimeResourceItem runtimeResourceItem)
        {
            IsSuccessful = isSuccessful;
            RuntimeResourceItem = runtimeResourceItem;
            Element = element;
            MissingAttributes = missingAttributes;
            RuntimeResourcesManifest = runtimeResourcesManifest;
            ErrorMessage = GetErrorMessage();
        }

        internal static RuntimeResourceItemLoadResult CreateMissingAttributesResult(RuntimeResourcesManifest runtimeResourcesManifest, XElement element, List<string> missingAttributes)
        {
            return new RuntimeResourceItemLoadResult(false, runtimeResourcesManifest, element, missingAttributes, null);
        }

        internal static RuntimeResourceItemLoadResult CreateSuccessfulResult(RuntimeResourcesManifest runtimeResourcesManifest, XElement element, RuntimeResourceItem runtimeResourceItem)
        {
            return new RuntimeResourceItemLoadResult(true, runtimeResourcesManifest, element, null, runtimeResourceItem);
        }

        private string GetErrorMessage()
        {
            if (MissingAttributes != null && MissingAttributes.Any())
            {
                return string.Format("Element '{0}' is missing the following attributes: {1}", Element, string.Join(", ", MissingAttributes));
            }

            return null;
        }
    }
}
