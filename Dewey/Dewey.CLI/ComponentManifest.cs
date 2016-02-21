using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.CLI
{
    class ComponentManifest
    {
        public string Name { get; private set; }

        public string Type { get; private set; }

        public ComponentManifest(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
