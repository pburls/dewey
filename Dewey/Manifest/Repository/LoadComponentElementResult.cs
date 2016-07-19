using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dewey.Manifest.Component;

namespace Dewey.Manifest.Repository
{
    public class LoadComponentElementResult
    {
        public IEnumerable<string> MissingAttributes { get; private set; }
        public ComponentItem ComponentItem { get; private set; }
        public XElement ComponentElement { get; private set; }
        //public LoadComponentItemResult LoadComponentItemResult { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool IsSuccessful { get; private set; }

        private LoadComponentElementResult(bool isSuccessful, XElement componentElement, ComponentItem componentItem, IEnumerable<string> missingAttributes/*, LoadComponentItemResult loadComponentItemResult*/)
        {
            IsSuccessful = isSuccessful;
            ComponentElement = componentElement;
            ComponentItem = componentItem;
            MissingAttributes = missingAttributes;
            //LoadComponentItemResult = loadComponentItemResult;
            ErrorMessage = GetErrorMessage();
        }

        public static LoadComponentElementResult CreateMissingAttributesResult(XElement componentElement, IEnumerable<string> missingAttributes)
        {
            return new LoadComponentElementResult(false, componentElement, null, missingAttributes/*, null*/);
        }

        internal static LoadComponentElementResult CreateSuccessfulResult(XElement componentElement, ComponentItem componentItem/*, LoadComponentItemResult loadComponentItemResult*/)
        {
            return new LoadComponentElementResult(true, componentElement, componentItem, null/*, loadComponentItemResult*/);
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
