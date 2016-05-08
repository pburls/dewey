using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dewey.CLI.Repository
{
    public class LoadComponentElementResult
    {
        public IEnumerable<string> MissingAttributes { get; private set; }
        public ComponentItem ComponentItem { get; private set; }
        public XElement ComponentElement { get; private set; }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                if (_errorMessage == null)
                {
                    _errorMessage = GetErrorMessage();
                }

                return _errorMessage;
            }
        }

        public static LoadComponentElementResult CreateMissingAttributesResult(XElement componentElement, ComponentItem componentItem, IEnumerable<string> missingAttributes)
        {
            var result = new LoadComponentElementResult();
            result.ComponentElement = componentElement;
            result.ComponentItem = componentItem;
            result.MissingAttributes = missingAttributes;
            return result;
        }

        public static LoadComponentElementResult CreateSuccessfulResult(XElement componentElement, ComponentItem componentItem)
        {
            var result = new LoadComponentElementResult();
            result.ComponentElement = componentElement;
            result.ComponentItem = componentItem;
            return result;
        }

        private string GetErrorMessage()
        {
            if (ComponentItem == null)
            {
                return "Repository element without a valid name: " + ComponentElement.ToString();
            }

            if (ComponentItem != null && MissingAttributes.Any())
            {
                return string.Format("Component element '{0}' is missing the following attributes: {1}", ComponentItem.Name, string.Join(", ", MissingAttributes));
            }

            return null;
        }
    }
}
