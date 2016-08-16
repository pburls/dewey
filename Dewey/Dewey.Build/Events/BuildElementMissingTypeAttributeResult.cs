using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dewey.Build.Events
{
    public class BuildElementMissingTypeAttributeResult : BuildCommandEventBase
    {
        public XElement BuildElement { get; private set; }

        public BuildElementMissingTypeAttributeResult(string componentName, XElement buildElement) : base(componentName)
        {
            BuildElement = buildElement;
        }
    }
}
