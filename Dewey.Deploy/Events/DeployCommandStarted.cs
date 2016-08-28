using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.Deploy.Events
{
    public class DeployCommandStarted : DeployCommandEvent
    {
        public DeployCommandStarted(DeployCommand command) : base(command)
        {

        }
    }
}
