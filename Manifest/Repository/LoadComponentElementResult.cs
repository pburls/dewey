using Dewey.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.Manifest.Repository
{
    public class LoadComponentElementResult
    {
        public IEnumerable<string> MissingAttributes { get; private set; }
        public ComponentItem ComponentItem { get; private set; }
        public XElement ComponentElement { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool IsSuccessful { get; private set; }

        private LoadComponentElementResult(bool isSuccessful, XElement componentElement, ComponentItem componentItem, IEnumerable<string> missingAttributes)
        {
            IsSuccessful = isSuccessful;
            ComponentElement = componentElement;
            ComponentItem = componentItem;
            MissingAttributes = missingAttributes;
            ErrorMessage = GetErrorMessage();
        }

        public static LoadComponentElementResult CreateMissingAttributesResult(XElement componentElement, IEnumerable<string> missingAttributes)
        {
            return new LoadComponentElementResult(false, componentElement, null, missingAttributes);
        }

        internal static LoadComponentElementResult CreateSuccessfulResult(XElement componentElement, ComponentItem componentItem)
        {
            return new LoadComponentElementResult(true, componentElement, componentItem, null);
        }

        private string GetErrorMessage()
        {
            if (MissingAttributes != null && MissingAttributes.Any())
            {
                return string.Format("Component element '{0}' is missing the following attributes: {1}", ComponentElement, string.Join(", ", MissingAttributes));
            }

            return null;
        }
    }
}
