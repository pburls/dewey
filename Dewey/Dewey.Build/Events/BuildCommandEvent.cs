using Dewey.Messaging;

namespace Dewey.Build.Events
{
    public abstract class BuildCommandEvent : IEvent
    {
        public string ComponentName { get; protected set; }

        public BuildCommandEvent(BuildCommand command)
        {
            ComponentName = command.ComponentName;
        }
    }
}
