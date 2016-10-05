using Dewey.Messaging;
using System;

namespace Dewey.Build.Events
{
    public abstract class BuildCommandEvent : IEvent, IEquatable<BuildCommandEvent>
    {
        public string ComponentName { get; protected set; }

        public BuildCommandEvent(BuildCommand command)
        {
            ComponentName = command.ComponentName;
        }

        public bool Equals(BuildCommandEvent other)
        {
            if (other == null) return false;

            return ComponentName == other.ComponentName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            BuildCommandEvent other = obj as BuildCommandEvent;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return ComponentName.GetHashCode();
        }

        public static bool operator ==(BuildCommandEvent a, BuildCommandEvent b)
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

        public static bool operator !=(BuildCommandEvent a, BuildCommandEvent b)
        {
            return !(a == b);
        }
    }
}
