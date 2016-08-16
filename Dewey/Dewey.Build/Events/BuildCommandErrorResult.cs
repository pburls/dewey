using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Build.Events
{
    public class BuildCommandErrorResult : BuildCommandEventBase
    {
        public Exception Exception { get; private set; }

        public BuildCommandErrorResult(string componentName, Exception exception) : base(componentName)
        {
            Exception = exception;
        }
    }
}
