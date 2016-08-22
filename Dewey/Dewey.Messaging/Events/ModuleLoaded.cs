using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Messaging.Events
{
    public class ModuleLoaded : IEvent
    {
        public IModule Module { get; private set; }

        public ModuleLoaded(IModule module)
        {
            Module = module;
        }
    }
}
