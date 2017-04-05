using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Build
{
    public interface IBuildCommandCache
    {
        bool IsComponentAlreadyBuilt(string component);
    }
}
