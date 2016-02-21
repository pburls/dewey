using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.CLI
{
    class RepositoryComponent
    {
        public string Name { get; private set; }

        public string Location { get; private set; }

        public RepositoryComponent(string name, string location)
        {
            this.Name = name;
            this.Location = location;
        }
    }
}
