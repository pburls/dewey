using Ark3.Command;
using Dewey.Messaging;
using System;

namespace Dewey.Build.Events
{
    public class BuildCommandCompleted : BuildCommandEvent, IEquatable<BuildCommandCompleted>, ICommandCompleteEvent
    {
        public ICommand Command { get; private set; }
        public bool IsSuccessful { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }

        public BuildCommandCompleted(BuildCommand command, bool isSuccessful, TimeSpan elapsedTime) : base(command)
        {
            Command = command;
            IsSuccessful = isSuccessful;
            ElapsedTime = elapsedTime;
        }

        public bool Equals(BuildCommandCompleted other)
        {
            if (other == null) return false;

            return base.Equals(other)
                && Command == other.Command
                && IsSuccessful == other.IsSuccessful 
                && ElapsedTime == other.ElapsedTime;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            BuildCommandCompleted other = obj as BuildCommandCompleted;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Command.GetHashCode() ^ IsSuccessful.GetHashCode() ^ ElapsedTime.GetHashCode();
        }

        public static bool operator ==(BuildCommandCompleted a, BuildCommandCompleted b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.ComponentName == b.ComponentName
                && a.Command == b.Command
                && a.IsSuccessful == b.IsSuccessful 
                && a.ElapsedTime == b.ElapsedTime;
        }

        public static bool operator !=(BuildCommandCompleted a, BuildCommandCompleted b)
        {
            return !(a == b);
        }
    }
}
