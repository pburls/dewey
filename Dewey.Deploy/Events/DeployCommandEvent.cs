using Ark3.Event;

namespace Dewey.Deploy.Events
{
    public abstract class DeployCommandEvent : IEvent
    {
        public string ComponentName { get; protected set; }

        public DeployCommandEvent(DeployCommand command)
        {
            ComponentName = command.ComponentName;
        }
    }
}
