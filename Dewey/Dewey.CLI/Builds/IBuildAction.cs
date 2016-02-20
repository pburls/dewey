using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.CLI.Builds
{
    interface IBuildAction
    {
        void Build(string target);
    }
}
