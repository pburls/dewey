using Dewey.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.State.Messages
{
    public class GetComponent : ICommand
    {
        public string ComponentName { get; private set; }

        public GetComponent(string componentName)
        {
            ComponentName = componentName;
        }
    }

    public class GetComponentResult : IEvent
    {
        public GetComponent Command { get; private set; }
        public Component Component { get; private set; }

        public GetComponentResult(GetComponent command, Component component)
        {
            Command = command;
            Component = component;
        }
    }
}
