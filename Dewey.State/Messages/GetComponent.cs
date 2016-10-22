using Dewey.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.State.Messages
{
    public class GetComponent : ICommand, IEquatable<GetComponent>
    {
        public string ComponentName { get; protected set; }

        public GetComponent(string componentName)
        {
            ComponentName = componentName;
        }

        public bool Equals(GetComponent other)
        {
            if (other == null) return false;

            return ComponentName == other.ComponentName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            GetComponent other = obj as GetComponent;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return ComponentName.GetHashCode();
        }

        public static bool operator ==(GetComponent a, GetComponent b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ComponentName == b.ComponentName;
        }

        public static bool operator !=(GetComponent a, GetComponent b)
        {
            return !(a == b);
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

        public GetComponentResult WithCommand(GetComponent command)
        {
            return new GetComponentResult(command, Component);
        }

        public GetComponentResult WithComponent(Component component)
        {
            return new GetComponentResult(Command, component);
        }
    }
}
