using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dewey.Build.Events
{
    public class BuildElementMissingTypeAttributeResult : BuildCommandEvent
    {
        public XElement BuildElement { get; private set; }

        public BuildElementMissingTypeAttributeResult(BuildCommand command, XElement buildElement) : base(command)
        {
            BuildElement = buildElement;
        }
    }
}
